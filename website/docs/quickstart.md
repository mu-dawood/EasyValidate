---
id: quickstart
title: Quick Start Guide
sidebar_position: 3
---

import { DocsWrapper, FeatureCard, InfoBox, DocSection, FeatureGrid ,InlineSnippet} from '@site/src/components/DocsComponents';

<DocsWrapper>

# Quick Start Guide

<DocSection 
  title="Welcome to EasyValidate" 
  subtitle="Get up and running with EasyValidate in minutes. This guide covers the essential patterns and usage scenarios."
  icon="🚀"
  background="gradient"
>

<FeatureGrid>
  <FeatureCard
    icon="⚡"
    title="Zero Reflection"
    description="Compile-time code generation for maximum performance"
    color="primary"
  />
  <FeatureCard
    icon="🛠️"
    title="Easy Setup"
    description="Just add attributes to your properties and you're done"
    color="secondary"
  />
  <FeatureCard
    icon="🌍"
    title="Localization Ready"
    description="Built-in support for multiple languages and cultures"
    color="accent"
  />
</FeatureGrid>

</DocSection>

## Basic Model Validation

<DocSection 
  title="Step 1: Define Your Model" 
  subtitle="Start by adding validation attributes to your model properties"
  icon="📝"
>

<InfoBox type="tip" title="Pro Tip">
EasyValidate works with any class - POCOs, DTOs, ViewModels, or Entity Framework entities!
</InfoBox>

<InlineSnippet language="csharp">{`
using EasyValidate.Attributes;

public class User
{
    [NotEmpty]
    [EmailAddress]
    public string Email { get; set; }

    [Range(18, 99)]
    public int Age { get; set; }

    [NotEmpty]
    [MinLength(2)]
    [MaxLength(50)]
    public string FirstName { get; set; }

    [NotEmpty]
    [MinLength(2)]
    [MaxLength(50)]
    public string LastName { get; set; }

    [Phone]
    public string PhoneNumber { get; set; }
}
`}</InlineSnippet>

</DocSection>

<DocSection 
  title="Step 2: Perform Validation" 
  subtitle="The EasyValidate source generator automatically creates validation methods"
  icon="✅"
>

<InfoBox type="note" title="Source Generation">
No reflection needed! The validation code is generated at compile-time for optimal performance.
</InfoBox>

<InlineSnippet language="csharp">{`
public class UserService
{
    public async Task&lt;bool&gt; CreateUser(User user)
    {
        // Validate the entire model
        var result = user.Validate();
        
        if (!result.IsValid())
        {
            // Handle validation errors
            foreach (var error in result.Errors)
            {
                Console.WriteLine($"Property: {"{error.Key}"}");
                foreach (var validationError in error.Value)
                {
                    Console.WriteLine($"  • {"{validationError.FormattedMessage}"}");
                }
            }
            return false;
        }

        // Process valid user
        await SaveUserToDatabase(user);
        return true;
    }
}
`}</InlineSnippet>

</DocSection>

<DocSection 
  title="3. Validate Specific Properties" 
  subtitle="Check individual properties for validation errors"
  icon="🔍"
>

<InfoBox type="note" title="Validation Details">
This section explains how to validate specific properties of a model.
</InfoBox>

<InlineSnippet language="csharp">{`
public void ValidateEmail(User user)
{
    var result = user.Validate();
    
    // Check if email is valid
    if (!result.IsValid("Email"))
    {
        Console.WriteLine("Email address is invalid");
    }
    
    // Alternative: Check if email has errors
    if (result.HasErrors("Email"))
    {
        var emailErrors = result.Errors["Email"];
        foreach (var error in emailErrors)
        {
            Console.WriteLine($"Email error: {"{error.FormattedMessage}"}");
        }
    }
}
`}</InlineSnippet>

</DocSection>

## Advanced Scenarios

<DocSection 
  title="Custom Validation Messages" 
  subtitle="Use a custom formatter for localized or custom error messages"
  icon="✍️"
>

<InfoBox type="info" title="Custom Messages">
Learn how to define and use custom validation messages.
</InfoBox>

<InlineSnippet language="csharp">{`
public class CustomFormatter : IFormatter
{
    private readonly Dictionary&lt;string, string&gt; _messages = new()
    {
        { "NotEmptyValidationError", "The {"{0}"} field is required." },
        { "EmailAddressValidationError", "Please enter a valid email address for {"{0}"}." },
        { "RangeValidationError", "The {"{0}"} must be between {"{1}"} and {"{2}"}." }
    };

    public string Format(string message, object?[] args)
    {
        // Use custom message if available
        var errorCode = args.Length &gt; 0 ? args[0]?.ToString() : string.Empty;
        if (_messages.ContainsKey(errorCode))
        {
            return string.Format(_messages[errorCode], args.Skip(1).ToArray());
        }
        
        return string.Format(message, args);
    }
}

// Usage
var customFormatter = new CustomFormatter();
var result = user.Validate(customFormatter);
`}</InlineSnippet>

</DocSection>


<DocSection 
  title="Nested Object Validation" 
  subtitle="Automatically validate nested objects"
  icon="🏠"
>

<InfoBox type="info" title="Nested Validation">
EasyValidate supports nested object validation out of the box.
</InfoBox>

<InlineSnippet language="csharp">{`
public class Address
{
    [NotEmpty]
    [MaxLength(100)]
    public string Street { get; set; }

    [NotEmpty]
    [MaxLength(50)]
    public string City { get; set; }

    [NotEmpty]
    [MaxLength(10)]
    public string PostalCode { get; set; }
}

public class Customer
{
    [NotEmpty]
    public string Name { get; set; }

    // Nested validation - Address will be automatically validated
    public Address Address { get; set; }
    
    [NotEmpty&lt;Address&gt;]
    public List&lt;Address&gt; ShippingAddresses { get; set; }
}

// Usage
var customer = new Customer
{
    Name = "John Doe",
    Address = new Address { /* ... */ },
    ShippingAddresses = new List&lt;Address&gt; { /* ... */ }
};

var result = customer.Validate();
// This will validate the customer AND all nested addresses
`}</InlineSnippet>

</DocSection>

<DocSection 
  title="Collection Validation" 
  subtitle="Validate collections and their contents"
  icon="📚"
>

<InfoBox type="info" title="Collection Validation">
Learn how to validate collections and ensure their integrity.
</InfoBox>

<InlineSnippet language="csharp">{`
public class Order
{
    [NotEmpty]
    public string OrderNumber { get; set; }

    [NotEmpty&lt;OrderItem&gt;]
    [MinLength&lt;OrderItem&gt;(1)]
    [MaxLength&lt;OrderItem&gt;(50)]
    public List&lt;OrderItem&gt; Items { get; set; }

    [Unique&lt;string&gt;]
    public List&lt;string&gt; Categories { get; set; }
}

public class OrderItem
{
    [NotEmpty]
    public string ProductName { get; set; }

    [Positive]
    public decimal Price { get; set; }

    [Range(1, 1000)]
    public int Quantity { get; set; }
}
`}</InlineSnippet>

</DocSection>

<DocSection 
  title="Conditional Validation" 
  subtitle="Implement conditional logic for validation"
  icon="🔄"
>

<InfoBox type="info" title="Conditional Logic">
Use conditional logic for complex validation scenarios.
</InfoBox>

<InlineSnippet language="csharp">{`
public class PaymentInfo
{
    [NotEmpty]
    public string PaymentMethod { get; set; }

    // Only validate credit card if payment method is "CreditCard"
    public string CreditCardNumber { get; set; }

    public ValidationResult ValidateConditionally()
    {
        var result = this.Validate();

        // Add conditional validation
        if (PaymentMethod == "CreditCard")
        {
            if (string.IsNullOrEmpty(CreditCardNumber))
            {
                // Manual error addition for complex scenarios
                // Note: This requires extending the ValidationResult
            }
        }

        return result;
    }
}
`}</InlineSnippet>

</DocSection>
<DocSection 
  title="Understanding the ValidationResult Structure" 
  subtitle="Dive into the structure of ValidationResult"
  icon="📊"
>

<InfoBox type="info" title="ValidationResult Details">
Understand how to work with ValidationResult.
</InfoBox>

<InlineSnippet language="csharp">{`
var result = user.Validate();

// Check overall validity
bool isValid = result.IsValid();

// Check for any errors
bool hasErrors = result.HasErrors();

// Check specific property
bool emailValid = result.IsValid("Email");
bool emailHasErrors = result.HasErrors("Email");

// Access all errors
IReadOnlyDictionary&lt;string, IReadOnlyList&lt;ValidationError&gt;&gt; allErrors = result.Errors;

// Process errors
foreach (var propertyErrors in result.Errors)
{
    string propertyName = propertyErrors.Key;
    var errors = propertyErrors.Value;
    
    foreach (var error in errors)
    {
        Console.WriteLine($"Property: {"{propertyName}"}");
        Console.WriteLine($"Error Code: {"{error.ErrorCode}"}");
        Console.WriteLine($"Message: {"{error.FormattedMessage}"}");
        Console.WriteLine($"Attribute: {"{error.AttributeName}"}");
    }
}
`}</InlineSnippet>

</DocSection>
<DocSection 
  title="Error Handling Patterns" 
  subtitle="Handle validation errors effectively"
  icon="⚠️"
>

<InfoBox type="info" title="Error Handling">
Learn best practices for handling validation errors.
</InfoBox>

<InlineSnippet language="csharp">{`
public class ValidationHelper
{
    public static bool ValidateAndLog&lt;T&gt;(T model, ILogger logger) where T : class
    {
        var result = model.Validate();
        
        if (result.IsValid())
        {
            logger.LogInformation("Validation passed for {"{ModelType}"}", typeof(T).Name);
            return true;
        }

        logger.LogWarning("Validation failed for {"{ModelType}"} with {"{ErrorCount}"} errors", 
            typeof(T).Name, result.Errors.Count);

        foreach (var error in result.Errors)
        {
            logger.LogError("Validation error in {"{Property}"}: {"{Message}"}", 
                error.Key, string.Join(", ", error.Value.Select(e => e.FormattedMessage)));
        }

        return false;
    }

    public static Dictionary&lt;string, List&lt;string&gt;&gt; GetErrorDictionary(ValidationResult result)
    {
        return result.Errors.ToDictionary(
            kvp => kvp.Key,
            kvp => kvp.Value.Select(e => e.FormattedMessage).ToList()
        );
    }
}
`}</InlineSnippet>

</DocSection>

## Integration Examples

<DocSection 
  title="ASP.NET Core Model Validation" 
  subtitle="Integrate EasyValidate with ASP.NET Core"
  icon="🌐"
>

<InfoBox type="info" title="ASP.NET Core">
Integrate EasyValidate seamlessly with ASP.NET Core.
</InfoBox>

<InlineSnippet language="csharp">{`
[ApiController]
[Route("api/[controller]")]
public class UsersController : ControllerBase
{
    [HttpPost]
    public IActionResult CreateUser([FromBody] User user)
    {
        var validationResult = user.Validate();
        
        if (!validationResult.IsValid())
        {
            var errors = ValidationHelper.GetErrorDictionary(validationResult);
            return BadRequest(new { Errors = errors });
        }

        // Process valid user
        return Ok(new { Message = "User created successfully" });
    }
}
`}</InlineSnippet>

</DocSection>

<DocSection 
  title="Entity Framework Integration" 
  subtitle="Use EasyValidate with Entity Framework"
  icon="🗄️"
>

<InfoBox type="info" title="Entity Framework">
Learn how to integrate EasyValidate with Entity Framework.
</InfoBox>

<InlineSnippet language="csharp">{`
public class ApplicationDbContext : DbContext
{
    public override int SaveChanges()
    {
        ValidateEntities();
        return base.SaveChanges();
    }

    private void ValidateEntities()
    {
        var entities = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified)
            .Select(e => e.Entity);

        foreach (var entity in entities)
        {
            if (entity is IValidate validatable)
            {
                var result = validatable.Validate();
                if (!result.IsValid())
                {
                    var errors = string.Join("; ", result.Errors.SelectMany(e => 
                        e.Value.Select(v => $"{"{e.Key}"}: {"{v.FormattedMessage}"}")));
                    throw new ValidationException($"Entity validation failed: {"{errors}"}");
                }
            }
        }
    }
}
`}</InlineSnippet>

</DocSection>

## Best Practices

1. **Apply attributes at the property level** for clear, declarative validation
2. **Use the analyzer package** for compile-time feedback
3. **Handle validation results consistently** across your application
4. **Consider custom formatters** for localization requirements
5. **Validate early** in your application pipeline
6. **Use nested validation** for complex object graphs
7. **Log validation failures** for debugging and monitoring

---

**Next Steps:**
- Explore the complete [Attributes Reference](attributes.md)
- Learn about [extending EasyValidate](extending.md) with custom attributes
- Set up [source generators and analyzers](analyzers.md) for optimal performance

</DocsWrapper>
