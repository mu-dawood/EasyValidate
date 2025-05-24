import CodeWindow, { type Window } from '../components/CodeWindow';
import styles from './RealWorldExamples.module.css';

function RealWorldExamples() {
  const userValidationExample: Window[] = [
    {
      fileName: 'User.cs',
      language: 'csharp',
      snipt: `using EasyValidate.Attributes;

public class User
{
    [NotEmpty(ErrorMessage = "Name is required")]
    [MinLength(2)]
    [MaxLength(50)]
    public string Name { get; set; }

    [NotEmpty]
    [EmailAddress]
    public string Email { get; set; }

    [Range(18, 120, ErrorMessage = "Age must be between 18 and 120")]
    public int Age { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    [Url]
    public string? Website { get; set; }
}`,
      active: true
    }
  ];

  const productValidationExample: Window[] = [
    {
      fileName: 'Product.cs',
      language: 'csharp',
      snipt: `using EasyValidate.Attributes;

public class Product
{
    [NotEmpty]
    [MinLength(1)]
    [MaxLength(100)]
    public string Name { get; set; }

    [MaxLength(500)]
    public string? Description { get; set; }

    [Positive]
    public decimal Price { get; set; }

    [NonZero]
    public int StockQuantity { get; set; }

    [AlphaNumeric]
    [MaxLength(20)]
    public string? SKU { get; set; }

    [MinLength(1, ErrorMessage = "At least one category is required")]
    public List<string> Categories { get; set; } = new();
}`,
      active: true
    }
  ];

  const orderValidationExample: Window[] = [
    {
      fileName: 'Order.cs',
      language: 'csharp',
      snipt: `using EasyValidate.Attributes;

public class Order
{
    [NotEmpty]
    [Guid]
    public string OrderId { get; set; }

    [NotInPast]
    public DateTime OrderDate { get; set; }

    [Positive]
    public decimal TotalAmount { get; set; }

    [NotNull]
    public Customer Customer { get; set; }

    [MinLength(1, ErrorMessage = "Order must contain at least one item")]
    public List<OrderItem> Items { get; set; } = new();

    [NotNull]
    public ShippingAddress ShippingAddress { get; set; }
}

public class OrderItem
{
    [NotEmpty]
    public string ProductId { get; set; }

    [Positive]
    public int Quantity { get; set; }

    [Positive]
    public decimal UnitPrice { get; set; }
}

public class Customer
{
    [NotEmpty]
    [MaxLength(100)]
    public string FullName { get; set; }

    [NotEmpty]
    [EmailAddress]
    public string Email { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }
}

public class ShippingAddress
{
    [NotEmpty]
    [MaxLength(200)]
    public string Street { get; set; }

    [NotEmpty]
    [MaxLength(100)]
    public string City { get; set; }

    [NotEmpty]
    [MaxLength(20)]
    public string PostalCode { get; set; }

    [NotEmpty]
    [MaxLength(100)]
    public string Country { get; set; }
}`,
      active: true
    }
  ];

  const apiValidationExample: Window[] = [
    {
      fileName: 'UserController.cs',
      language: 'csharp',
      snipt: `using Microsoft.AspNetCore.Mvc;
using EasyValidate.Attributes;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    [HttpPost]
    public async Task<IActionResult> CreateUser([FromBody] CreateUserRequest request)
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
        {
            return BadRequest(new
            {
                Message = "Validation failed",
                Errors = validationResult.Errors.Select(e => new
                {
                    Field = e.PropertyName,
                    Message = e.ErrorMessage
                })
            });
        }

        // Process the valid user creation
        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            Age = request.Age
        };

        // Save user logic here
        
        return Ok(new { Message = "User created successfully", UserId = user.Id });
    }
}

public class CreateUserRequest
{
    [NotEmpty(ErrorMessage = "Name is required")]
    [MinLength(2, ErrorMessage = "Name must be at least 2 characters")]
    [MaxLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
    public string Name { get; set; }

    [NotEmpty(ErrorMessage = "Email is required")]
    [EmailAddress(ErrorMessage = "Please provide a valid email address")]
    public string Email { get; set; }

    [Range(18, 120, ErrorMessage = "Age must be between 18 and 120")]
    public int Age { get; set; }

    [Phone(ErrorMessage = "Please provide a valid phone number")]
    public string? PhoneNumber { get; set; }
}`,
      active: true
    }
  ];

  const complexValidationExample: Window[] = [
    {
      fileName: 'AdvancedModels.cs',
      language: 'csharp',
      snipt: `using EasyValidate.Attributes;

public class Company
{
    [NotEmpty]
    [MaxLength(200)]
    public string Name { get; set; }

    [NotEmpty]
    [EmailAddress]
    public string ContactEmail { get; set; }

    [Url]
    public string? Website { get; set; }

    [MinLength(1, ErrorMessage = "Company must have at least one department")]
    public List<Department> Departments { get; set; } = new();

    [NotNull]
    public Address HeadquartersAddress { get; set; }

    [Positive]
    public int EmployeeCount { get; set; }

    [PastDate]
    public DateTime FoundedDate { get; set; }
}

public class Department
{
    [NotEmpty]
    [MaxLength(100)]
    public string Name { get; set; }

    [Positive]
    public decimal Budget { get; set; }

    [MinLength(1, ErrorMessage = "Department must have at least one employee")]
    public List<Employee> Employees { get; set; } = new();

    [NotNull]
    public Employee Manager { get; set; }
}

public class Employee
{
    [NotEmpty]
    [MaxLength(100)]
    public string FullName { get; set; }

    [NotEmpty]
    [EmailAddress]
    public string WorkEmail { get; set; }

    [NotEmpty]
    [MaxLength(50)]
    public string Position { get; set; }

    [Positive]
    public decimal Salary { get; set; }

    [PastDate]
    public DateTime DateOfBirth { get; set; }

    [PastDate]
    public DateTime HireDate { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    [MaxLength(10, ErrorMessage = "Maximum 10 skills allowed")]
    public List<string> Skills { get; set; } = new();
}

public class Address
{
    [NotEmpty]
    [MaxLength(200)]
    public string Street { get; set; }

    [NotEmpty]
    [MaxLength(100)]
    public string City { get; set; }

    [MaxLength(50)]
    public string? State { get; set; }

    [NotEmpty]
    [Numeric]
    [MaxLength(20)]
    public string PostalCode { get; set; }

    [NotEmpty]
    [MaxLength(100)]
    public string Country { get; set; }
}`,
      active: true
    }
  ];

  return (
    <div className={styles.container}>
      <header className={styles.header}>
        <div className={styles.headerContent}>
          <span className={styles.badge}>Real-World Examples</span>
          <h1 className={styles.title}>Production-Ready Validation Patterns</h1>
          <p className={styles.subtitle}>
            Discover how EasyValidate handles complex real-world scenarios with elegant, maintainable code.
          </p>
        </div>
      </header>

      <div className={styles.examples}>
        <section className={styles.example}>
          <div className={styles.exampleHeader}>
            <div className={styles.exampleInfo}>
              <h2 className={styles.exampleTitle}>User Registration & Profile</h2>
              <p className={styles.exampleDescription}>
                Essential user validation patterns for registration forms and profile management. 
                Covers name validation, email verification, age constraints, and optional contact fields.
              </p>
              <div className={styles.tags}>
                <span className={styles.tag}>NotEmpty</span>
                <span className={styles.tag}>EmailAddress</span>
                <span className={styles.tag}>MinLength</span>
                <span className={styles.tag}>Range</span>
              </div>
            </div>
            <span className={styles.difficulty}>Basic</span>
          </div>
          <CodeWindow windows={userValidationExample} variant="light" />
        </section>

        <section className={styles.example}>
          <div className={styles.exampleHeader}>
            <div className={styles.exampleInfo}>
              <h2 className={styles.exampleTitle}>E-commerce Product Catalog</h2>
              <p className={styles.exampleDescription}>
                Product catalog validation for e-commerce platforms. Handles pricing constraints, 
                inventory tracking, and category management with robust validation rules.
              </p>
              <div className={styles.tags}>
                <span className={styles.tag}>Positive</span>
                <span className={styles.tag}>AlphaNumeric</span>
                <span className={styles.tag}>Collections</span>
                <span className={styles.tag}>NonZero</span>
              </div>
            </div>
            <span className={styles.difficulty}>Intermediate</span>
          </div>
          <CodeWindow windows={productValidationExample} variant="light" />
        </section>

        <section className={styles.example}>
          <div className={styles.exampleHeader}>
            <div className={styles.exampleInfo}>
              <h2 className={styles.exampleTitle}>Order Processing System</h2>
              <p className={styles.exampleDescription}>
                Complex order validation with nested objects, customer details, and line items. 
                Demonstrates hierarchical validation and business rule enforcement.
              </p>
              <div className={styles.tags}>
                <span className={styles.tag}>Nested Objects</span>
                <span className={styles.tag}>NotInPast</span>
                <span className={styles.tag}>Guid</span>
                <span className={styles.tag}>Phone</span>
              </div>
            </div>
            <span className={styles.difficulty}>Advanced</span>
          </div>
          <CodeWindow windows={orderValidationExample} variant="light" />
        </section>

        <section className={styles.example}>
          <div className={styles.exampleHeader}>
            <div className={styles.exampleInfo}>
              <h2 className={styles.exampleTitle}>Web API Integration</h2>
              <p className={styles.exampleDescription}>
                ASP.NET Core Web API integration showing request validation, error handling, 
                and response formatting for production-ready applications.
              </p>
              <div className={styles.tags}>
                <span className={styles.tag}>ASP.NET Core</span>
                <span className={styles.tag}>Error Handling</span>
                <span className={styles.tag}>DTOs</span>
                <span className={styles.tag}>REST API</span>
              </div>
            </div>
            <span className={styles.difficulty}>Production</span>
          </div>
          <CodeWindow windows={apiValidationExample} variant="light" />
        </section>

        <section className={styles.example}>
          <div className={styles.exampleHeader}>
            <div className={styles.exampleInfo}>
              <h2 className={styles.exampleTitle}>Enterprise Data Models</h2>
              <p className={styles.exampleDescription}>
                Enterprise-grade validation for complex organizational structures. Features deep nesting, 
                advanced date validation, and skill set management.
              </p>
              <div className={styles.tags}>
                <span className={styles.tag}>PastDate</span>
                <span className={styles.tag}>Deep Nesting</span>
                <span className={styles.tag}>Enterprise</span>
                <span className={styles.tag}>Collections</span>
              </div>
            </div>
            <span className={styles.difficulty}>Enterprise</span>
          </div>
          <CodeWindow windows={complexValidationExample} variant="light" />
        </section>
      </div>

      <footer className={styles.footer}>
        <div className={styles.footerContent}>
          <h3 className={styles.footerTitle}>Ready to Build Something Amazing?</h3>
          <p className={styles.footerDescription}>
            These examples are just the beginning. EasyValidate scales from simple forms to enterprise applications.
          </p>
          <div className={styles.footerActions}>
            <a href="/docs/getting-started" className={styles.primaryButton}>
              Get Started
            </a>
            <a href="/docs/attributes" className={styles.secondaryButton}>
              View All Attributes
            </a>
          </div>
        </div>
      </footer>
    </div>
  );
}

export default RealWorldExamples;
