import CodeWindow from '../components/CodeWindow';
import styles from './AdvancedUsage.module.css';

function AdvancedUsage() {
  const dependencyInjectionWindows = [
    {
      fileName: 'Startup Configuration',
      language: 'csharp',
      snipt: `// Program.cs or Startup.cs
using EasyValidate;
using EasyValidate.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add EasyValidate services
builder.Services.AddEasyValidate(options =>
{
    options.EnableDetailedErrors = true;
    options.StopOnFirstFailure = false;
    options.CascadeMode = CascadeMode.Continue;
});

// Register custom validators
builder.Services.AddScoped<IValidator<User>, UserValidator>();
builder.Services.AddScoped<IValidator<Order>, OrderValidator>();

var app = builder.Build();`
    },
    {
      fileName: 'Controller Usage',
      language: 'csharp',
      snipt: `[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    private readonly IValidator<User> _userValidator;
    private readonly IValidator<CreateUserRequest> _createUserValidator;

    public UsersController(
        IValidator<User> userValidator,
        IValidator<CreateUserRequest> createUserValidator)
    {
        _userValidator = userValidator;
        _createUserValidator = createUserValidator;
    }

    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var validationResult = await _createUserValidator.ValidateAsync(request);
        
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.ToDictionary());
        }

        // Process valid request
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Age = request.Age
        };

        return Ok(user);
    }
}`
    }
  ];

  const customValidatorCode = {
    'Complex Validator': `public class OrderValidator : AbstractValidator<Order>
{
    private readonly IProductService _productService;
    private readonly IUserService _userService;

    public OrderValidator(IProductService productService, IUserService userService)
    {
        _productService = productService;
        _userService = userService;

        RuleFor(x => x.CustomerId)
            .NotEmpty()
            .MustAsync(async (customerId, cancellation) => 
                await _userService.ExistsAsync(customerId))
            .WithMessage("Customer does not exist");

        RuleFor(x => x.Items)
            .NotEmpty()
            .WithMessage("Order must contain at least one item");

        RuleForEach(x => x.Items)
            .SetValidator(new OrderItemValidator(_productService));

        RuleFor(x => x.TotalAmount)
            .Must((order, totalAmount) => 
                Math.Abs(totalAmount - order.Items.Sum(i => i.Price * i.Quantity)) < 0.01m)
            .WithMessage("Total amount does not match sum of item prices");

        When(x => x.ShippingMethod == ShippingMethod.Express, () =>
        {
            RuleFor(x => x.ExpressDeliveryDate)
                .NotEmpty()
                .Must(date => date >= DateTime.Today.AddDays(1))
                .WithMessage("Express delivery date must be at least tomorrow");
        });
    }
}`,
    'Item Validator': `public class OrderItemValidator : AbstractValidator<OrderItem>
{
    private readonly IProductService _productService;

    public OrderItemValidator(IProductService productService)
    {
        _productService = productService;

        RuleFor(x => x.ProductId)
            .NotEmpty()
            .MustAsync(async (productId, cancellation) => 
                await _productService.IsAvailableAsync(productId))
            .WithMessage("Product is not available");

        RuleFor(x => x.Quantity)
            .GreaterThan(0)
            .LessThanOrEqualTo(100)
            .WithMessage("Quantity must be between 1 and 100");

        RuleFor(x => x.Price)
            .GreaterThan(0)
            .MustAsync(async (item, price, cancellation) =>
            {
                var product = await _productService.GetByIdAsync(item.ProductId);
                return Math.Abs(price - product.Price) < 0.01m;
            })
            .WithMessage("Price does not match product price");
    }
}`
  };

  const conditionalValidationCode = {
    'Dynamic Rules': `public class UserProfileValidator : AbstractValidator<UserProfile>
{
    public UserProfileValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress();

        // Conditional validation based on user type
        When(x => x.UserType == UserType.Business, () =>
        {
            RuleFor(x => x.CompanyName)
                .NotEmpty()
                .WithMessage("Company name is required for business users");

            RuleFor(x => x.TaxId)
                .NotEmpty()
                .Matches(@"^[0-9]{2}-[0-9]{7}$")
                .WithMessage("Tax ID must be in format XX-XXXXXXX");
        });

        When(x => x.UserType == UserType.Individual, () =>
        {
            RuleFor(x => x.FirstName)
                .NotEmpty()
                .WithMessage("First name is required for individual users");

            RuleFor(x => x.LastName)
                .NotEmpty()
                .WithMessage("Last name is required for individual users");
        });

        // Conditional validation based on age
        When(x => x.Age < 18, () =>
        {
            RuleFor(x => x.ParentEmail)
                .NotEmpty()
                .EmailAddress()
                .WithMessage("Parent email is required for users under 18");
        });

        // Complex conditional logic
        When(x => x.Country == "US" && x.StateProvince != null, () =>
        {
            RuleFor(x => x.StateProvince)
                .Must(BeValidUSState)
                .WithMessage("Please select a valid US state");

            RuleFor(x => x.ZipCode)
                .Matches(@"^[0-9]{5}(-[0-9]{4})?$")
                .WithMessage("Please enter a valid US ZIP code");
        });
    }

    private bool BeValidUSState(string state)
    {
        var validStates = new[] { "AL", "AK", "AZ", "AR", "CA", "CO", /* ... */ };
        return validStates.Contains(state);
    }
}`
  };

  const asyncValidationCode = {
    'Async Database Validation': `public class UserRegistrationValidator : AbstractValidator<UserRegistrationModel>
{
    private readonly IUserRepository _userRepository;
    private readonly IEmailService _emailService;

    public UserRegistrationValidator(IUserRepository userRepository, IEmailService emailService)
    {
        _userRepository = userRepository;
        _emailService = emailService;

        RuleFor(x => x.Username)
            .NotEmpty()
            .MinimumLength(3)
            .MaximumLength(50)
            .MustAsync(async (username, cancellation) =>
                !await _userRepository.UsernameExistsAsync(username))
            .WithMessage("Username '{PropertyValue}' is already taken");

        RuleFor(x => x.Email)
            .NotEmpty()
            .EmailAddress()
            .MustAsync(async (email, cancellation) =>
                !await _userRepository.EmailExistsAsync(email))
            .WithMessage("Email '{PropertyValue}' is already registered")
            .MustAsync(async (email, cancellation) =>
                await _emailService.IsValidDomainAsync(email))
            .WithMessage("Email domain is not allowed");

        RuleFor(x => x.Password)
            .NotEmpty()
            .MinimumLength(8)
            .Must(ContainUppercase)
            .WithMessage("Password must contain at least one uppercase letter")
            .Must(ContainLowercase)
            .WithMessage("Password must contain at least one lowercase letter")
            .Must(ContainDigit)
            .WithMessage("Password must contain at least one digit")
            .Must(ContainSpecialCharacter)
            .WithMessage("Password must contain at least one special character");
    }

    private bool ContainUppercase(string password) => password.Any(char.IsUpper);
    private bool ContainLowercase(string password) => password.Any(char.IsLower);
    private bool ContainDigit(string password) => password.Any(char.IsDigit);
    private bool ContainSpecialCharacter(string password) => 
        password.Any(c => "!@#$%^&*()_+-=[]{}|;:,.<>?".Contains(c));
}`,
    'Usage with Cancellation': `public async Task<ValidationResult> ValidateUserAsync(
    UserRegistrationModel user, 
    CancellationToken cancellationToken = default)
{
    var validator = new UserRegistrationValidator(_userRepository, _emailService);
    return await validator.ValidateAsync(user, cancellationToken);
}`
  };

  const performanceCode = {
    'Caching Validators': `// Register validators as singletons for better performance
services.AddSingleton<IValidator<User>, UserValidator>();
services.AddSingleton<IValidator<Product>, ProductValidator>();

// Use factory pattern for validators with dependencies
services.AddScoped<IValidatorFactory, ValidatorFactory>();

public class ValidatorFactory : IValidatorFactory
{
    private readonly IServiceProvider _serviceProvider;
    
    public ValidatorFactory(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public IValidator<T> GetValidator<T>()
    {
        return _serviceProvider.GetService<IValidator<T>>();
    }
}`,
    'Batch Validation': `public async Task<Dictionary<int, ValidationResult>> ValidateBatchAsync<T>(
    IEnumerable<(int Id, T Item)> items,
    IValidator<T> validator,
    CancellationToken cancellationToken = default)
{
    var tasks = items.Select(async item =>
    {
        var result = await validator.ValidateAsync(item.Item, cancellationToken);
        return new { item.Id, Result = result };
    });

    var results = await Task.WhenAll(tasks);
    
    return results.ToDictionary(x => x.Id, x => x.Result);
}`,
    'Parallel Validation': `public async Task<ValidationResult> ValidateInParallelAsync<T>(
    T instance,
    params IValidator<T>[] validators)
{
    var tasks = validators.Select(validator => validator.ValidateAsync(instance));
    var results = await Task.WhenAll(tasks);

    var combinedResult = new ValidationResult();
    foreach (var result in results)
    {
        if (!result.IsValid)
        {
            combinedResult.Errors.AddRange(result.Errors);
        }
    }

    return combinedResult;
}`
  };

  const testingCode = {
    'Unit Testing': `[Test]
public async Task UserValidator_Should_Fail_When_Email_Already_Exists()
{
    // Arrange
    var mockUserRepository = new Mock<IUserRepository>();
    mockUserRepository.Setup(x => x.EmailExistsAsync("test@example.com"))
                     .ReturnsAsync(true);

    var validator = new UserRegistrationValidator(mockUserRepository.Object, null);
    var user = new UserRegistrationModel
    {
        Username = "testuser",
        Email = "test@example.com",
        Password = "Password123!"
    };

    // Act
    var result = await validator.ValidateAsync(user);

    // Assert
    Assert.IsFalse(result.IsValid);
    Assert.That(result.Errors.Any(e => e.PropertyName == "Email"));
    Assert.That(result.Errors.First(e => e.PropertyName == "Email").ErrorMessage,
               Contains.Substring("already registered"));
}`,
    'Integration Testing': `[Test]
public async Task UserRegistration_Should_Validate_Successfully_With_Valid_Data()
{
    // Arrange
    var factory = new WebApplicationFactory<Program>();
    var client = factory.CreateClient();

    var registrationData = new
    {
        Username = "newuser",
        Email = "newuser@example.com",
        Password = "SecurePassword123!",
        FirstName = "John",
        LastName = "Doe"
    };

    var json = JsonSerializer.Serialize(registrationData);
    var content = new StringContent(json, Encoding.UTF8, "application/json");

    // Act
    var response = await client.PostAsync("/api/users/register", content);

    // Assert
    Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
}`
  };

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1 className={styles.title}>Advanced Usage</h1>
        <p className={styles.subtitle}>
          Explore advanced patterns, dependency injection, custom validators, and performance optimization techniques
        </p>
      </div>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>🏗️ Dependency Injection</h2>
        <p className={styles.text}>
          EasyValidate integrates seamlessly with .NET's dependency injection container for scalable applications:
        </p>
        <CodeWindow 
          code={dependencyInjectionCode}
          title="Dependency Injection Setup"
          language="csharp"
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>🎯 Custom Validators</h2>
        <p className={styles.text}>
          Create sophisticated validation logic with custom validators that can access external services:
        </p>
        <CodeWindow 
          code={customValidatorCode}
          title="Complex Custom Validators"
          language="csharp"
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>🔀 Conditional Validation</h2>
        <p className={styles.text}>
          Implement dynamic validation rules that adapt based on object state and business logic:
        </p>
        <CodeWindow 
          code={conditionalValidationCode}
          title="Dynamic Conditional Validation"
          language="csharp"
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>⚡ Asynchronous Validation</h2>
        <p className={styles.text}>
          Perform async validation for database lookups, API calls, and other I/O operations:
        </p>
        <CodeWindow 
          code={asyncValidationCode}
          title="Async Validation Examples"
          language="csharp"
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>🚀 Performance Optimization</h2>
        <p className={styles.text}>
          Optimize validation performance for high-throughput scenarios:
        </p>
        <CodeWindow 
          code={performanceCode}
          title="Performance Optimization Techniques"
          language="csharp"
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>🧪 Testing Strategies</h2>
        <p className={styles.text}>
          Best practices for testing custom validators and validation logic:
        </p>
        <CodeWindow 
          code={testingCode}
          title="Testing Validation Logic"
          language="csharp"
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>🛠️ Advanced Patterns</h2>
        
        <div className={styles.patternGrid}>
          <div className={styles.patternCard}>
            <div className={styles.patternIcon}>🔄</div>
            <h3 className={styles.patternTitle}>Validation Pipeline</h3>
            <p className={styles.patternDescription}>
              Create validation pipelines that execute validators in sequence with short-circuiting.
            </p>
          </div>
          
          <div className={styles.patternCard}>
            <div className={styles.patternIcon}>🎭</div>
            <h3 className={styles.patternTitle}>Rule Sets</h3>
            <p className={styles.patternDescription}>
              Define different validation rule sets for different scenarios (create vs update).
            </p>
          </div>
          
          <div className={styles.patternCard}>
            <div className={styles.patternIcon}>🌍</div>
            <h3 className={styles.patternTitle}>Localization</h3>
            <p className={styles.patternDescription}>
              Implement multilingual error messages with resource files and culture-specific validation.
            </p>
          </div>
          
          <div className={styles.patternCard}>
            <div className={styles.patternIcon}>📊</div>
            <h3 className={styles.patternTitle}>Validation Metrics</h3>
            <p className={styles.patternDescription}>
              Track validation performance and failure patterns for monitoring and optimization.
            </p>
          </div>
        </div>
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>⚠️ Common Pitfalls</h2>
        
        <div className={styles.pitfallsList}>
          <div className={styles.pitfallItem}>
            <div className={styles.pitfallIcon}>❌</div>
            <div className={styles.pitfallContent}>
              <h4 className={styles.pitfallTitle}>Blocking Async Operations</h4>
              <p className={styles.pitfallText}>
                Don't use <code>.Result</code> or <code>.Wait()</code> on async validation operations. Always use <code>await</code> properly.
              </p>
            </div>
          </div>
          
          <div className={styles.pitfallItem}>
            <div className={styles.pitfallIcon}>❌</div>
            <div className={styles.pitfallContent}>
              <h4 className={styles.pitfallTitle}>Validator Instance Reuse</h4>
              <p className={styles.pitfallText}>
                Be careful when reusing validator instances across requests if they contain state or dependencies.
              </p>
            </div>
          </div>
          
          <div className={styles.pitfallItem}>
            <div className={styles.pitfallIcon}>❌</div>
            <div className={styles.pitfallContent}>
              <h4 className={styles.pitfallTitle}>Excessive Database Calls</h4>
              <p className={styles.pitfallText}>
                Cache validation results and batch database operations to avoid N+1 query problems.
              </p>
            </div>
          </div>
          
          <div className={styles.pitfallItem}>
            <div className={styles.pitfallIcon}>❌</div>
            <div className={styles.pitfallContent}>
              <h4 className={styles.pitfallTitle}>Circular Dependencies</h4>
              <p className={styles.pitfallText}>
                Avoid circular references between validators and ensure proper dependency injection registration.
              </p>
            </div>
          </div>
        </div>
      </section>
    </div>
  );
}

export default AdvancedUsage;
