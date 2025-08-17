using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.General.NotNull;

public class NotNullSimpleTests
{
    [Fact]
    public void ValidNotNullProperties_ShouldReturnValid()
    {
        // Arrange
        var model = new NotNullModel
        {
            Name = "Test Name",
            Data = new object(),
            Items = new List<string> { "item1" }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void NullName_ShouldReturnInvalid()
    {
        // Arrange
        var model = new NotNullModel
        {
            Name = null,
            Data = new object(),
            Items = new List<string>()
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("Name") && e.FormattedMessage.Contains("null"));
    }

    [Fact]
    public void NullData_ShouldReturnInvalid()
    {
        // Arrange
        var model = new NotNullModel
        {
            Name = "Valid Name",
            Data = null,
            Items = new List<string>()
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("Data") && e.FormattedMessage.Contains("null"));
    }

    [Fact]
    public void NullItems_ShouldReturnInvalid()
    {
        // Arrange
        var model = new NotNullModel
        {
            Name = "Valid Name",
            Data = new object(),
            Items = null
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("Items") && e.FormattedMessage.Contains("null"));
    }

    [Fact]
    public void AllNullProperties_ShouldReturnMultipleErrors()
    {
        // Arrange
        var model = new NotNullModel
        {
            Name = null,
            Data = null,
            Items = null
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(3, result.GetAllErrors().Count());
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("Valid String")]
    public void EmptyOrWhitespaceStrings_ShouldBeValid(string value)
    {
        // Arrange
        var model = new NotNullModel
        {
            Name = value,
            Data = new object(),
            Items = new List<string>()
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void EmptyList_ShouldBeValid()
    {
        // Arrange
        var model = new NotNullModel
        {
            Name = "Valid Name",
            Data = new object(),
            Items = new List<string>()
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }
}
