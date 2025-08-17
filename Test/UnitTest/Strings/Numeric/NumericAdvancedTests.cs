using EasyValidate.Abstractions;

using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Numeric;

public class NumericAdvancedTests
{
    [Fact]
    public void Validate_NestedObjectWithValidNumericValues_ReturnsValidResult()
    {
        // Arrange
        var model = new NumericNestedModel
        {
            MainValue = "123456",
            Details = new NumericModel
            {
                Amount = "999.99",
                Percentage = "100",
                OptionalValue = "42"
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
    public void Validate_NestedObjectWithInvalidNumericValues_ReturnsInvalidResultWithPrefixedErrors()
    {
        // Arrange
        var model = new NumericNestedModel
        {
            MainValue = "123456",
            Details = new NumericModel
            {
                Amount = "abc123", // Invalid numeric value
                Percentage = "100",
                OptionalValue = "not-a-number" // Invalid numeric value
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(2, result.GetAllErrors().Count());

        var amountErrors = result.GetAllErrors().Where(e => e.PropertyName == "Details.Amount").ToList();
        var optionalValueErrors = result.GetAllErrors().Where(e => e.PropertyName == "Details.OptionalValue").ToList();

        Assert.Single(amountErrors);
        Assert.Single(optionalValueErrors);
        Assert.Contains("must contain only numeric characters", amountErrors[0].FormattedMessage);
        Assert.Contains("must contain only numeric characters", optionalValueErrors[0].FormattedMessage);
    }

    [Fact]
    public void Validate_NullNestedObject_ReturnsValidResult()
    {
        // Arrange
        var model = new NumericNestedModel
        {
            MainValue = "123456",
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
    public void Validate_WithCustomFormatter_FormatsErrorsCorrectly()
    {
        // Arrange
        var model = new NumericModel
        {
            Amount = "not-numeric",
            Percentage = "100",
            OptionalValue = "42"
        };
        var formatter = new CustomTestFormatter();

        // Act
        var result = model.Validate((conf) => conf.SetFormatter(formatter));

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.GetAllErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName == "Amount");
        var amountErrors = result.GetAllErrors().Where(e => e.PropertyName == "Amount").ToList();
        Assert.Single(amountErrors);
        Assert.Contains("CUSTOM:", amountErrors[0].FormattedMessage);
        Assert.Contains("must contain only numeric characters", amountErrors[0].FormattedMessage);
    }

    [Fact]
    public void Validate_NumericWithSpaces_ReturnsInvalidResult()
    {
        // Arrange
        var model = new NumericModel
        {
            Amount = "123 456", // Invalid: contains space
            Percentage = "100",
            OptionalValue = "42"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.GetAllErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName == "Amount");
        Assert.Contains(result.GetAllErrors().Where(e => e.PropertyName == "Amount"), e => e.FormattedMessage.Contains("must contain only numeric characters"));
    }

    [Fact]
    public void Validate_LargeNumericValue_ReturnsValidResult()
    {
        // Arrange
        var model = new NumericModel
        {
            Amount = "999999999999999999999",
            Percentage = "123456789",
            OptionalValue = "987654321"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Validate_ScientificNotation_ReturnsValidResult()
    {
        // Arrange
        var model = new NumericModel
        {
            Amount = "1E10", // Scientific notation
            Percentage = "2e5",
            OptionalValue = "3.14E-2"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Validate_NullOptionalValue_ReturnsValidResult()
    {
        // Arrange
        var model = new NumericModel
        {
            Amount = "123.45",
            Percentage = "100",
            OptionalValue = null // Null optional value should be valid
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Validate_BothMainAndNestedInvalidNumericValues_ReturnsAllErrors()
    {
        // Arrange
        var model = new NumericNestedModel
        {
            MainValue = "not-numeric", // Invalid main value
            Details = new NumericModel
            {
                Amount = "also-not-numeric", // Invalid nested value
                Percentage = "100",
                OptionalValue = "invalid-optional" // Invalid nested optional value
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(3, result.GetAllErrors().Count());

        var mainValueErrors = result.GetAllErrors().Where(e => e.PropertyName == "MainValue").ToList();
        var amountErrors = result.GetAllErrors().Where(e => e.PropertyName == "Details.Amount").ToList();
        var optionalValueErrors = result.GetAllErrors().Where(e => e.PropertyName == "Details.OptionalValue").ToList();

        Assert.Single(mainValueErrors);
        Assert.Single(amountErrors);
        Assert.Single(optionalValueErrors);
        Assert.Contains("must contain only numeric characters", mainValueErrors[0].FormattedMessage);
        Assert.Contains("must contain only numeric characters", amountErrors[0].FormattedMessage);
        Assert.Contains("must contain only numeric characters", optionalValueErrors[0].FormattedMessage);
    }

    [Fact]
    public void Validate_NegativeNumbers_ReturnsValidResult()
    {
        // Arrange
        var model = new NumericModel
        {
            Amount = "-123.45",
            Percentage = "-100",
            OptionalValue = "-0.001"
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
