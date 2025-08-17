using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Numerics.EvenNumber;

public class EvenNumberSimpleTests
{
    [Fact]
    public void ValidEvenNumbers_ShouldReturnValid()
    {
        // Arrange
        var model = new EvenNumberModel
        {
            IntValue = 2,
            LongValue = 100L,
            ShortValue = 4,
            ByteValue = 6,
            NullableInt = 8
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void OddIntValue_ShouldReturnInvalid()
    {
        // Arrange
        var model = new EvenNumberModel
        {
            IntValue = 3,
            LongValue = 2L,
            ShortValue = 4,
            ByteValue = 6,
            NullableInt = 8
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("IntValue"));
    }

    [Fact]
    public void Zero_ShouldReturnValid()
    {
        // Arrange
        var model = new EvenNumberModel
        {
            IntValue = 0,
            LongValue = 0L,
            ShortValue = 0,
            ByteValue = 0,
            NullableInt = 0
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Theory]
    [InlineData(0)]
    [InlineData(2)]
    [InlineData(4)]
    [InlineData(100)]
    [InlineData(1000)]
    [InlineData(-2)]
    [InlineData(-100)]
    public void IntValue_WithEvenNumbers_ShouldReturnValid(int value)
    {
        // Arrange
        var model = new EvenNumberModel
        {
            IntValue = value
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("IntValue")));
    }

    [Theory]
    [InlineData(1)]
    [InlineData(3)]
    [InlineData(5)]
    [InlineData(99)]
    [InlineData(1001)]
    [InlineData(-1)]
    [InlineData(-99)]
    public void IntValue_WithOddNumbers_ShouldReturnInvalid(int value)
    {
        // Arrange
        var model = new EvenNumberModel
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
    [InlineData(0L)]
    [InlineData(2L)]
    [InlineData(1000L)]
    [InlineData(-2L)]
    [InlineData(long.MaxValue - 1)] // Ensure it's even
    public void LongValue_WithEvenNumbers_ShouldReturnValid(long value)
    {
        // Arrange
        var model = new EvenNumberModel
        {
            LongValue = value
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("LongValue")));
    }

    [Theory]
    [InlineData(1L)]
    [InlineData(3L)]
    [InlineData(1001L)]
    [InlineData(-1L)]
    [InlineData(long.MaxValue)] // This should be odd
    public void LongValue_WithOddNumbers_ShouldReturnInvalid(long value)
    {
        // Arrange
        var model = new EvenNumberModel
        {
            LongValue = value
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("LongValue"));
    }

    [Fact]
    public void NullableInt_WithNull_ShouldReturnValid()
    {
        // Arrange
        var model = new EvenNumberModel
        {
            IntValue = 2,
            LongValue = 4L,
            ShortValue = 6,
            ByteValue = 8,
            NullableInt = null
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void NullableInt_WithOddValue_ShouldReturnInvalid()
    {
        // Arrange
        var model = new EvenNumberModel
        {
            IntValue = 2,
            LongValue = 4L,
            ShortValue = 6,
            ByteValue = 8,
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
    public void NullableInt_WithEvenValue_ShouldReturnValid()
    {
        // Arrange
        var model = new EvenNumberModel
        {
            IntValue = 2,
            LongValue = 4L,
            ShortValue = 6,
            ByteValue = 8,
            NullableInt = 10
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void AllOddValues_ShouldReturnMultipleErrors()
    {
        // Arrange
        var model = new EvenNumberModel
        {
            IntValue = 1,
            LongValue = 3L,
            ShortValue = 5,
            ByteValue = 7,
            NullableInt = 9
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(5, result.GetAllErrors().Count());
    }

    [Theory]
    [InlineData((short)0)]
    [InlineData((short)2)]
    [InlineData((short)4)]
    [InlineData((short)100)]
    [InlineData((short)-2)]
    public void ShortValue_WithEvenNumbers_ShouldReturnValid(short value)
    {
        // Arrange
        var model = new EvenNumberModel
        {
            ShortValue = value
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("ShortValue")));
    }

    [Theory]
    [InlineData((byte)0)]
    [InlineData((byte)2)]
    [InlineData((byte)4)]
    [InlineData((byte)100)]
    [InlineData((byte)254)]
    public void ByteValue_WithEvenNumbers_ShouldReturnValid(byte value)
    {
        // Arrange
        var model = new EvenNumberModel
        {
            ByteValue = value
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("ByteValue")));
    }

    [Theory]
    [InlineData((byte)1)]
    [InlineData((byte)3)]
    [InlineData((byte)5)]
    [InlineData((byte)99)]
    [InlineData((byte)255)]
    public void ByteValue_WithOddNumbers_ShouldReturnInvalid(byte value)
    {
        // Arrange
        var model = new EvenNumberModel
        {
            ByteValue = value
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("ByteValue"));
    }

    [Fact]
    public void NegativeEvenNumbers_ShouldReturnValid()
    {
        // Arrange
        var model = new EvenNumberModel
        {
            IntValue = -2,
            LongValue = -100L,
            ShortValue = -4,
            NullableInt = -6
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("IntValue") || e.PropertyName.Contains("LongValue") || e.PropertyName.Contains("ShortValue") || e.PropertyName.Contains("NullableInt")));
    }

    [Fact]
    public void NegativeOddNumbers_ShouldReturnInvalid()
    {
        // Arrange
        var model = new EvenNumberModel
        {
            IntValue = -1,
            LongValue = -99L,
            ShortValue = -3,
            NullableInt = -5
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.True(result.GetAllErrors().Count() >= 4);
    }

    [Fact]
    public void LargeEvenNumbers_ShouldReturnValid()
    {
        // Arrange
        var model = new EvenNumberModel
        {
            IntValue = int.MaxValue - 1, // Make sure it's even
            LongValue = long.MaxValue - 1, // Make sure it's even
            ShortValue = short.MaxValue - 1, // Make sure it's even
            ByteValue = byte.MaxValue - 1 // Make sure it's even (254)
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }
}
