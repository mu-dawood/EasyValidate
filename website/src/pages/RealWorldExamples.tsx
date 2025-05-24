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
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinLength = 2)]
    public string Name { get; set; }

    [Required]
    [Email]
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
    [Required]
    [StringLength(100, MinLength = 1)]
    public string Name { get; set; }

    [StringLength(500)]
    public string? Description { get; set; }

    [Required]
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0")]
    public decimal Price { get; set; }

    [Required]
    [Range(0, int.MaxValue)]
    public int StockQuantity { get; set; }

    [StringLength(20)]
    public string? SKU { get; set; }

    [CollectionNotEmpty]
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
    [Required]
    public string OrderId { get; set; }

    [Required]
    public DateTime OrderDate { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal TotalAmount { get; set; }

    [Required]
    public Customer Customer { get; set; }

    [CollectionNotEmpty]
    public List<OrderItem> Items { get; set; } = new();

    [Required]
    public ShippingAddress ShippingAddress { get; set; }
}

public class OrderItem
{
    [Required]
    public string ProductId { get; set; }

    [Required]
    [Range(1, int.MaxValue)]
    public int Quantity { get; set; }

    [Required]
    [Range(0.01, double.MaxValue)]
    public decimal UnitPrice { get; set; }
}

public class Customer
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; }

    [Required]
    [Email]
    public string Email { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }
}

public class ShippingAddress
{
    [Required]
    [StringLength(200)]
    public string Street { get; set; }

    [Required]
    [StringLength(100)]
    public string City { get; set; }

    [Required]
    [StringLength(20)]
    public string PostalCode { get; set; }

    [Required]
    [StringLength(100)]
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

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateUser(int id, [FromBody] UpdateUserRequest request)
    {
        var validationResult = request.Validate();
        if (!validationResult.IsValid)
        {
            return BadRequest(validationResult.Errors);
        }

        // Update user logic here
        
        return Ok();
    }
}

public class CreateUserRequest
{
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50, MinLength = 2, ErrorMessage = "Name must be between 2 and 50 characters")]
    public string Name { get; set; }

    [Required(ErrorMessage = "Email is required")]
    [Email(ErrorMessage = "Please provide a valid email address")]
    public string Email { get; set; }

    [Required]
    [Range(18, 120, ErrorMessage = "Age must be between 18 and 120")]
    public int Age { get; set; }

    [Phone(ErrorMessage = "Please provide a valid phone number")]
    public string? PhoneNumber { get; set; }
}

public class UpdateUserRequest
{
    [StringLength(50, MinLength = 2)]
    public string? Name { get; set; }

    [Email]
    public string? Email { get; set; }

    [Range(18, 120)]
    public int? Age { get; set; }

    [Phone]
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
    [Required]
    [StringLength(200)]
    public string Name { get; set; }

    [Required]
    [Email]
    public string ContactEmail { get; set; }

    [Url]
    public string? Website { get; set; }

    [CollectionNotEmpty]
    public List<Department> Departments { get; set; } = new();

    [Required]
    public Address HeadquartersAddress { get; set; }

    [Range(1, int.MaxValue)]
    public int EmployeeCount { get; set; }

    [DateRange("1800-01-01", "2100-12-31")]
    public DateTime FoundedDate { get; set; }
}

public class Department
{
    [Required]
    [StringLength(100)]
    public string Name { get; set; }

    [Required]
    [Range(1, double.MaxValue)]
    public decimal Budget { get; set; }

    [CollectionNotEmpty]
    public List<Employee> Employees { get; set; } = new();

    [Required]
    public Employee Manager { get; set; }
}

public class Employee
{
    [Required]
    [StringLength(100)]
    public string FullName { get; set; }

    [Required]
    [Email]
    public string WorkEmail { get; set; }

    [Required]
    [StringLength(50)]
    public string Position { get; set; }

    [Required]
    [Range(0, double.MaxValue)]
    public decimal Salary { get; set; }

    [Required]
    [DateRange("1950-01-01", "2010-12-31")]
    public DateTime DateOfBirth { get; set; }

    [Required]
    public DateTime HireDate { get; set; }

    [Phone]
    public string? PhoneNumber { get; set; }

    [CollectionMaxLength(10)]
    public List<string> Skills { get; set; } = new();
}

public class Address
{
    [Required]
    [StringLength(200)]
    public string Street { get; set; }

    [Required]
    [StringLength(100)]
    public string City { get; set; }

    [StringLength(50)]
    public string? State { get; set; }

    [Required]
    [StringLength(20)]
    public string PostalCode { get; set; }

    [Required]
    [StringLength(100)]
    public string Country { get; set; }
}`,
      active: true
    }
  ];

  return (
    <div className={styles.container}>
      <div className={styles.header}>
        <h1 className={styles.title}>Real-World Examples</h1>
        <p className={styles.subtitle}>
          Explore practical validation scenarios and see how EasyValidate handles complex real-world use cases.
        </p>
      </div>

      <div className={styles.examples}>
        <section className={styles.example}>
          <h2 className={styles.exampleTitle}>User Registration & Profile</h2>
          <p className={styles.exampleDescription}>
            A common user model with name, email, age, and contact information validation.
            Perfect for registration forms and user profile management.
          </p>
          <CodeWindow windows={userValidationExample} variant="light" />
        </section>

        <section className={styles.example}>
          <h2 className={styles.exampleTitle}>E-commerce Product</h2>
          <p className={styles.exampleDescription}>
            Product validation for e-commerce applications including price, stock, and category validation.
            Demonstrates range validation and collection handling.
          </p>
          <CodeWindow windows={productValidationExample} variant="default" />
        </section>

        <section className={styles.example}>
          <h2 className={styles.exampleTitle}>Order Processing System</h2>
          <p className={styles.exampleDescription}>
            Complex order validation with nested objects, customer details, and order items.
            Shows how to validate hierarchical data structures.
          </p>
          <CodeWindow windows={orderValidationExample} variant="hero" />
        </section>

        <section className={styles.example}>
          <h2 className={styles.exampleTitle}>Web API Integration</h2>
          <p className={styles.exampleDescription}>
            ASP.NET Core Web API controller showing how to integrate EasyValidate in API endpoints.
            Includes request validation and error response handling.
          </p>
          <CodeWindow windows={apiValidationExample} variant="colorful" />
        </section>

        <section className={styles.example}>
          <h2 className={styles.exampleTitle}>Enterprise Data Models</h2>
          <p className={styles.exampleDescription}>
            Complex enterprise models with deep nesting, collections, and advanced validation rules.
            Demonstrates validation for company, department, and employee hierarchies.
          </p>
          <CodeWindow windows={complexValidationExample} variant="light" />
        </section>
      </div>

      <div className={styles.footer}>
        <h3>Need More Examples?</h3>
        <p>
          These examples showcase common validation patterns. For more specific use cases or custom validation scenarios, 
          check out our <a href="#getting-started">Getting Started guide</a> or explore the full documentation.
        </p>
      </div>
    </div>
  );
}

export default RealWorldExamples;
