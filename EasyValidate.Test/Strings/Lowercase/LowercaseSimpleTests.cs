using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Lowercase;

public class LowercaseSimpleTests
{
    [Fact]
    public void Lowercase_ValidLowercaseString_ReturnsValidResult()
    {
        // Arrange
        var model = new LowercaseModel
        {
            Username = "john.doe",
            Email = "user@example.com",
            Slug = "product-name"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Lowercase_InvalidUppercaseString_ReturnsInvalidResult()
    {
        // Arrange
        var model = new LowercaseModel
        {
            Username = "John.Doe", // Invalid: contains uppercase
            Email = "user@example.com",
            Slug = "product-name"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Username)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Email)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Slug)));
        var usernameErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Username))).ToList();
        Assert.Single(usernameErrors);
        Assert.Contains("must be lowercase", usernameErrors[0].FormattedMessage);
    }

    [Fact]
    public void Lowercase_NullString_ReturnsValidResult()
    {
        // Arrange
        var model = new LowercaseModel
        {
            Username = "valid.username",
            Email = null, // Null should be valid
            Slug = "product-name"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Theory]
    [InlineData("hello")]
    [InlineData("world")]
    [InlineData("user.name")]
    [InlineData("test with spaces")]
    [InlineData("numbers 123 and symbols !@#")]
    public void Lowercase_ValidLowercaseVariations_ReturnsValidResult(string value)
    {
        // Arrange
        var model = new LowercaseModel
        {
            Username = value,
            Email = "valid",
            Slug = "valid"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Theory]
    [InlineData("Hello")]
    [InlineData("WORLD")]
    [InlineData("User.Name")]
    [InlineData("Test With Spaces")]
    [InlineData("UPPERCASE")]
    public void Lowercase_InvalidUppercaseVariations_ReturnsInvalidResult(string value)
    {
        // Arrange
        var model = new LowercaseModel
        {
            Username = value,
            Email = "valid",
            Slug = "valid"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Username)));
        var usernameErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Username))).ToList();
        Assert.Single(usernameErrors);
        Assert.Contains("must be lowercase", usernameErrors[0].FormattedMessage);
    }

    [Fact]
    public void Lowercase_OnlyNumbersAndSymbols_ReturnsValidResult()
    {
        // Arrange
        var model = new LowercaseModel
        {
            Username = "123456789",
            Email = "!@#$%^&*()",
            Slug = "123-abc-!@#"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }
}
