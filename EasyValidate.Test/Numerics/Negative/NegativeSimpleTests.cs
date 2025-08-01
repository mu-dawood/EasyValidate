using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Numerics.Negative;

public class NegativeSimpleTests
{
    [Fact]
    public void ValidNegativeNumbers_ShouldReturnValid()
    {
        // Arrange
        var model = new NegativeModel
        {
            IntValue = -1,
            DoubleValue = -3.14,
            DecimalValue = -100.50m,
            FloatValue = -2.5f,
            LongValue = -1000L,
            NullableInt = -5
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void PositiveIntValue_ShouldReturnInvalid()
    {
        // Arrange
        var model = new NegativeModel
        {
            IntValue = 1,
            DoubleValue = -1.0,
            DecimalValue = -1m,
            FloatValue = -1f,
            LongValue = -1L
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("IntValue"));
    }

    [Fact]
    public void ZeroValue_ShouldReturnInvalid()
    {
        // Arrange
        var model = new NegativeModel
        {
            IntValue = 0,
            DoubleValue = -1.0,
            DecimalValue = -1m,
            FloatValue = -1f,
            LongValue = -1L
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("IntValue"));
    }

    [Theory]
    [InlineData(-1)]
    [InlineData(-100)]
    [InlineData(-999)]
    [InlineData(int.MinValue)]
    public void IntValue_WithVariousNegativeNumbers_ShouldReturnValid(int value)
    {
        // Arrange
        var model = new NegativeModel
        {
            IntValue = value
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains("IntValue"));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(1)]
    [InlineData(100)]
    [InlineData(int.MaxValue)]
    public void IntValue_WithNonNegativeNumbers_ShouldReturnInvalid(int value)
    {
        // Arrange
        var model = new NegativeModel
        {
            IntValue = value
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("IntValue"));
    }

    [Theory]
    [InlineData(-1.0)]
    [InlineData(-0.1)]
    [InlineData(-999.999)]
    [InlineData(double.MinValue)]
    public void DoubleValue_WithNegativeNumbers_ShouldReturnValid(double value)
    {
        // Arrange
        var model = new NegativeModel
        {
            DoubleValue = value
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains("DoubleValue"));
    }

    [Theory]
    [InlineData(0.0)]
    [InlineData(0.1)]
    [InlineData(1.0)]
    [InlineData(999.999)]
    public void DoubleValue_WithNonNegativeNumbers_ShouldReturnInvalid(double value)
    {
        // Arrange
        var model = new NegativeModel
        {
            DoubleValue = value
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("DoubleValue"));
    }

    [Fact]
    public void NullableInt_WithNull_ShouldReturnValid()
    {
        // Arrange
        var model = new NegativeModel
        {
            IntValue = -1,
            DoubleValue = -1.0,
            DecimalValue = -1m,
            FloatValue = -1f,
            LongValue = -1L,
            NullableInt = null
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void NullableInt_WithPositiveValue_ShouldReturnInvalid()
    {
        // Arrange
        var model = new NegativeModel
        {
            IntValue = -1,
            DoubleValue = -1.0,
            DecimalValue = -1m,
            FloatValue = -1f,
            LongValue = -1L,
            NullableInt = 5
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("NullableInt"));
    }

    [Fact]
    public void MultiplePositiveValues_ShouldReturnMultipleErrors()
    {
        // Arrange
        var model = new NegativeModel
        {
            IntValue = 1,
            DoubleValue = 2.5,
            DecimalValue = 100m,
            FloatValue = 1.5f,
            LongValue = 50L,
            NullableInt = 10
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(6, result.GetAllErrors().Count());
    }

    [Fact]
    public void DecimalPrecision_ShouldBeHandledCorrectly()
    {
        // Arrange
        var model = new NegativeModel
        {
            DecimalValue = -0.0000000001m
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains("DecimalValue"));
    }

    [Fact]
    public void FloatMinValue_ShouldBeValid()
    {
        // Arrange
        var model = new NegativeModel
        {
            FloatValue = float.MinValue
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains("FloatValue"));
    }

    [Fact]
    public void LongMinValue_ShouldBeValid()
    {
        // Arrange
        var model = new NegativeModel
        {
            LongValue = long.MinValue
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains("LongValue"));
    }
}
