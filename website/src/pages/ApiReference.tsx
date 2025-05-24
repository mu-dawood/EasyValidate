import CodeWindow from '../components/CodeWindow';
import styles from './ApiReference.module.css';

function ApiReference() {
  const validatorApiCode = {
    'IValidator<T>': `public interface IValidator<T>
{
    ValidationResult Validate(T instance);
    Task<ValidationResult> ValidateAsync(T instance, CancellationToken cancellationToken = default);
    ValidationResult Validate(T instance, string ruleSet);
    Task<ValidationResult> ValidateAsync(T instance, string ruleSet, CancellationToken cancellationToken = default);
}

// Usage
var validator = new EasyValidator<User>();
var result = validator.Validate(user);
var asyncResult = await validator.ValidateAsync(user);`,
    'AbstractValidator<T>': `public abstract class AbstractValidator<T> : IValidator<T>
{
    protected IRuleBuilderInitial<T, TProperty> RuleFor<TProperty>(Expression<Func<T, TProperty>> expression);
    protected IRuleBuilderInitialCollection<T, TElement> RuleForEach<TElement>(Expression<Func<T, IEnumerable<TElement>>> expression);
    protected void When(Func<T, bool> predicate, Action action);
    protected void Unless(Func<T, bool> predicate, Action action);
    protected void RuleSet(string ruleSetName, Action action);
}

// Example implementation
public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        RuleFor(x => x.Name).NotEmpty();
        RuleFor(x => x.Email).EmailAddress();
    }
}`
  };

  const validationResultCode = {
    'ValidationResult': `public class ValidationResult
{
    public bool IsValid { get; }
    public IList<ValidationFailure> Errors { get; }
    public IDictionary<string, string[]> ToDictionary();
    public string ToString();
}

// Usage examples
var result = validator.Validate(user);

if (!result.IsValid)
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
    }
    
    // Convert to dictionary for API responses
    var errorDict = result.ToDictionary();
    return BadRequest(errorDict);
}`,
    'ValidationFailure': `public class ValidationFailure
{
    public string PropertyName { get; set; }
    public string ErrorMessage { get; set; }
    public object AttemptedValue { get; set; }
    public object CustomState { get; set; }
    public string ErrorCode { get; set; }
    public Severity Severity { get; set; }
}

// Creating custom failures
var failure = new ValidationFailure("Email", "Email is required")
{
    ErrorCode = "EMAIL_REQUIRED",
    Severity = Severity.Error
};`
  };

  const ruleBuilderCode = {
    'Basic Rules': `// String validation
RuleFor(x => x.Name)
    .NotEmpty()
    .Length(2, 50)
    .Matches(@"^[a-zA-Z\s]+$")
    .WithMessage("Name must contain only letters and spaces");

// Numeric validation
RuleFor(x => x.Age)
    .GreaterThan(0)
    .LessThan(150)
    .WithMessage("Age must be between 1 and 149");

// Collection validation
RuleFor(x => x.Items)
    .NotEmpty()
    .Must(items => items.Count <= 10)
    .WithMessage("Maximum 10 items allowed");`,
    'Custom Rules': `// Custom predicate
RuleFor(x => x.Email)
    .Must(BeAValidEmail)
    .WithMessage("Please enter a valid email address");

// Async custom rule
RuleFor(x => x.Username)
    .MustAsync(async (username, cancellation) => 
        !await _userService.UsernameExistsAsync(username))
    .WithMessage("Username is already taken");

// Complex custom rule with context
RuleFor(x => x.Password)
    .Must((user, password) => IsValidPassword(password, user.Username))
    .WithMessage("Password cannot contain username");

private bool BeAValidEmail(string email)
{
    return !string.IsNullOrEmpty(email) && 
           email.Contains("@") && 
           email.Contains(".");
}`
  };

  const conditionalRulesCode = {
    'When/Unless': `// Conditional validation
When(x => x.IsBusinessAccount, () =>
{
    RuleFor(x => x.CompanyName).NotEmpty();
    RuleFor(x => x.TaxId).NotEmpty().Length(9);
});

Unless(x => x.IsMinor, () =>
{
    RuleFor(x => x.CreditCardNumber).CreditCard();
});

// Nested conditions
When(x => x.Country == "US", () =>
{
    When(x => x.State != null, () =>
    {
        RuleFor(x => x.ZipCode)
            .NotEmpty()
            .Matches(@"^\d{5}(-\d{4})?$");
    });
});`,
    'WhenAsync': `WhenAsync(async (x, cancellation) => 
    await _subscriptionService.HasActiveSubscriptionAsync(x.UserId), () =>
{
    RuleFor(x => x.PremiumFeatures)
        .NotEmpty()
        .WithMessage("Premium features require active subscription");
});`
  };

  const ruleSetCode = {
    'Rule Sets': `public class UserValidator : AbstractValidator<User>
{
    public UserValidator()
    {
        // Default rules (always applied)
        RuleFor(x => x.Email).NotEmpty().EmailAddress();
        
        // Create rule set
        RuleSet("Create", () =>
        {
            RuleFor(x => x.Password)
                .NotEmpty()
                .MinimumLength(8);
        });
        
        // Update rule set
        RuleSet("Update", () =>
        {
            RuleFor(x => x.Password)
                .MinimumLength(8)
                .When(x => !string.IsNullOrEmpty(x.Password));
        });
    }
}

// Usage
var createResult = validator.Validate(user, "Create");
var updateResult = validator.Validate(user, "Update");`
  };

  const extensionMethodsCode = {
    'ServiceCollection Extensions': `// Program.cs
services.AddEasyValidate(options =>
{
    options.EnableDetailedErrors = true;
    options.StopOnFirstFailure = false;
    options.DefaultErrorMessage = "Validation failed";
});

// Register specific validators
services.AddScoped<IValidator<User>, UserValidator>();
services.AddValidatorsFromAssembly(typeof(UserValidator).Assembly);`,
    'Validation Extensions': `// Extension methods for common scenarios
public static class ValidationExtensions
{
    public static async Task<ValidationResult> ValidateAndThrowAsync<T>(
        this IValidator<T> validator, 
        T instance)
    {
        var result = await validator.ValidateAsync(instance);
        if (!result.IsValid)
        {
            throw new ValidationException(result.Errors);
        }
        return result;
    }

    public static ValidationResult ValidateProperty<T, TProperty>(
        this IValidator<T> validator,
        T instance,
        Expression<Func<T, TProperty>> propertyExpression)
    {
        // Validate only specific property
        // Implementation details...
    }
}`
  };

  const errorHandlingCode = {
    'ValidationException': `public class ValidationException : Exception
{
    public IEnumerable<ValidationFailure> Errors { get; }
    
    public ValidationException(IEnumerable<ValidationFailure> errors)
        : base($"Validation failed: {string.Join(", ", errors.Select(e => e.ErrorMessage))}")
    {
        Errors = errors;
    }
}

// Usage
try
{
    await validator.ValidateAndThrowAsync(user);
}
catch (ValidationException ex)
{
    foreach (var error in ex.Errors)
    {
        Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
    }
}`,
    'Global Error Handling': `// ASP.NET Core middleware
public class ValidationExceptionMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (ValidationException ex)
        {
            await HandleValidationExceptionAsync(context, ex);
        }
    }

    private async Task HandleValidationExceptionAsync(HttpContext context, ValidationException ex)
    {
        context.Response.StatusCode = 400;
        context.Response.ContentType = "application/json";

        var response = new
        {
            type = "validation_error",
            title = "Validation Failed",
            errors = ex.Errors.GroupBy(e => e.PropertyName)
                              .ToDictionary(g => g.Key, g => g.Select(e => e.ErrorMessage).ToArray())
        };

        await context.Response.WriteAsync(JsonSerializer.Serialize(response));
    }
}`
  };

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1 className={styles.title}>API Reference</h1>
        <p className={styles.subtitle}>
          Complete API documentation for EasyValidate interfaces, classes, and extension methods
        </p>
      </div>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>🏗️ Core Interfaces</h2>
        <p className={styles.text}>
          The fundamental interfaces that form the foundation of EasyValidate:
        </p>
        <CodeWindow 
          code={validatorApiCode}
          title="IValidator<T> and AbstractValidator<T>"
          language="csharp"
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>📊 Validation Results</h2>
        <p className={styles.text}>
          Classes for handling validation results and error information:
        </p>
        <CodeWindow 
          code={validationResultCode}
          title="ValidationResult and ValidationFailure"
          language="csharp"
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>🔧 Rule Builders</h2>
        <p className={styles.text}>
          Fluent API for building validation rules:
        </p>
        <CodeWindow 
          code={ruleBuilderCode}
          title="Rule Builder API"
          language="csharp"
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>🔀 Conditional Rules</h2>
        <p className={styles.text}>
          API for implementing conditional validation logic:
        </p>
        <CodeWindow 
          code={conditionalRulesCode}
          title="Conditional Validation API"
          language="csharp"
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>📚 Rule Sets</h2>
        <p className={styles.text}>
          Organize validation rules into named sets for different scenarios:
        </p>
        <CodeWindow 
          code={ruleSetCode}
          title="Rule Set API"
          language="csharp"
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>🔌 Extension Methods</h2>
        <p className={styles.text}>
          Extension methods for dependency injection and common validation patterns:
        </p>
        <CodeWindow 
          code={extensionMethodsCode}
          title="Extension Methods"
          language="csharp"
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>⚠️ Error Handling</h2>
        <p className={styles.text}>
          Exception handling and error processing utilities:
        </p>
        <CodeWindow 
          code={errorHandlingCode}
          title="Error Handling API"
          language="csharp"
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>📖 Built-in Validators</h2>
        
        <div className={styles.apiTable}>
          <div className={styles.tableHeader}>
            <div className={styles.tableHeaderCell}>Method</div>
            <div className={styles.tableHeaderCell}>Description</div>
            <div className={styles.tableHeaderCell}>Example</div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>NotEmpty()</code></div>
            <div className={styles.tableCell}>Validates that property is not null, empty, or whitespace</div>
            <div className={styles.tableCell}><code>RuleFor(x =&gt; x.Name).NotEmpty()</code></div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>NotNull()</code></div>
            <div className={styles.tableCell}>Validates that property is not null</div>
            <div className={styles.tableCell}><code>RuleFor(x =&gt; x.User).NotNull()</code></div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>Length(min, max)</code></div>
            <div className={styles.tableCell}>Validates string length within range</div>
            <div className={styles.tableCell}><code>RuleFor(x =&gt; x.Name).Length(2, 50)</code></div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>MinimumLength(length)</code></div>
            <div className={styles.tableCell}>Validates minimum string length</div>
            <div className={styles.tableCell}><code>RuleFor(x =&gt; x.Password).MinimumLength(8)</code></div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>MaximumLength(length)</code></div>
            <div className={styles.tableCell}>Validates maximum string length</div>
            <div className={styles.tableCell}><code>RuleFor(x =&gt; x.Title).MaximumLength(100)</code></div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>EmailAddress()</code></div>
            <div className={styles.tableCell}>Validates email address format</div>
            <div className={styles.tableCell}><code>RuleFor(x =&gt; x.Email).EmailAddress()</code></div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>CreditCard()</code></div>
            <div className={styles.tableCell}>Validates credit card number format</div>
            <div className={styles.tableCell}><code>RuleFor(x =&gt; x.CardNumber).CreditCard()</code></div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>GreaterThan(value)</code></div>
            <div className={styles.tableCell}>Validates that value is greater than specified</div>
            <div className={styles.tableCell}><code>RuleFor(x =&gt; x.Age).GreaterThan(0)</code></div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>LessThan(value)</code></div>
            <div className={styles.tableCell}>Validates that value is less than specified</div>
            <div className={styles.tableCell}><code>RuleFor(x =&gt; x.Age).LessThan(150)</code></div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>InclusiveBetween(from, to)</code></div>
            <div className={styles.tableCell}>Validates that value is between range (inclusive)</div>
            <div className={styles.tableCell}><code>RuleFor(x =&gt; x.Score).InclusiveBetween(0, 100)</code></div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>Matches(regex)</code></div>
            <div className={styles.tableCell}>Validates against regular expression pattern</div>
            <div className={styles.tableCell}><code>RuleFor(x =&gt; x.Phone).Matches(@&quot;^\d{'{10}'}$&quot;)</code></div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>Must(predicate)</code></div>
            <div className={styles.tableCell}>Custom validation predicate</div>
            <div className={styles.tableCell}><code>RuleFor(x =&gt; x.Value).Must(BeValid)</code></div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>MustAsync(predicate)</code></div>
            <div className={styles.tableCell}>Async custom validation predicate</div>
            <div className={styles.tableCell}><code>RuleFor(x =&gt; x.Id).MustAsync(ExistsAsync)</code></div>
          </div>
        </div>
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>⚙️ Configuration Options</h2>
        
        <div className={styles.configTable}>
          <div className={styles.tableHeader}>
            <div className={styles.tableHeaderCell}>Property</div>
            <div className={styles.tableHeaderCell}>Type</div>
            <div className={styles.tableHeaderCell}>Description</div>
            <div className={styles.tableHeaderCell}>Default</div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>EnableDetailedErrors</code></div>
            <div className={styles.tableCell}>bool</div>
            <div className={styles.tableCell}>Include detailed error information</div>
            <div className={styles.tableCell}>false</div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>StopOnFirstFailure</code></div>
            <div className={styles.tableCell}>bool</div>
            <div className={styles.tableCell}>Stop validation on first error</div>
            <div className={styles.tableCell}>false</div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>CascadeMode</code></div>
            <div className={styles.tableCell}>CascadeMode</div>
            <div className={styles.tableCell}>How to handle multiple rules for same property</div>
            <div className={styles.tableCell}>Continue</div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>DefaultErrorMessage</code></div>
            <div className={styles.tableCell}>string</div>
            <div className={styles.tableCell}>Default error message when none specified</div>
            <div className={styles.tableCell}>null</div>
          </div>
        </div>
      </section>
    </div>
  );
}

export default ApiReference;
