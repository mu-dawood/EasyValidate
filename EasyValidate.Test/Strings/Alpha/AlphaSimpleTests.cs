using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Alpha;

public class AlphaSimpleTests
{
    [Fact]
    public void Alpha_ValidString_ReturnsValidResult()
    {
        // Arrange
        var model = new AlphaModel
        {
            FirstName = "John",
            LastName = "Smith",
            MiddleName = "William"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.FirstName)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.LastName)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.MiddleName)));
    }

    [Fact]
    public void Alpha_InvalidStringWithNumbers_ReturnsInvalidResult()
    {
        // Arrange
        var model = new AlphaModel
        {
            FirstName = "John123",
            LastName = "Smith",
            MiddleName = "William"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.FirstName)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.LastName)));
        Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.MiddleName)));
        var firstNameErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.FirstName))).ToList();
        Assert.Single(firstNameErrors);
        Assert.Equal("AlphaValidationError", firstNameErrors[0].ErrorCode);
        Assert.Equal("The FirstName field must contain only alphabetic characters.", firstNameErrors[0].FormattedMessage);
    }

    [Fact]
    public void Alpha_InvalidStringWithSpecialCharacters_ReturnsInvalidResult()
    {
        // Arrange
        var model = new AlphaModel
        {
            FirstName = "John-Doe",
            LastName = "O'Connor",
            MiddleName = "Jr."
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.FirstName)));
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.LastName)));
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.MiddleName)));
        Assert.Equal(3, result.GetAllErrors().Count());
    }

    [Fact]
    public void Alpha_EmptyString_ReturnsInvalidResult()
    {
        // Arrange
        var model = new AlphaModel
        {
            FirstName = "",
            LastName = "Smith",
            MiddleName = "William"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.FirstName)));
        var firstNameErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.FirstName))).ToList();
        Assert.Single(firstNameErrors);
        Assert.Equal("AlphaValidationError", firstNameErrors[0].ErrorCode);
    }

    [Fact]
    public void Alpha_WhitespaceString_ReturnsInvalidResult()
    {
        // Arrange
        var model = new AlphaModel
        {
            FirstName = "   ",
            LastName = "Smith",
            MiddleName = "William"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.FirstName)));
    }

    [Fact]
    public void Alpha_NullString_ReturnsValidResult()
    {
        // Arrange
        var model = new AlphaModel
        {
            FirstName = "John",
            LastName = null,
            MiddleName = "William"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Theory]
    [InlineData("JOHN")]
    [InlineData("john")]
    [InlineData("John")]
    [InlineData("JoHn")]
    [InlineData("A")]
    [InlineData("z")]
    public void Alpha_ValidVariations_ReturnsValidResult(string value)
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

    [Theory]
    [InlineData("John123")]
    [InlineData("John-Doe")]
    [InlineData("John_Doe")]
    [InlineData("John Doe")]
    [InlineData("John@email")]
    [InlineData("John!")]
    [InlineData("John?")]
    [InlineData("John.")]
    [InlineData("John,")]
    [InlineData("123")]
    [InlineData("@#$")]
    public void Alpha_InvalidVariations_ReturnsInvalidResult(string value)
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
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.FirstName)));
    }
}
