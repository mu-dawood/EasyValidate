using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Numerics.Range;

public class RangeSimpleTests
{
    [Fact]
    public void Range_ValidValuesWithinRange_ReturnsValidResult()
    {
        // Arrange
        var model = new RangeModel
        {
            Age = 25, // Within 1-100
            Rating = 4.5m, // Within 0.0-5.0
            Temperature = 5.0, // Within -10 to 10
            Score = 85.5f // Within 0-1000
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Range_ValueBelowMinimum_ReturnsInvalidResult()
    {
        // Arrange
        var model = new RangeModel
        {
            Age = 0, // Below minimum (1)
            Rating = 3.0m,
            Temperature = 0.0,
            Score = 500.0f
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Age)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Rating)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Temperature)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Score)));
        var ageErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Age))).ToList();
        Assert.Single(ageErrors);
        Assert.Contains("must be within", ageErrors[0].FormattedMessage);
    }

    [Fact]
    public void Range_ValueAboveMaximum_ReturnsInvalidResult()
    {
        // Arrange
        var model = new RangeModel
        {
            Age = 101, // Above maximum (100)
            Rating = 3.0m,
            Temperature = 0.0,
            Score = 500.0f
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Age)));
        var ageErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Age))).ToList();
        Assert.Single(ageErrors);
        Assert.Contains("must be within", ageErrors[0].FormattedMessage);
    }

    [Fact]
    public void Range_BoundaryValues_ReturnsValidResult()
    {
        // Arrange
        var model = new RangeModel
        {
            Age = 1, // Minimum boundary
            Rating = 0.0m, // Minimum boundary
            Temperature = -10.0, // Minimum boundary
            Score = 1000.0f // Maximum boundary
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Range_MaximumBoundaryValues_ReturnsValidResult()
    {
        // Arrange
        var model = new RangeModel
        {
            Age = 100, // Maximum boundary
            Rating = 5.0m, // Maximum boundary
            Temperature = 10.0, // Maximum boundary
            Score = 0.0f // Minimum boundary
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Range_NullableValue_ReturnsValidResult()
    {
        // Arrange
        var model = new RangeModel
        {
            Age = 25,
            Rating = null, // Null should be valid for nullable
            Temperature = 0.0,
            Score = 500.0f
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
    [InlineData(25)]
    [InlineData(50)]
    [InlineData(75)]
    [InlineData(100)]
    public void Range_ValidAgeValues_ReturnsValidResult(int age)
    {
        // Arrange
        var model = new RangeModel
        {
            Age = age,
            Rating = 3.0m,
            Temperature = 0.0,
            Score = 500.0f
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
    [InlineData(0)]
    [InlineData(101)]
    [InlineData(999)]
    public void Range_InvalidAgeValues_ReturnsInvalidResult(int age)
    {
        // Arrange
        var model = new RangeModel
        {
            Age = age,
            Rating = 3.0m,
            Temperature = 0.0,
            Score = 500.0f
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Age)));
        var ageErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Age))).ToList();
        Assert.Single(ageErrors);
        Assert.Contains("must be within", ageErrors[0].FormattedMessage);
    }

    [Fact]
    public void Range_MultipleInvalidValues_ReturnsMultipleErrors()
    {
        // Arrange
        var model = new RangeModel
        {
            Age = 0, // Invalid: below minimum
            Rating = 6.0m, // Invalid: above maximum
            Temperature = -15.0, // Invalid: below minimum
            Score = 1500.0f // Invalid: above maximum
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(4, result.GetAllErrors().Count());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Age)));
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Rating)));
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Temperature)));
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Score)));
    }
}
