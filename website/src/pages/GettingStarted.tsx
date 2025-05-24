import CodeWindow from '../components/CodeWindow';
import styles from './GettingStarted.module.css';

function GettingStarted() {
  const installationWindows = [
    {
      fileName: 'Package Manager',
      language: 'bash',
      snipt: `Install-Package EasyValidate`
    },
    {
      fileName: 'NuGet CLI',
      language: 'bash', 
      snipt: `nuget install EasyValidate`
    },
    {
      fileName: '.NET CLI',
      language: 'bash',
      snipt: `dotnet add package EasyValidate`
    }
  ];

  const basicUsageWindows = [
    {
      fileName: 'User.cs',
      language: 'csharp',
      snipt: `using EasyValidate.Attributes;

public class User
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, ErrorMessage = "Name must be less than 50 characters")]
    public string Name { get; set; }

    [Required]
    [EmailAddress(ErrorMessage = "Please enter a valid email address")]
    public string Email { get; set; }

    [Range(18, 100, ErrorMessage = "Age must be between 18 and 100")]
    public int Age { get; set; }
}`
    },
    {
      fileName: 'Program.cs',
      language: 'csharp',
      snipt: `using EasyValidate;

var user = new User
{
    Name = "John Doe",
    Email = "john@example.com",
    Age = 25
};

var validator = new EasyValidator<User>();
var result = validator.Validate(user);

if (result.IsValid)
{
    Console.WriteLine("User is valid!");
}
else
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"{error.PropertyName}: {error.ErrorMessage}");
    }
}`
    }
  ];

  const quickStartWindows = [
    {
      fileName: 'Startup.cs',
      language: 'csharp',
      snipt: `// Startup.cs or Program.cs
services.AddEasyValidate();

// Controller
[ApiController]
public class UserController : ControllerBase
{
    private readonly IEasyValidator<User> _validator;

    public UserController(IEasyValidator<User> validator)
    {
        _validator = validator;
    }

    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        var result = _validator.Validate(user);
        
        if (!result.IsValid)
        {
            return BadRequest(result.Errors);
        }

        // Process valid user
        return Ok();
    }
}`
    },
    {
      fileName: 'Program.cs',
      language: 'csharp',
      snipt: `using EasyValidate;
using EasyValidate.Attributes;

var user = new User();
var validator = new EasyValidator<User>();

// Validate and handle results
var result = validator.Validate(user);
Console.WriteLine($"Is Valid: {result.IsValid}");

public class User
{
    [Required]
    public string Name { get; set; }
    
    [EmailAddress]
    public string Email { get; set; }
}`
    }
  ];

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1 className={styles.title}>Getting Started</h1>
        <p className={styles.subtitle}>
          Learn how to install and use EasyValidate in your .NET applications
        </p>
      </div>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>📦 Installation</h2>
        <p className={styles.text}>
          EasyValidate is available as a NuGet package. Choose your preferred installation method:
        </p>
        <CodeWindow 
          windows={installationWindows}
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>🏃‍♂️ Quick Start</h2>
        <p className={styles.text}>
          Here's a simple example to get you started with EasyValidate:
        </p>
        
        <div className={styles.stepContainer}>
          <div className={styles.step}>
            <h3 className={styles.stepTitle}>Step 1: Define Your Model</h3>
            <p className={styles.stepText}>
              Add validation attributes to your model properties:
            </p>
          </div>
        </div>
        
        <CodeWindow 
          windows={basicUsageWindows}
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>🚀 Framework Integration</h2>
        <p className={styles.text}>
          EasyValidate works seamlessly with popular .NET frameworks:
        </p>
        <CodeWindow 
          windows={quickStartWindows}
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>✨ Key Features</h2>
        <div className={styles.featuresGrid}>
          <div className={styles.featureCard}>
            <div className={styles.featureIcon}>🎯</div>
            <h3 className={styles.featureTitle}>Attribute-Based</h3>
            <p className={styles.featureText}>
              Use simple attributes to define validation rules directly on your model properties.
            </p>
          </div>
          
          <div className={styles.featureCard}>
            <div className={styles.featureIcon}>⚡</div>
            <h3 className={styles.featureTitle}>High Performance</h3>
            <p className={styles.featureText}>
              Optimized validation engine with minimal overhead and fast execution.
            </p>
          </div>
          
          <div className={styles.featureCard}>
            <div className={styles.featureIcon}>🔧</div>
            <h3 className={styles.featureTitle}>Extensible</h3>
            <p className={styles.featureText}>
              Create custom validation attributes and rules to fit your specific needs.
            </p>
          </div>
          
          <div className={styles.featureCard}>
            <div className={styles.featureIcon}>🌐</div>
            <h3 className={styles.featureTitle}>Framework Agnostic</h3>
            <p className={styles.featureText}>
              Works with ASP.NET Core, WPF, WinForms, Blazor, and more.
            </p>
          </div>
        </div>
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>📚 Next Steps</h2>
        <div className={styles.nextStepsGrid}>
          <a href="/docs/examples" className={styles.nextStepCard}>
            <div className={styles.nextStepIcon}>💡</div>
            <h3 className={styles.nextStepTitle}>Real-World Examples</h3>
            <p className={styles.nextStepText}>
              Explore practical validation scenarios and implementation patterns.
            </p>
          </a>
          
          <a href="/docs/attributes" className={styles.nextStepCard}>
            <div className={styles.nextStepIcon}>🏷️</div>
            <h3 className={styles.nextStepTitle}>Validation Attributes</h3>
            <p className={styles.nextStepText}>
              Learn about all available validation attributes and their options.
            </p>
          </a>
          
          <a href="/docs/advanced" className={styles.nextStepCard}>
            <div className={styles.nextStepIcon}>⚙️</div>
            <h3 className={styles.nextStepTitle}>Advanced Usage</h3>
            <p className={styles.nextStepText}>
              Discover advanced features like custom validators and conditional validation.
            </p>
          </a>
        </div>
      </section>
    </div>
  );
}

export default GettingStarted;
