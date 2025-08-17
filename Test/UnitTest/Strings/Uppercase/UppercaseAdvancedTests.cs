using EasyValidate.Abstractions;

using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Uppercase;

public class UppercaseAdvancedTests
{
    [Fact]
    public void Uppercase_NestedObjectValidation_CorrectlyMergesErrors()
    {
        // Arrange
        var model = new UppercaseNestedModel
        {
            MainCode = "VALID MAIN",
            Details = new UppercaseModel
            {
                Title = "invalid title", // Invalid: lowercase
                Code = "VALID CODE",
                Category = "VALID CATEGORY"
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.True(result.Property("Details")?.HasErrors("Title"));
        Assert.False(result.Property(nameof(model.MainCode))?.HasErrors());
        Assert.False(result.Property("Details")?.HasErrors("Code"));
        Assert.False(result.Property("Details")?.HasErrors("Category"));

        var detailsTitleErrors = result.GetAllErrors().Where(e => e.PropertyName == "Details.Title").ToList();
        Assert.Single(detailsTitleErrors);
        Assert.Contains("must be uppercase", detailsTitleErrors[0].FormattedMessage);
    }

    [Fact]
    public void Uppercase_NestedObjectWithNullReference_HandlesGracefully()
    {
        // Arrange
        var model = new UppercaseNestedModel
        {
            MainCode = "VALID MAIN",
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
    public void Uppercase_NestedObjectAllValid_ReturnsValidResult()
    {
        // Arrange
        var model = new UppercaseNestedModel
        {
            MainCode = "VALID MAIN CODE",
            Details = new UppercaseModel
            {
                Title = "VALID TITLE",
                Code = "VALID CODE",
                Category = "VALID CATEGORY"
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
    public void Uppercase_WithCustomFormatter_FormatsErrorsCorrectly()
    {
        // Arrange
        var model = new UppercaseModel
        {
            Title = "invalid title",
            Code = "VALID",
            Category = "VALID"
        };
        var formatter = new CustomTestFormatter();

        // Act
        var result = model.Validate((conf) => conf.SetFormatter(formatter));

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.GetAllErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName == "Title");
        var titleErrors = result.GetAllErrors().Where(e => e.PropertyName == "Title").ToList();
        Assert.Single(titleErrors);
        Assert.Contains("CUSTOM:", titleErrors[0].FormattedMessage);
        Assert.Contains("must be uppercase", titleErrors[0].FormattedMessage);
    }

    [Fact]
    public void Uppercase_UnicodeUppercaseCharacters_ReturnsValidResult()
    {
        // Arrange
        var model = new UppercaseModel
        {
            Title = "ÀÁÂÃÄÅÆÇÈÉÊË", // Unicode uppercase characters
            Code = "ÌÍÎÏÐÑÒÓÔÕÖ",
            Category = "ØÙÚÛÜÝÞ"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Uppercase_UnicodeLowercaseCharacters_ReturnsInvalidResult()
    {
        // Arrange
        var model = new UppercaseModel
        {
            Title = "àáâãäåæçèéêë", // Unicode lowercase characters
            Code = "VALID",
            Category = "VALID"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.True(result.Property(nameof(model.Title))?.HasErrors());
        Assert.False(result.Property(nameof(model.Code))?.HasErrors());
        Assert.False(result.Property(nameof(model.Category))?.HasErrors());
    }



    [Fact]
    public void Uppercase_LargeValidString_ReturnsValidResult()
    {
        // Arrange
        var largeText = new string('A', 10000) + " " + new string('B', 5000) + " " + new string('1', 2000);
        var model = new UppercaseModel
        {
            Title = largeText,
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

    [Fact]
    public void Uppercase_MultipleErrorsOnDifferentProperties_CollectsAllErrors()
    {
        // Arrange
        var model = new UppercaseModel
        {
            Title = "invalid title", // Invalid
            Code = "invalid code", // Invalid
            Category = "invalid category" // Invalid
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(3, result.GetAllErrors().Count());
        Assert.True(result.Property(nameof(model.Title))?.HasErrors());
        Assert.True(result.Property(nameof(model.Code))?.HasErrors());
        Assert.True(result.Property(nameof(model.Category))?.HasErrors());
    }
}

public class CustomTestFormatter : IFormatter
{

    public string Format(string messageTemplate, object[] args)
    {
        return $"CUSTOM: {string.Format(messageTemplate, args)}";
    }
}
