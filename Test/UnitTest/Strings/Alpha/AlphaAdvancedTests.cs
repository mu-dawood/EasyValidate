using EasyValidate.Abstractions;

using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Alpha;

public class AlphaAdvancedTests
{
    [Fact]
    public void Alpha_NestedObjectValidation_CorrectlyMergesErrors()
    {
        // Arrange
        var model = new AlphaNestedModel
        {
            Name = "John123",  // Invalid
            NestedModel = new AlphaModel
            {
                FirstName = "Jane456",  // Invalid
                LastName = "Smith",     // Valid
                MiddleName = "Ann-Marie" // Invalid
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());

        // Check main object error
        Assert.True(result.Property(nameof(model.Name))?.HasErrors());

        // Check nested object errors with proper prefixing
        Assert.True(result.Property("NestedModel")?.HasErrors("FirstName"));
        Assert.False(result.Property("NestedModel")?.HasErrors("LastName"));
        Assert.True(result.Property("NestedModel")?.HasErrors("MiddleName"));

        // Verify error count
        Assert.Equal(3, result.GetAllErrors().Count());

        // Verify error messages contain proper prefixes
        var nestedFirstNameErrors = result.GetAllErrors().Where(e => e.PropertyName == "NestedModel.FirstName").ToList();
        Assert.Single(nestedFirstNameErrors);
        Assert.Equal("AlphaValidationError", nestedFirstNameErrors[0].ErrorCode);
    }

    [Fact]
    public void Alpha_NestedObjectWithNullReference_HandlesGracefully()
    {
        // Arrange
        var model = new AlphaNestedModel
        {
            Name = "ValidName",
            NestedModel = null
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void Alpha_NestedObjectAllValid_ReturnsValidResult()
    {
        // Arrange
        var model = new AlphaNestedModel
        {
            Name = "John",
            NestedModel = new AlphaModel
            {
                FirstName = "Jane",
                LastName = "Smith",
                MiddleName = "Ann"
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
    public void Alpha_MultipleErrorsOnSameProperty_CollectsAllErrors()
    {
        // This test assumes multiple Alpha attributes could be applied
        // For now, testing with single attribute
        var model = new AlphaModel
        {
            FirstName = "John123",
            LastName = "Smith",
            MiddleName = "William"
        };

        var result = model.Validate();

        Assert.False(result.IsValid());

        var firstNameErrors = result.GetAllErrors().Where(e => e.PropertyName == nameof(model.FirstName)).ToList();
        Assert.Single(firstNameErrors); // Only one Alpha attribute per property
    }

    [Fact]
    public void Alpha_CustomFormatter_UsesCustomFormat()
    {
        // Arrange
        var customFormatter = new CustomTestFormatter();
        var model = new AlphaModel
        {
            FirstName = "John123",
            LastName = "Smith",
            MiddleName = "William"
        };

        // Act
        var result = model.Validate((ctf) => ctf.SetFormatter(customFormatter));

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.Property(nameof(model.FirstName))?.HasErrors());

        var firstNameErrors = result.GetAllErrors().Where(e => e.PropertyName == nameof(model.FirstName)).ToList();
        Assert.Single(firstNameErrors);
        Assert.Equal("CUSTOM: The FirstName field must contain only alphabetic characters.", firstNameErrors[0].FormattedMessage);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData("\t")]
    [InlineData("\n")]
    [InlineData("\r\n")]
    public void Alpha_VariousWhitespaceInputs_ReturnsInvalidResult(string value)
    {
        // Arrange
        var model = new AlphaModel
        {
            FirstName = value,
            LastName = "Valid",
            MiddleName = "Test"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.Property(nameof(model.FirstName))?.HasErrors());
    }

    [Fact]
    public void Alpha_LargeValidString_ReturnsValidResult()
    {
        // Arrange
        var largeString = new string('A', 1000); // 1000 character string of 'A's
        var model = new AlphaModel
        {
            FirstName = largeString,
            LastName = "Valid",
            MiddleName = "Test"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
    }

    [Fact]
    public void Alpha_UnicodeCharacters_ReturnsValidResult()
    {
        // Arrange
        var model = new AlphaModel
        {
            FirstName = "Åse",      // Nordic characters
            LastName = "José",      // Spanish characters  
            MiddleName = "Müller"   // German characters
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
    }

    [Theory]
    [InlineData("Café")]     // Contains é
    [InlineData("Naïve")]    // Contains ï
    [InlineData("Résumé")]   // Contains é
    [InlineData("Piñata")]   // Contains ñ
    public void Alpha_AccentedCharacters_ReturnsValidResult(string value)
    {
        // Arrange
        var model = new AlphaModel
        {
            FirstName = value,
            LastName = "Valid",
            MiddleName = "Test"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
    }
}

public class CustomTestFormatter : IFormatter
{

    public string Format(string messageTemplate, object[] args)
    {
        return $"CUSTOM: {string.Format(messageTemplate, args)}";
    }

}
