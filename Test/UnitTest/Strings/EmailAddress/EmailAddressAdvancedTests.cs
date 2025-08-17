using EasyValidate.Abstractions;

using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.EmailAddress;

public class EmailAddressAdvancedTests
{
    [Fact]
    public void Validate_NestedObjectWithValidEmails_ReturnsValidResult()
    {
        // Arrange
        var model = new EmailAddressNestedModel
        {
            AdminEmail = "admin@company.com",
            UserEmails = new EmailAddressModel
            {
                Email = "user@example.com",
                ContactEmail = "contact@company.org",
                OptionalEmail = "optional@test.net"
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Validate_NestedObjectWithInvalidEmails_ReturnsInvalidResultWithPrefixedErrors()
    {
        // Arrange
        var model = new EmailAddressNestedModel
        {
            AdminEmail = "admin@company.com",
            UserEmails = new EmailAddressModel
            {
                Email = "invalid.email", // Invalid email
                ContactEmail = "contact@company.org",
                OptionalEmail = "another.invalid" // Invalid email
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(2, result.GetAllErrors().Count());

        var emailErrors = result.GetAllErrors().Where(e => e.PropertyName == "UserEmails.Email").ToList();
        var optionalEmailErrors = result.GetAllErrors().Where(e => e.PropertyName == "UserEmails.OptionalEmail").ToList();

        Assert.Single(emailErrors);
        Assert.Single(optionalEmailErrors);
        Assert.Contains("must be a valid email address", emailErrors[0].FormattedMessage);
        Assert.Contains("must be a valid email address", optionalEmailErrors[0].FormattedMessage);
    }

    [Fact]
    public void Validate_NullNestedObject_ReturnsValidResult()
    {
        // Arrange
        var model = new EmailAddressNestedModel
        {
            AdminEmail = "admin@company.com",
            UserEmails = null
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Validate_WithCustomFormatter_FormatsErrorsCorrectly()
    {
        // Arrange
        var model = new EmailAddressModel
        {
            Email = "invalid.email",
            ContactEmail = "valid@example.com",
            OptionalEmail = "another@valid.com"
        };
        var formatter = new CustomTestFormatter();

        // Act
        var result = model.Validate((conf) => conf.SetFormatter(formatter));

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.GetAllErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName == "Email");
        var emailErrors = result.GetAllErrors().Where(e => e.PropertyName == "Email").ToList();
        Assert.Single(emailErrors);
        Assert.Contains("CUSTOM:", emailErrors[0].FormattedMessage);
        Assert.Contains("must be a valid email address", emailErrors[0].FormattedMessage);
    }

    [Fact]
    public void Validate_EmailWithSpaces_ReturnsInvalidResult()
    {
        // Arrange
        var model = new EmailAddressModel
        {
            Email = "test @example.com", // Email with space
            ContactEmail = "valid@example.com",
            OptionalEmail = "another@valid.com"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.GetAllErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName == "Email");
        Assert.Contains(result.GetAllErrors().Where(e => e.PropertyName == "Email"), e => e.FormattedMessage.Contains("must be a valid email address"));
    }

    [Fact]
    public void Validate_EmailWithInvalidTLD_ReturnsInvalidResult()
    {
        // Arrange
        var model = new EmailAddressModel
        {
            Email = "test@example", // Missing TLD
            ContactEmail = "valid@example.com",
            OptionalEmail = "another@valid.com"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.GetAllErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName == "Email");
        Assert.Contains(result.GetAllErrors().Where(e => e.PropertyName == "Email"), e => e.FormattedMessage.Contains("must be a valid email address"));
    }

    [Fact]
    public void Validate_EmailWithSpecialChars_ReturnsValidResult()
    {
        // Arrange
        var model = new EmailAddressModel
        {
            Email = "test+tag@example.com", // Valid email with special chars
            ContactEmail = "user.name@example.com",
            OptionalEmail = "user_name@example.org"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Validate_NullOptionalEmail_ReturnsValidResult()
    {
        // Arrange
        var model = new EmailAddressModel
        {
            Email = "valid@example.com",
            ContactEmail = "contact@company.org",
            OptionalEmail = null // Null optional email should be valid
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Validate_BothMainAndNestedInvalidEmails_ReturnsAllErrors()
    {
        // Arrange
        var model = new EmailAddressNestedModel
        {
            AdminEmail = "invalid.admin", // Invalid main email
            UserEmails = new EmailAddressModel
            {
                Email = "invalid.user", // Invalid nested email
                ContactEmail = "valid@company.org",
                OptionalEmail = "invalid.optional" // Invalid nested optional email
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(3, result.GetAllErrors().Count());

        var adminEmailErrors = result.GetAllErrors().Where(e => e.PropertyName == "AdminEmail").ToList();
        var emailErrors = result.GetAllErrors().Where(e => e.PropertyName == "UserEmails.Email").ToList();
        var optionalEmailErrors = result.GetAllErrors().Where(e => e.PropertyName == "UserEmails.OptionalEmail").ToList();

        Assert.Single(adminEmailErrors);
        Assert.Single(emailErrors);
        Assert.Single(optionalEmailErrors);
        Assert.Contains("must be a valid email address", adminEmailErrors[0].FormattedMessage);
        Assert.Contains("must be a valid email address", emailErrors[0].FormattedMessage);
        Assert.Contains("must be a valid email address", optionalEmailErrors[0].FormattedMessage);
    }

    [Fact]
    public void Validate_InternationalDomainEmail_ReturnsValidResult()
    {
        // Arrange
        var model = new EmailAddressModel
        {
            Email = "test@example.com", // Standard international domain
            ContactEmail = "user@domain.co.uk", // Country code domain
            OptionalEmail = "example@subdomain.example.org"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }
}

public class CustomTestFormatter : IFormatter
{
    public string Format<T>(AttributeResult result, T value)
    {
        return $"CUSTOM: {string.Format(result.MessageTemplate, result.MessageArgs)}";
    }
}
