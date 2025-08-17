using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.EmailAddress;

public class EmailAddressSimpleTests
{
    [Fact]
    public void EmailAddress_ValidEmail_ReturnsValidResult()
    {
        // Arrange
        var model = new EmailAddressModel
        {
            Email = "test@example.com",
            ContactEmail = "contact@test.org",
            OptionalEmail = "optional@sample.net"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void EmailAddress_InvalidEmailFormat_ReturnsInvalidResult()
    {
        // Arrange
        var model = new EmailAddressModel
        {
            Email = "invalid-email", // Invalid email format
            ContactEmail = "contact@test.org",
            OptionalEmail = "optional@sample.net"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Email)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.ContactEmail)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalEmail)));
        var emailErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Email))).ToList();
        Assert.Single(emailErrors);
        Assert.Contains("must be a valid email address", emailErrors[0].FormattedMessage);
    }

    [Fact]
    public void EmailAddress_EmptyString_ReturnsInvalidResult()
    {
        // Arrange
        var model = new EmailAddressModel
        {
            Email = "", // Empty string should fail EmailAddress validation
            ContactEmail = "contact@test.org",
            OptionalEmail = "optional@sample.net"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Email)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.ContactEmail)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalEmail)));
        var emailErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Email))).ToList();
        Assert.Single(emailErrors);
        Assert.Contains("must be a valid email address", emailErrors[0].FormattedMessage);
    }

    [Fact]
    public void EmailAddress_MultipleInvalidEmails_ReturnsMultipleErrors()
    {
        // Arrange
        var model = new EmailAddressModel
        {
            Email = "invalid-email1", // Invalid
            ContactEmail = "invalid-email2", // Invalid
            OptionalEmail = "invalid-email3" // Invalid
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(3, result.GetAllErrors().Count());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Email)));
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.ContactEmail)));
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalEmail)));
    }

    [Theory]
    [InlineData("test@example.com")]
    [InlineData("user@domain.org")]
    [InlineData("contact@website.net")]
    [InlineData("admin@company.co.uk")]
    public void EmailAddress_ValidEmails_ReturnsValidResult(string email)
    {
        // Arrange
        var model = new EmailAddressModel
        {
            Email = email,
            ContactEmail = "contact@test.org",
            OptionalEmail = "optional@sample.net"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Theory]
    [InlineData("invalid-email")]
    [InlineData("@domain.com")]
    [InlineData("user@")]
    [InlineData("user@@domain.com")]
    [InlineData("user@domain")]
    public void EmailAddress_InvalidEmails_ReturnsInvalidResult(string email)
    {
        // Arrange
        var model = new EmailAddressModel
        {
            Email = email,
            ContactEmail = "contact@test.org",
            OptionalEmail = "optional@sample.net"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Email)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.ContactEmail)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalEmail)));
        var emailErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Email))).ToList();
        Assert.Single(emailErrors);
        Assert.Contains("must be a valid email address", emailErrors[0].FormattedMessage);
    }
}
