using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Numerics.Positive;

public class PositiveSimpleTests
{
    [Fact]
    public void Positive_ValidPositiveNumbers_ReturnsValidResult()
    {
        // Arrange
        var model = new PositiveModel
        {
            Count = 5,
            Price = 29.99m,
            Rating = 4.5,
            Score = 85.7f
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Positive_InvalidZeroValue_ReturnsInvalidResult()
    {
        // Arrange
        var model = new PositiveModel
        {
            Count = 0, // Invalid: zero is not positive
            Price = 29.99m,
            Rating = 4.5,
            Score = 85.7f
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Count)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Price)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Rating)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Score)));
        var countErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Count))).ToList();
        Assert.Single(countErrors);
        Assert.Contains("must be a positive", countErrors[0].FormattedMessage);
    }

    [Fact]
    public void Positive_InvalidNegativeValue_ReturnsInvalidResult()
    {
        // Arrange
        var model = new PositiveModel
        {
            Count = -5, // Invalid: negative number
            Price = 29.99m,
            Rating = 4.5,
            Score = 85.7f
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Count)));
        var countErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Count))).ToList();
        Assert.Single(countErrors);
        Assert.Contains("must be a positive", countErrors[0].FormattedMessage);
    }

    [Fact]
    public void Positive_NullablePositiveValue_ReturnsValidResult()
    {
        // Arrange
        var model = new PositiveModel
        {
            Count = 5,
            Price = null, // Null should be valid for nullable
            Rating = 4.5,
            Score = 85.7f
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Theory]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(999999)]
    [InlineData(int.MaxValue)]
    public void Positive_ValidPositiveIntegers_ReturnsValidResult(int value)
    {
        // Arrange
        var model = new PositiveModel
        {
            Count = value,
            Price = 10.0m,
            Rating = 1.0,
            Score = 1.0f
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(-999999)]
    [InlineData(int.MinValue)]
    [InlineData(0)]
    public void Positive_InvalidNonPositiveIntegers_ReturnsInvalidResult(int value)
    {
        // Arrange
        var model = new PositiveModel
        {
            Count = value,
            Price = 10.0m,
            Rating = 1.0,
            Score = 1.0f
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Count)));
        var countErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Count))).ToList();
        Assert.Single(countErrors);
        Assert.Contains("must be a positive", countErrors[0].FormattedMessage);
    }

    [Fact]
    public void Positive_MultipleInvalidValues_ReturnsMultipleErrors()
    {
        // Arrange
        var model = new PositiveModel
        {
            Count = -1, // Invalid
            Price = 10.0m,
            Rating = -2.5, // Invalid
            Score = 0.0f // Invalid
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(3, result.GetAllErrors().Count());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Count)));
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Rating)));
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Score)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Price)));
    }
}
