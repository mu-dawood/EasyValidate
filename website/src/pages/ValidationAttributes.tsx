import CodeWindow from '../components/CodeWindow';
import styles from './ValidationAttributes.module.css';

function ValidationAttributes() {
  const basicAttributesWindows = [
    {
      fileName: 'Required.cs',
      language: 'csharp',
      snipt: `[Required(ErrorMessage = "This field is required")]
public string Name { get; set; }

[Required(AllowEmptyStrings = false)]
public string Description { get; set; }`
    },
    {
      fileName: 'StringLength.cs',
      language: 'csharp',
      snipt: `[StringLength(50, ErrorMessage = "Maximum 50 characters allowed")]
public string Title { get; set; }

[StringLength(100, MinimumLength = 10, 
    ErrorMessage = "Must be between 10 and 100 characters")]
public string Content { get; set; }`
    },
    {
      fileName: 'Range.cs',
      language: 'csharp',
      snipt: `[Range(1, 100, ErrorMessage = "Value must be between 1 and 100")]
public int Percentage { get; set; }

[Range(typeof(decimal), "0.01", "999999.99")]
public decimal Price { get; set; }`
    }
  ];

  const formatAttributesWindows = [
    {
      fileName: 'EmailAddress.cs',
      language: 'csharp',
      snipt: `[EmailAddress(ErrorMessage = "Please enter a valid email address")]
public string Email { get; set; }

// Custom email validation
[EmailAddress]
[RegularExpression(@"^[a-zA-Z0-9._%+-]+@company\\.com$", 
    ErrorMessage = "Must be a company email address")]
public string WorkEmail { get; set; }`
    },
    {
      fileName: 'Phone-URL.cs',
      language: 'csharp',
      snipt: `[Phone(ErrorMessage = "Please enter a valid phone number")]
public string PhoneNumber { get; set; }

[Url(ErrorMessage = "Please enter a valid URL")]
public string Website { get; set; }`
    },
    {
      fileName: 'CreditCard.cs',
      language: 'csharp',
      snipt: `[CreditCard(ErrorMessage = "Please enter a valid credit card number")]
public string CardNumber { get; set; }`
    }
  ];

  const comparisonAttributesWindows = [
    {
      fileName: 'Compare.cs',
      language: 'csharp',
      snipt: `public class RegisterViewModel
{
    [Required]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Required]
    [Compare("Password", ErrorMessage = "Passwords do not match")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }
}`
    },
    {
      fileName: 'MinMaxLength.cs',
      language: 'csharp',
      snipt: `[MinLength(8, ErrorMessage = "Password must be at least 8 characters")]
[MaxLength(128, ErrorMessage = "Password cannot exceed 128 characters")]
public string Password { get; set; }

[MinLength(1)]
[MaxLength(5)]
public List<string> Tags { get; set; }`
    }
  ];

  const customAttributesWindows = [
    {
      fileName: 'CustomAttribute.cs',
      language: 'csharp',
      snipt: `public class ValidDateRangeAttribute : ValidationAttribute
{
    private readonly string _startDateProperty;
    private readonly string _endDateProperty;

    public ValidDateRangeAttribute(string startDateProperty, string endDateProperty)
    {
        _startDateProperty = startDateProperty;
        _endDateProperty = endDateProperty;
    }

    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    {
        var startDateProperty = validationContext.ObjectType.GetProperty(_startDateProperty);
        var endDateProperty = validationContext.ObjectType.GetProperty(_endDateProperty);

        var startDate = (DateTime?)startDateProperty?.GetValue(validationContext.ObjectInstance);
        var endDate = (DateTime?)endDateProperty?.GetValue(validationContext.ObjectInstance);

        if (startDate.HasValue && endDate.HasValue && startDate > endDate)
        {
            return new ValidationResult("Start date must be before end date");
        }

        return ValidationResult.Success;
    }
}`
    },
    {
      fileName: 'Usage.cs',
      language: 'csharp',
      snipt: `public class Event
{
    [Required]
    public DateTime StartDate { get; set; }

    [Required]
    public DateTime EndDate { get; set; }

    [ValidDateRange("StartDate", "EndDate")]
    public bool IsValidDateRange => true;
}`
    }
  ];

  const conditionalValidationWindows = [
    {
      fileName: 'RequiredIf.cs',
      language: 'csharp',
      snipt: `public class Order
{
    public bool IsGift { get; set; }

    [RequiredIf("IsGift", true, ErrorMessage = "Gift message is required for gifts")]
    public string GiftMessage { get; set; }

    public ShippingMethod ShippingMethod { get; set; }

    [RequiredIf("ShippingMethod", ShippingMethod.Express)]
    public DateTime? ExpressDeliveryDate { get; set; }
}`
    },
    {
      fileName: 'ValidateIf.cs',
      language: 'csharp',
      snipt: `public class Employee
{
    public bool IsManager { get; set; }

    [ValidateIf("IsManager", true)]
    [Range(50000, 200000, ErrorMessage = "Manager salary must be between 50k-200k")]
    public decimal? Salary { get; set; }

    [ValidateIf("IsManager", true)]
    [Required(ErrorMessage = "Department is required for managers")]
    public string Department { get; set; }
}`
    }
  ];

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1 className={styles.title}>Validation Attributes</h1>
        <p className={styles.subtitle}>
          Complete reference guide for all EasyValidate attributes and their configuration options
        </p>
      </div>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>🔨 Basic Validation Attributes</h2>
        <p className={styles.text}>
          These are the fundamental validation attributes you'll use most frequently:
        </p>
        
        <div className={styles.attributeGrid}>
          <div className={styles.attributeCard}>
            <h3 className={styles.attributeName}>Required</h3>
            <p className={styles.attributeDescription}>
              Ensures that a property has a non-null, non-empty value.
            </p>
          </div>
          
          <div className={styles.attributeCard}>
            <h3 className={styles.attributeName}>StringLength</h3>
            <p className={styles.attributeDescription}>
              Validates the length of string properties with minimum and maximum constraints.
            </p>
          </div>
          
          <div className={styles.attributeCard}>
            <h3 className={styles.attributeName}>Range</h3>
            <p className={styles.attributeDescription}>
              Validates that a numeric value falls within a specified range.
            </p>
          </div>
        </div>

        <CodeWindow 
          windows={basicAttributesWindows}
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>📧 Format Validation Attributes</h2>
        <p className={styles.text}>
          Specialized attributes for validating common data formats:
        </p>
        <CodeWindow 
          windows={formatAttributesWindows}
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>🔗 Comparison Attributes</h2>
        <p className={styles.text}>
          Attributes for comparing values between properties or against fixed values:
        </p>
        <CodeWindow 
          windows={comparisonAttributesWindows}
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>🎨 Custom Validation Attributes</h2>
        <p className={styles.text}>
          Create your own validation logic by inheriting from ValidationAttribute:
        </p>
        <CodeWindow 
          windows={customAttributesWindows}
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>🔀 Conditional Validation</h2>
        <p className={styles.text}>
          Apply validation rules conditionally based on other property values:
        </p>
        <CodeWindow 
          windows={conditionalValidationWindows}
        />
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>📋 Complete Attribute Reference</h2>
        
        <div className={styles.referenceTable}>
          <div className={styles.tableHeader}>
            <div className={styles.tableHeaderCell}>Attribute</div>
            <div className={styles.tableHeaderCell}>Purpose</div>
            <div className={styles.tableHeaderCell}>Parameters</div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>Required</code></div>
            <div className={styles.tableCell}>Ensures property is not null/empty</div>
            <div className={styles.tableCell}>AllowEmptyStrings, ErrorMessage</div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>StringLength</code></div>
            <div className={styles.tableCell}>Validates string length</div>
            <div className={styles.tableCell}>MaximumLength, MinimumLength</div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>MinLength</code></div>
            <div className={styles.tableCell}>Validates minimum length</div>
            <div className={styles.tableCell}>Length</div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>MaxLength</code></div>
            <div className={styles.tableCell}>Validates maximum length</div>
            <div className={styles.tableCell}>Length</div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>Range</code></div>
            <div className={styles.tableCell}>Validates numeric range</div>
            <div className={styles.tableCell}>Minimum, Maximum, Type</div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>EmailAddress</code></div>
            <div className={styles.tableCell}>Validates email format</div>
            <div className={styles.tableCell}>ErrorMessage</div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>Phone</code></div>
            <div className={styles.tableCell}>Validates phone number format</div>
            <div className={styles.tableCell}>ErrorMessage</div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>Url</code></div>
            <div className={styles.tableCell}>Validates URL format</div>
            <div className={styles.tableCell}>ErrorMessage</div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>CreditCard</code></div>
            <div className={styles.tableCell}>Validates credit card numbers</div>
            <div className={styles.tableCell}>ErrorMessage</div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>Compare</code></div>
            <div className={styles.tableCell}>Compares with another property</div>
            <div className={styles.tableCell}>OtherProperty</div>
          </div>
          
          <div className={styles.tableRow}>
            <div className={styles.tableCell}><code>RegularExpression</code></div>
            <div className={styles.tableCell}>Validates against regex pattern</div>
            <div className={styles.tableCell}>Pattern</div>
          </div>
        </div>
      </section>

      <section className={styles.section}>
        <h2 className={styles.sectionTitle}>💡 Best Practices</h2>
        <div className={styles.tipsGrid}>
          <div className={styles.tipCard}>
            <div className={styles.tipIcon}>✅</div>
            <h3 className={styles.tipTitle}>Clear Error Messages</h3>
            <p className={styles.tipText}>
              Always provide user-friendly error messages that explain what went wrong and how to fix it.
            </p>
          </div>
          
          <div className={styles.tipCard}>
            <div className={styles.tipIcon}>🎯</div>
            <h3 className={styles.tipTitle}>Specific Validation</h3>
            <p className={styles.tipText}>
              Use the most specific validation attribute for your use case rather than generic regex patterns.
            </p>
          </div>
          
          <div className={styles.tipCard}>
            <div className={styles.tipIcon}>🔄</div>
            <h3 className={styles.tipTitle}>Combine Attributes</h3>
            <p className={styles.tipText}>
              Multiple validation attributes can be applied to a single property for comprehensive validation.
            </p>
          </div>
          
          <div className={styles.tipCard}>
            <div className={styles.tipIcon}>⚡</div>
            <h3 className={styles.tipTitle}>Performance</h3>
            <p className={styles.tipText}>
              Order attributes from fastest to slowest validation for better performance on invalid data.
            </p>
          </div>
        </div>
      </section>
    </div>
  );
}

export default ValidationAttributes;
