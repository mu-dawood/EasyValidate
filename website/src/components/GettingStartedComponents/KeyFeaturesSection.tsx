import { useState } from 'react';
import InlineSnippet from '../InlineSnippet/InlineSnippet';
import styles from './GettingStartedComponents.module.css';

interface Feature {
  id: string;
  icon: string;
  title: string;
  description: string;
  benefits: string[];
  codeExample?: {
    title: string;
    language: string;
    code: string;
  };
}

function KeyFeaturesSection() {
  const [selectedFeature, setSelectedFeature] = useState<string | null>(null);

  const features: Feature[] = [
    {
      id: 'attribute-based',
      icon: '🎯',
      title: 'Attribute-Based Validation',
      description: 'Use simple, declarative attributes to define validation rules directly on your model properties.',
      benefits: [
        'Clean, readable code',
        'No external configuration files',
        'IntelliSense support',
        'Compile-time safety'
      ],
      codeExample: {
        title: 'Attribute Examples',
        language: 'csharp',
        code: `public partial class Product
{
    [Required]
    [StringLength(100, MinimumLength = 3)]
    public string Name { get; set; } = string.Empty;

    [Range(0.01, 10000.00)]
    [Display(Name = "Price ($)")]
    public decimal Price { get; set; }

    [EmailAddress]
    [Display(Name = "Contact Email")]
    public string? ContactEmail { get; set; }

    [Url]
    public string? Website { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }
}`
      }
    },
    {
      id: 'high-performance',
      icon: '⚡',
      title: 'High Performance',
      description: 'Source generators create optimized validation code at compile-time, eliminating reflection overhead.',
      benefits: [
        'Zero reflection at runtime',
        'Minimal memory allocation',
        'Lightning-fast validation',
        'AOT compilation ready'
      ],
      codeExample: {
        title: 'Generated Validation Code',
        language: 'csharp',
        code: `// Generated at compile-time by EasyValidate
public partial class User
{
    public ValidationResult Validate()
    {
        var errors = new List<ValidationError>();
        
        // Name validation (optimized, no reflection)
        if (string.IsNullOrEmpty(this.Name))
        {
            errors.Add(new ValidationError(
                nameof(Name), 
                "Name is required", 
                this.Name
            ));
        }
        else if (this.Name.Length > 50)
        {
            errors.Add(new ValidationError(
                nameof(Name), 
                "Name must be less than 50 characters", 
                this.Name
            ));
        }
        
        // Email validation
        if (!string.IsNullOrEmpty(this.Email) && 
            !EmailAddressValidator.IsValid(this.Email))
        {
            errors.Add(new ValidationError(
                nameof(Email), 
                "Please enter a valid email address", 
                this.Email
            ));
        }
        
        return new ValidationResult(errors);
    }
}`
      }
    },
    {
      id: 'extensible',
      icon: '🔧',
      title: 'Highly Extensible',
      description: 'Create custom validation attributes and rules to fit your specific business requirements.',
      benefits: [
        'Custom attribute support',
        'Pluggable validators',
        'Business rule integration',
        'Complex validation scenarios'
      ],
      codeExample: {
        title: 'Custom Validation Attribute',
        language: 'csharp',
        code: `// Custom validation attribute
public class CreditCardAttribute : StringValidationAttributeBase
{
    public override bool IsValid(string? value)
    {
        if (string.IsNullOrEmpty(value)) 
            return true; // Let Required handle null checks
            
        // Remove spaces and dashes
        var cleanValue = value.Replace(" ", "").Replace("-", "");
        
        // Check if all digits
        if (!cleanValue.All(char.IsDigit))
            return false;
            
        // Luhn algorithm validation
        return IsValidLuhn(cleanValue);
    }
    
    private bool IsValidLuhn(string cardNumber)
    {
        int sum = 0;
        bool alternate = false;
        
        for (int i = cardNumber.Length - 1; i >= 0; i--)
        {
            int digit = int.Parse(cardNumber[i].ToString());
            
            if (alternate)
            {
                digit *= 2;
                if (digit > 9) digit -= 9;
            }
            
            sum += digit;
            alternate = !alternate;
        }
        
        return sum % 10 == 0;
    }
}

// Usage
public partial class PaymentInfo
{
    [Required]
    [CreditCard(ErrorMessage = "Please enter a valid credit card number")]
    public string CardNumber { get; set; } = string.Empty;
}`
      }
    },
    {
      id: 'framework-agnostic',
      icon: '🌐',
      title: 'Framework Agnostic',
      description: 'Works seamlessly across all .NET platforms and frameworks without dependencies.',
      benefits: [
        'ASP.NET Core integration',
        'Blazor support',
        'WPF and WinForms ready',
        '.NET MAUI compatible'
      ],
      codeExample: {
        title: 'Cross-Platform Usage',
        language: 'csharp',
        code: `// Shared model across all platforms
public partial class UserRegistration
{
    [Required, StringLength(50)]
    public string Username { get; set; } = string.Empty;
    
    [Required, EmailAddress]
    public string Email { get; set; } = string.Empty;
    
    [Required, MinLength(8)]
    [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]",
        ErrorMessage = "Password must contain uppercase, lowercase, digit and special character")]
    public string Password { get; set; } = string.Empty;
}

// Use in ASP.NET Core Controller
[HttpPost]
public IActionResult Register([FromBody] UserRegistration registration)
{
    var result = registration.Validate();
    if (!result.IsValid) return BadRequest(result.Errors);
    // Process registration...
}

// Use in Blazor Component
private async Task HandleSubmit()
{
    var result = userRegistration.Validate();
    if (result.IsValid)
    {
        await UserService.RegisterAsync(userRegistration);
    }
}

// Use in WPF ViewModel
private void RegisterUser()
{
    if (UserRegistration.Validate().IsValid)
    {
        // Process registration...
    }
}`
      }
    },
    {
      id: 'type-safe',
      icon: '🛡️',
      title: 'Type Safe & IntelliSense',
      description: 'Full compile-time checking with rich IntelliSense support and error detection.',
      benefits: [
        'Compile-time validation',
        'IDE error highlighting',
        'Auto-completion support',
        'Refactoring safety'
      ],
      codeExample: {
        title: 'Type Safety Features',
        language: 'csharp',
        code: `public partial class Order
{
    [Required]
    public int CustomerId { get; set; }
    
    [Range(typeof(DateTime), "2024-01-01", "2030-12-31")]
    public DateTime OrderDate { get; set; }
    
    [MinLength(1, ErrorMessage = "Order must contain at least one item")]
    public List<OrderItem> Items { get; set; } = new();
    
    // Compile-time safe property access
    public bool ValidateCustomer()
    {
        var result = this.Validate();
        
        // IntelliSense knows the exact property names
        var customerErrors = result.Errors
            .Where(e => e.Property == nameof(CustomerId))
            .ToList();
            
        return customerErrors.Count == 0;
    }
}

// Analyzer catches common mistakes at compile-time:
public partial class InvalidExample
{
    // ❌ Analyzer error: Range attribute cannot be used on string
    // [Range(1, 100)]  
    // public string Name { get; set; }
    
    // ❌ Analyzer error: EmailAddress attribute should not be used with Required
    // [Required, EmailAddress] // Use just [EmailAddress] instead
    // public string? Email { get; set; }
    
    // ✅ Correct usage
    [Required]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;
    
    [EmailAddress] // Optional email
    public string? Email { get; set; }
}`
      }
    },
    {
      id: 'rich-diagnostics',
      icon: '🔍',
      title: 'Rich Error Diagnostics',
      description: 'Detailed validation results with property-level error information and context.',
      benefits: [
        'Detailed error messages',
        'Property-level errors',
        'Attempted value tracking',
        'Custom error formatting'
      ],
      codeExample: {
        title: 'Comprehensive Error Information',
        language: 'csharp',
        code: `public static class ValidationHelper
{
    public static void DisplayDetailedErrors<T>(T model)
    {
        var result = ((dynamic)model).Validate();
        
        Console.WriteLine($"Validation Summary for {typeof(T).Name}:");
        Console.WriteLine($"Status: {(result.IsValid ? "✅ Valid" : "❌ Invalid")}");
        Console.WriteLine($"Total Errors: {result.Errors.Count}");
        Console.WriteLine();
        
        if (!result.IsValid)
        {
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Property: {error.Property}");
                Console.WriteLine($"Message: {error.Message}");
                Console.WriteLine($"Attempted Value: {error.AttemptedValue ?? "null"}");
                Console.WriteLine($"Error Code: {error.ErrorCode}");
                Console.WriteLine("---");
            }
            
            // Group errors by severity
            var criticalErrors = result.Errors.Where(e => e.Severity == ErrorSeverity.Critical);
            var warningErrors = result.Errors.Where(e => e.Severity == ErrorSeverity.Warning);
            
            if (criticalErrors.Any())
            {
                Console.WriteLine($"Critical Issues: {criticalErrors.Count()}");
            }
            
            if (warningErrors.Any())
            {
                Console.WriteLine($"Warnings: {warningErrors.Count()}");
            }
        }
    }
    
    public static Dictionary<string, object> ToErrorResponse(ValidationResult result)
    {
        return new Dictionary<string, object>
        {
            ["isValid"] = result.IsValid,
            ["errorCount"] = result.Errors.Count,
            ["errors"] = result.Errors.GroupBy(e => e.Property)
                .ToDictionary(g => g.Key, g => g.Select(e => new {
                    message = e.Message,
                    attemptedValue = e.AttemptedValue,
                    errorCode = e.ErrorCode
                }).ToArray())
        };
    }
}`
      }
    }
  ];

  const handleFeatureClick = (featureId: string) => {
    setSelectedFeature(selectedFeature === featureId ? null : featureId);
  };

  return (
    <section className={styles.section}>
      <div className={styles.sectionHeader}>
        <h2 className={styles.sectionTitle}>
          <span className={styles.sectionIcon}>✨</span>
          Key Features
        </h2>
        <p className={styles.sectionDescription}>
          Discover what makes EasyValidate the perfect choice for .NET validation
        </p>
      </div>

      <div className={styles.featuresContainer}>
        <div className={styles.featuresGrid}>
          {features.map((feature) => (
            <div
              key={feature.id}
              className={`${styles.featureCard} ${selectedFeature === feature.id ? styles.active : ''}`}
              onClick={() => handleFeatureClick(feature.id)}
            >
              <div className={styles.featureHeader}>
                <div className={styles.featureIcon}>{feature.icon}</div>
                <h3 className={styles.featureTitle}>{feature.title}</h3>
              </div>
              
              <p className={styles.featureDescription}>{feature.description}</p>
              
              <div className={styles.featureBenefits}>
                {feature.benefits.map((benefit, index) => (
                  <div key={index} className={styles.benefitItem}>
                    <span className={styles.benefitIcon}>✓</span>
                    <span className={styles.benefitText}>{benefit}</span>
                  </div>
                ))}
              </div>

              <div className={styles.featureAction}>
                <span className={styles.actionText}>
                  {selectedFeature === feature.id ? 'Hide' : 'View'} Example
                </span>
                <span className={styles.actionIcon}>
                  {selectedFeature === feature.id ? '▲' : '▼'}
                </span>
              </div>
            </div>
          ))}
        </div>

        {/* Expanded Feature Code Example */}
        {selectedFeature && (
          <div className={styles.featureCodeExample}>
            {(() => {
              const feature = features.find(f => f.id === selectedFeature);
              return feature?.codeExample ? (
                <div className={styles.codeExampleContainer}>
                  <div className={styles.codeExampleHeader}>
                    <h4 className={styles.codeExampleTitle}>
                      {feature.icon} {feature.codeExample.title}
                    </h4>
                    <button 
                      className={styles.copyButton}
                      onClick={() => navigator.clipboard.writeText(feature.codeExample!.code)}
                      title="Copy code"
                    >
                      📋 Copy
                    </button>
                  </div>
                  
                  <div className={styles.codeContent}>
                    <InlineSnippet 
                      snipt={feature.codeExample.code}
                      language={feature.codeExample.language}
                      className={styles.featureCode}
                    />
                  </div>
                </div>
              ) : null;
            })()}
          </div>
        )}
      </div>

      {/* Summary Stats */}
      <div className={styles.featureStats}>
        <div className={styles.stat}>
          <div className={styles.statNumber}>67+</div>
          <div className={styles.statLabel}>Built-in Attributes</div>
        </div>
        <div className={styles.stat}>
          <div className={styles.statNumber}>5</div>
          <div className={styles.statLabel}>Framework Integrations</div>
        </div>
        <div className={styles.stat}>
          <div className={styles.statNumber}>0ms</div>
          <div className={styles.statLabel}>Runtime Reflection</div>
        </div>
        <div className={styles.stat}>
          <div className={styles.statNumber}>100%</div>
          <div className={styles.statLabel}>Type Safe</div>
        </div>
      </div>
    </section>
  );
}

export default KeyFeaturesSection;
