import CodeWindow, { type Window } from '../CodeWindow';
import styles from './Examples.module.css';

function Examples() {
  const basicExample: Window[] = [
    {
      fileName: 'User.cs',
      language: 'csharp',
      snipt: `public partial class User
{
    [Required]
    [StringLength(50, MinimumLength = 2)]
    public string Name { get; set; }

    [Required]
    [Email]
    public string Email { get; set; }

    [Range(18, 120)]
    public int Age { get; set; }
}`,
      active: true
    },
    {
      fileName: 'Usage.cs',
      language: 'csharp',
      snipt: `var user = new User
{
    Name = "John Doe",
    Email = "john@example.com",
    Age = 25
};

var result = user.Validate();

if (result.IsValid)
{
    Console.WriteLine("User is valid!");
}
else
{
    foreach (var error in result.Errors)
    {
        Console.WriteLine($"{error.Property}: {error.Message}");
    }
}`
    }
  ];

  const complexExample: Window[] = [
    {
      fileName: 'Product.cs',
      language: 'csharp',
      snipt: `public partial class Product
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal Price { get; set; }

    [Required]
    [MinLength(1)]
    public List<string> Categories { get; set; }

    [Required]
    [StringLength(500)]
    public string Description { get; set; }

    [Required]
    [Url]
    public string ImageUrl { get; set; }

    [Required]
    public DateTime CreatedAt { get; set; }
}`,
      active: true
    },
    {
      fileName: 'Usage.cs',
      language: 'csharp',
      snipt: `var product = new Product
{
    Name = "Gaming Laptop",
    Price = 1299.99m,
    Categories = new List<string> { "Electronics", "Gaming" },
    Description = "High-performance gaming laptop with RTX graphics.",
    ImageUrl = "https://example.com/laptop.jpg",
    CreatedAt = DateTime.UtcNow
};

var validationResult = product.Validate();

if (!validationResult.IsValid)
{
    var errors = validationResult.Errors
        .GroupBy(e => e.Property)
        .ToDictionary(g => g.Key, g => g.Select(e => e.Message).ToList());
    
    foreach (var (property, messages) in errors)
    {
        Console.WriteLine($"{property}:");
        foreach (var message in messages)
        {
            Console.WriteLine($"  - {message}");
        }
    }
}`
    }
  ];

  const customValidationExample: Window[] = [
    {
      fileName: 'CustomAttributes.cs',
      language: 'csharp',
      snipt: `[AttributeUsage(AttributeTargets.Property)]
public class PasswordStrengthAttribute : ValidationAttributeBase
{
    public override string GetErrorMessage(string propertyName, object value)
    {
        return $"{propertyName} must contain at least 8 characters with uppercase, lowercase, and numbers.";
    }

    public override bool IsValid(object value)
    {
        if (value is not string password) return false;
        
        return password.Length >= 8 &&
               password.Any(char.IsUpper) &&
               password.Any(char.IsLower) &&
               password.Any(char.IsDigit);
    }
}`,
      active: true
    },
    {
      fileName: 'UserAccount.cs',
      language: 'csharp',
      snipt: `public partial class UserAccount
{
    [Required]
    [StringLength(50)]
    public string Username { get; set; }

    [Required]
    [Email]
    public string Email { get; set; }

    [Required]
    [PasswordStrength]
    public string Password { get; set; }

    [Required]
    [Compare(nameof(Password))]
    public string ConfirmPassword { get; set; }
}`
    }
  ];

  return (
    <section id="examples" className={styles.examples}>
      <div className={styles.container}>
        <div className={styles.header}>
          <h2 className={styles.title}>Real-World Examples</h2>
          <p className={styles.subtitle}>
            See how EasyValidate handles various validation scenarios from simple to complex.
          </p>
        </div>

        <div className={styles.exampleGrid}>
          <div className={styles.example}>
            <div className={styles.exampleHeader}>
              <h3 className={styles.exampleTitle}>Basic User Validation</h3>
              <p className={styles.exampleDescription}>
                Simple validation with built-in attributes for common scenarios.
              </p>
            </div>
            <div className={styles.codeBlock}>
              <CodeWindow windows={basicExample} variant="light" />
            </div>
          </div>

          <div className={styles.example}>
            <div className={styles.exampleHeader}>
              <h3 className={styles.exampleTitle}>Complex Product Model</h3>
              <p className={styles.exampleDescription}>
                Advanced validation with collections, ranges, and URLs.
              </p>
            </div>
            <div className={styles.codeBlock}>
              <CodeWindow windows={complexExample} variant="light" />
            </div>
          </div>

          <div className={styles.example}>
            <div className={styles.exampleHeader}>
              <h3 className={styles.exampleTitle}>Custom Validation Attributes</h3>
              <p className={styles.exampleDescription}>
                Create your own validation logic with custom attributes.
              </p>
            </div>
            <div className={styles.codeBlock}>
              <CodeWindow windows={customValidationExample} variant="light" />
            </div>
          </div>
        </div>

        <div className={styles.callToAction}>
          <h3 className={styles.ctaTitle}>Ready to get started?</h3>
          <p className={styles.ctaDescription}>
            Install EasyValidate and start validating your models in minutes.
          </p>
          <a href="#getting-started" className={styles.ctaButton}>
            View Installation Guide
          </a>
        </div>
      </div>
    </section>
  );
}

export default Examples;
