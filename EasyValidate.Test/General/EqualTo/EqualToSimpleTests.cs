using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.General.EqualTo;

public class EqualToSimpleTests
{
    [Fact]
    public void ValidEqualValues_ShouldReturnValid()
    {
        // Arrange
        var model = new EqualToModel
        {
            StringProperty = "ExpectedValue",
            IntProperty = 42,
            DoubleProperty = 3.14,
            BoolProperty = true,
            NullableProperty = null
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void StringProperty_WithWrongValue_ShouldReturnInvalid()
    {
        // Arrange
        var model = new EqualToModel
        {
            StringProperty = "WrongValue",
            IntProperty = 42,
            DoubleProperty = 3.14,
            BoolProperty = true,
            NullableProperty = null
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("StringProperty"));
    }

    [Fact]
    public void IntProperty_WithWrongValue_ShouldReturnInvalid()
    {
        // Arrange
        var model = new EqualToModel
        {
            StringProperty = "ExpectedValue",
            IntProperty = 99,
            DoubleProperty = 3.14,
            BoolProperty = true,
            NullableProperty = null
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("IntProperty"));
    }

    [Fact]
    public void DoubleProperty_WithWrongValue_ShouldReturnInvalid()
    {
        // Arrange
        var model = new EqualToModel
        {
            StringProperty = "ExpectedValue",
            IntProperty = 42,
            DoubleProperty = 2.71,
            BoolProperty = true,
            NullableProperty = null
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("DoubleProperty"));
    }

    [Fact]
    public void BoolProperty_WithWrongValue_ShouldReturnInvalid()
    {
        // Arrange
        var model = new EqualToModel
        {
            StringProperty = "ExpectedValue",
            IntProperty = 42,
            DoubleProperty = 3.14,
            BoolProperty = false,
            NullableProperty = null
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("BoolProperty"));
    }

    [Fact]
    public void NullableProperty_WithNonNullValue_ShouldReturnInvalid()
    {
        // Arrange
        var model = new EqualToModel
        {
            StringProperty = "ExpectedValue",
            IntProperty = 42,
            DoubleProperty = 3.14,
            BoolProperty = true,
            NullableProperty = "SomeValue"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("NullableProperty"));
    }

    [Theory]
    [InlineData("ExpectedValue")]
    public void StringProperty_WithExactMatch_ShouldReturnValid(string value)
    {
        // Arrange
        var model = new EqualToModel
        {
            StringProperty = value
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("StringProperty")));
    }

    [Theory]
    [InlineData("expectedvalue")]  // Case different
    [InlineData("EXPECTEDVALUE")]  // Case different
    [InlineData(" ExpectedValue")] // Leading space
    [InlineData("ExpectedValue ")] // Trailing space
    [InlineData("")]
    [InlineData(null)]
    public void StringProperty_WithNonExactMatch_ShouldReturnInvalid(string? value)
    {
        // Arrange
        var model = new EqualToModel
        {
            StringProperty = value
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("StringProperty"));
    }

    [Theory]
    [InlineData(42)]
    public void IntProperty_WithExactMatch_ShouldReturnValid(int value)
    {
        // Arrange
        var model = new EqualToModel
        {
            IntProperty = value
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("IntProperty")));
    }

    [Theory]
    [InlineData(0)]
    [InlineData(41)]
    [InlineData(43)]
    [InlineData(-42)]
    [InlineData(int.MaxValue)]
    [InlineData(int.MinValue)]
    public void IntProperty_WithNonExactMatch_ShouldReturnInvalid(int value)
    {
        // Arrange
        var model = new EqualToModel
        {
            IntProperty = value
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("IntProperty"));
    }

    [Fact]
    public void MultipleWrongValues_ShouldReturnMultipleErrors()
    {
        // Arrange
        var model = new EqualToModel
        {
            StringProperty = "Wrong",
            IntProperty = 0,
            DoubleProperty = 0.0,
            BoolProperty = false,
            NullableProperty = "NotNull"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(5, result.GetAllErrors().Count());
    }

    [Fact]
    public void DoubleProperty_WithPrecisionIssues_ShouldBeHandledCorrectly()
    {
        // Arrange
        var model = new EqualToModel
        {
            DoubleProperty = 3.14000000001 // Very close but not exactly 3.14
        };

        // Act
        var result = model.Validate();

        // Assert
        // This test depends on the exact implementation of double comparison
        // Most implementations should consider this as not equal
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("DoubleProperty"));
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    public void BoolProperty_ComparedAgainstTrue_ShouldWorkCorrectly(bool value)
    {
        // Arrange
        var model = new EqualToModel
        {
            BoolProperty = value
        };

        // Act
        var result = model.Validate();

        // Assert
        if (value == true)
        {
            Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("BoolProperty")));
        }
        else
        {
            Assert.False(result.IsValid());
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("BoolProperty"));
        }
    }
}
