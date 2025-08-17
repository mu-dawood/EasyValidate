using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Uppercase;

public class UppercaseSimpleTests
{
    [Fact]
    public void Uppercase_ValidUppercaseString_ReturnsValidResult()
    {
        // Arrange
        var model = new UppercaseModel
        {
            Title = "HELLO WORLD",
            Code = "ABC123",
            Category = "PRODUCT"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Uppercase_InvalidLowercaseString_ReturnsInvalidResult()
    {
        // Arrange
        var model = new UppercaseModel
        {
            Title = "hello world", // Invalid: contains lowercase
            Code = "ABC123",
            Category = "PRODUCT"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Title)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Code)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Category)));
        var titleErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Title))).ToList();
        Assert.Single(titleErrors);
        Assert.Contains("must be uppercase", titleErrors[0].FormattedMessage);
    }

    [Fact]
    public void Uppercase_MixedCaseString_ReturnsInvalidResult()
    {
        // Arrange
        var model = new UppercaseModel
        {
            Title = "Hello World", // Invalid: mixed case
            Code = "ABC123",
            Category = "PRODUCT"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Title)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Code)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Category)));
        var titleErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Title))).ToList();
        Assert.Single(titleErrors);
        Assert.Contains("must be uppercase", titleErrors[0].FormattedMessage);
    }

    [Fact]
    public void Uppercase_EmptyString_ReturnsValidResult()
    {
        // Arrange
        var model = new UppercaseModel
        {
            Title = "", // Empty string should fail
            Code = "ABC123",
            Category = "PRODUCT"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
    }

    [Fact]
    public void Uppercase_NullString_ReturnsValidResult()
    {
        // Arrange
        var model = new UppercaseModel
        {
            Title = "VALID UPPERCASE",
            Code = null, // Null should be valid
            Category = "PRODUCT"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Theory]
    [InlineData("HELLO")]
    [InlineData("WORLD")]
    [InlineData("ABC123")]
    [InlineData("TEST WITH SPACES")]
    [InlineData("NUMBERS 123 AND SYMBOLS !@#")]
    public void Uppercase_ValidUppercaseVariations_ReturnsValidResult(string value)
    {
        // Arrange
        var model = new UppercaseModel
        {
            Title = value,
            Code = "VALID",
            Category = "VALID"
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
    [InlineData("Hello")]
    [InlineData("HELLO world")]
    [InlineData("Hello World")]
    [InlineData("test")]
    public void Uppercase_InvalidMixedOrLowercaseVariations_ReturnsInvalidResult(string value)
    {
        // Arrange
        var model = new UppercaseModel
        {
            Title = value,
            Code = "VALID",
            Category = "VALID"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Title)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Code)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Category)));
        var titleErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Title))).ToList();
        Assert.Single(titleErrors);
        Assert.Contains("must be uppercase", titleErrors[0].FormattedMessage);
    }

    [Fact]
    public void Uppercase_OnlyNumbersAndSymbols_ReturnsValidResult()
    {
        // Arrange
        var model = new UppercaseModel
        {
            Title = "123456789",
            Code = "!@#$%^&*()",
            Category = "123 ABC !@#"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }
}
