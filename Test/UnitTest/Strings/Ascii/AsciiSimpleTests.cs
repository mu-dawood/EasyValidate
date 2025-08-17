using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Ascii;

public class AsciiSimpleTests
{
    [Fact]
    public void Ascii_ValidAsciiString_ReturnsValidResult()
    {
        // Arrange
        var model = new AsciiModel
        {
            Text = "Hello World! 123",
            Description = "Basic ASCII text with numbers and symbols",
            Content = "ABCDEFGHIJKLMNOPQRSTUVWXYZ abcdefghijklmnopqrstuvwxyz 0123456789"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Ascii_InvalidNonAsciiString_ReturnsInvalidResult()
    {
        // Arrange
        var model = new AsciiModel
        {
            Text = "Hello Wörld! 🌍", // Contains non-ASCII characters
            Description = "Basic ASCII text",
            Content = "Standard content"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Text)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Description)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Content)));
        var textErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Text))).ToList();
        Assert.Single(textErrors);
        Assert.Contains("must contain only ASCII characters", textErrors[0].FormattedMessage);
    }

    [Fact]
    public void Ascii_EmptyString_ReturnsInvalidResult()
    {
        // Arrange
        var model = new AsciiModel
        {
            Text = "", // Empty string should fail ASCII validation
            Description = "Valid ASCII",
            Content = "Valid content"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Text)));
        var textErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Text))).ToList();
        Assert.Single(textErrors);
        Assert.Contains("must contain only ASCII characters", textErrors[0].FormattedMessage);
    }

    [Fact]
    public void Ascii_NullString_ReturnsValidResult()
    {
        // Arrange
        var model = new AsciiModel
        {
            Text = "Valid ASCII",
            Description = null, // Null should be valid
            Content = "Valid content"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Theory]
    [InlineData("Hello World")]
    [InlineData("123456789")]
    [InlineData("!@#$%^&*()")]
    [InlineData("ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz")]
    [InlineData(" \t\n\r")]
    public void Ascii_ValidAsciiVariations_ReturnsValidResult(string value)
    {
        // Arrange
        var model = new AsciiModel
        {
            Text = value,
            Description = "Valid",
            Content = "Valid"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Theory]
    [InlineData("Héllo")]
    [InlineData("Wörld")]
    [InlineData("🌍")]
    [InlineData("中文")]
    [InlineData("العربية")]
    [InlineData("Русский")]
    public void Ascii_InvalidNonAsciiVariations_ReturnsInvalidResult(string value)
    {
        // Arrange
        var model = new AsciiModel
        {
            Text = value,
            Description = "Valid",
            Content = "Valid"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Text)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Description)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Content)));
        var textErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Text))).ToList();
        Assert.Single(textErrors);
        Assert.Contains("must contain only ASCII characters", textErrors[0].FormattedMessage);
    }
}
