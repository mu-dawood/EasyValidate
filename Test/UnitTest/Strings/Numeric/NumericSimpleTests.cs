using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Numeric;

public class NumericSimpleTests
{
    [Fact]
    public void Numeric_ValidNumbers_ReturnsValidResult()
    {
        // Arrange
        var model = new NumericModel
        {
            Amount = "123",
            Percentage = "99.5",
            OptionalValue = "42"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Numeric_InvalidNonNumericString_ReturnsInvalidResult()
    {
        // Arrange
        var model = new NumericModel
        {
            Amount = "abc", // Invalid non-numeric string
            Percentage = "99.5",
            OptionalValue = "42"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Amount)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Percentage)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalValue)));
        var amountErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Amount))).ToList();
        Assert.Single(amountErrors);
        Assert.Contains("must contain only numeric characters", amountErrors[0].FormattedMessage);
    }

    [Fact]
    public void Numeric_EmptyString_ReturnsInvalidResult()
    {
        // Arrange
        var model = new NumericModel
        {
            Amount = "", // Empty string should fail Numeric validation
            Percentage = "99.5",
            OptionalValue = "42"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Amount)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Percentage)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalValue)));
        var amountErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Amount))).ToList();
        Assert.Single(amountErrors);
        Assert.Contains("must contain only numeric characters", amountErrors[0].FormattedMessage);
    }

    [Fact]
    public void Numeric_MultipleInvalidValues_ReturnsMultipleErrors()
    {
        // Arrange
        var model = new NumericModel
        {
            Amount = "abc", // Invalid
            Percentage = "xyz", // Invalid
            OptionalValue = "def" // Invalid
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(3, result.GetAllErrors().Count());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Amount)));
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Percentage)));
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalValue)));
    }

    [Theory]
    [InlineData("123")]
    [InlineData("0")]
    [InlineData("999")]
    [InlineData("12345")]
    public void Numeric_ValidNumericStrings_ReturnsValidResult(string value)
    {
        // Arrange
        var model = new NumericModel
        {
            Amount = value,
            Percentage = "50",
            OptionalValue = "100"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Theory]
    [InlineData("abc")]
    [InlineData("12a")]
    [InlineData("a12")]
    [InlineData("1a2")]
    [InlineData("!@#")]
    public void Numeric_InvalidNonNumericStrings_ReturnsInvalidResult(string value)
    {
        // Arrange
        var model = new NumericModel
        {
            Amount = value,
            Percentage = "50",
            OptionalValue = "100"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Amount)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Percentage)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalValue)));
        var amountErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Amount))).ToList();
        Assert.Single(amountErrors);
        Assert.Contains("must contain only numeric characters", amountErrors[0].FormattedMessage);
    }
}
