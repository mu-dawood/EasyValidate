using EasyValidate.Abstractions;

using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Ascii;

public class AsciiAdvancedTests
{
    [Fact]
    public void Ascii_NestedObjectValidation_CorrectlyMergesErrors()
    {
        // Arrange
        var model = new AsciiNestedModel
        {
            MainText = "Valid ASCII",
            Details = new AsciiModel
            {
                Text = "Héllo", // Invalid: contains non-ASCII
                Description = "Valid ASCII",
                Content = "Valid content",
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.True(result.Property("Details")?.HasErrors("Text"));
        Assert.False(result.Property(nameof(model.MainText))?.HasErrors());
        Assert.False(result.Property("Details")?.HasErrors("Description"));
        Assert.False(result.Property("Details")?.HasErrors("Content"));

        var detailsTextErrors = result.GetAllErrors().Where(e => e.PropertyName == "Details.Text").ToList();
        Assert.Single(detailsTextErrors);
        Assert.Contains("must contain only ASCII characters", detailsTextErrors[0].FormattedMessage);
    }

    [Fact]
    public void Ascii_NestedObjectWithNullReference_HandlesGracefully()
    {
        // Arrange
        var model = new AsciiNestedModel
        {
            MainText = "Valid ASCII",
            Details = null
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Ascii_NestedObjectAllValid_ReturnsValidResult()
    {
        // Arrange
        var model = new AsciiNestedModel
        {
            MainText = "Valid ASCII Main",
            Details = new AsciiModel
            {
                Text = "Valid ASCII Text",
                Description = "Valid ASCII Description",
                Content = "Valid ASCII Content"
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
    public void Ascii_WithCustomFormatter_FormatsErrorsCorrectly()
    {
        // Arrange
        var model = new AsciiModel
        {
            Text = "Héllo",
            Description = "Valid",
            Content = "Valid"
        };
        var formatter = new CustomTestFormatter();

        // Act
        var result = model.Validate((conf) => conf.SetFormatter(formatter));

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.GetAllErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName == "Text");
        var textErrors = result.GetAllErrors().Where(e => e.PropertyName == "Text").ToList();
        Assert.Single(textErrors);
        Assert.Contains("CUSTOM:", textErrors[0].FormattedMessage);
        Assert.Contains("must contain only ASCII characters", textErrors[0].FormattedMessage);
    }

    [Fact]
    public void Ascii_ExtendedAsciiCharacters_ReturnsValidResult()
    {
        // Arrange - Extended ASCII characters (128-255) should be considered valid ASCII
        var model = new AsciiModel
        {
            Text = "Standard ASCII only: abcABC123!@#",
            Description = "More ASCII: $%^&*()",
            Content = "Even more: []{}|\\;':\",./<>?"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Theory]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public void Ascii_VariousWhitespaceInputs_ReturnsValidResult(string value)
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
    }

    [Theory]
    [InlineData("")]
    public void Ascii_VariousWhitespaceInputs_ReturnsInvalidResult(string value)
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
    }

    [Fact]
    public void Ascii_LargeValidString_ReturnsValidResult()
    {
        // Arrange
        var largeText = new string('A', 10000) + new string('1', 5000) + new string('!', 2000);
        var model = new AsciiModel
        {
            Text = largeText,
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

    [Fact]
    public void Ascii_MultipleErrorsOnDifferentProperties_CollectsAllErrors()
    {
        // Arrange
        var model = new AsciiModel
        {
            Text = "Héllo", // Invalid
            Description = "Wörld", // Invalid
            Content = "🌍" // Invalid
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(3, result.GetAllErrors().Count());
        Assert.True(result.Property(nameof(model.Text))?.HasErrors());
        Assert.True(result.Property(nameof(model.Description))?.HasErrors());
        Assert.True(result.Property(nameof(model.Content))?.HasErrors());
    }
}

public class CustomTestFormatter : IFormatter
{

    public string Format(string messageTemplate, object[] args)
    {
        return $"CUSTOM: {string.Format(messageTemplate, args)}";
    }
}
