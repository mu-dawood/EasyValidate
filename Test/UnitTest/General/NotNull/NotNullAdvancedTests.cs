using EasyValidate.Abstractions;

using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.General.NotNull;

public class NotNullAdvancedTests
{
    [Fact]
    public void NestedModel_WithValidProperties_ShouldReturnValid()
    {
        // Arrange
        var model = new NotNullNestedModel
        {
            MainProperty = "Valid Main",
            Details = new NotNullModel
            {
                Name = "Valid Name",
                Data = new { Test = "data" },
                Items = new List<string> { "item1", "item2" }
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void NestedModel_WithNullMainProperty_ShouldReturnInvalid()
    {
        // Arrange
        var model = new NotNullNestedModel
        {
            MainProperty = null,
            Details = new NotNullModel
            {
                Name = "Valid Name",
                Data = new object(),
                Items = new List<string>()
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("MainProperty") && e.FormattedMessage.Contains("null"));
    }

    [Fact]
    public void NestedModel_WithNullNestedProperties_ShouldReturnMultipleErrors()
    {
        // Arrange
        var model = new NotNullNestedModel
        {
            MainProperty = "Valid Main",
            Details = new NotNullModel
            {
                Name = null,
                Data = null,
                Items = ["valid item"]
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(2, result.GetAllErrors().Count());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("Name") && e.FormattedMessage.Contains("null"));
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("Data") && e.FormattedMessage.Contains("null"));
    }

    [Fact]
    public void ComplexObject_AsData_ShouldBeValid()
    {
        // Arrange
        var complexObject = new
        {
            Id = 1,
            Description = "Complex object",
            SubObject = new { Value = "test" }
        };

        var model = new NotNullModel
        {
            Name = "Valid Name",
            Data = complexObject,
            Items = new List<string>()
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void DefaultValueTypes_ShouldBeValid()
    {
        // Test with value types that have default values but are not null
        var model = new NotNullModel
        {
            Name = "Valid Name",
            Data = 0, // int default value
            Items = new List<string>()
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Theory]
    [InlineData(true)]
    [InlineData(false)]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(3.14)]
    public void VariousValueTypes_ShouldBeValid(object value)
    {
        // Arrange
        var model = new NotNullModel
        {
            Name = "Valid Name",
            Data = value,
            Items = new List<string>()
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void CustomFormatter_WithNotNullErrors_ShouldFormatCorrectly()
    {
        // Arrange
        var model = new NotNullModel
        {
            Name = null,
            Data = new object(),
            Items = null
        };
        var formatter = new TestFormatter();

        // Act
        var result = model.Validate((conf) => conf.SetFormatter(formatter));

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        var formattedMessage = result.ToString();
        Assert.Contains("Error:", formattedMessage);
    }

    public class TestFormatter : EasyValidate.Abstractions.IFormatter
    {
        public string Format<T>(AttributeResult result, T value)
        {
            return string.Join(" | ", result.MessageTemplate.Select(e => $"Error: {e}"));
        }

        [Fact]
        public void MixedNullAndValidProperties_ShouldOnlyReportNullErrors()
        {
            // Arrange
            var model = new NotNullModel
            {
                Name = "Valid Name", // Valid
                Data = null,        // Invalid - null
                Items = new List<string> { "valid" } // Valid
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.Single(result.GetAllErrors());
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("Data") && e.FormattedMessage.Contains("null"));
        }
    }
}
