using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Contains;

public class ContainsSimpleTests
{
    [Fact]
    public void ValidContainsString_ShouldReturnValid()
    {
        // Arrange
        var model = new ContainsModel
        {
            BasicContains = "This is a test string",
            CaseSensitive = "This contains CASE",
            CaseInsensitive = "This contains REQUIRED text",
            EmailContains = "user@domain.com",
            WhitespaceContains = "Word with spaces"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void BasicContains_WithoutRequiredText_ShouldReturnInvalid()
    {
        // Arrange
        var model = new ContainsModel
        {
            BasicContains = "This does not contain the required word",
            CaseSensitive = "Valid CASE",
            CaseInsensitive = "valid required text",
            EmailContains = "user@domain.com",
            WhitespaceContains = "Word with spaces"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("BasicContains"));
    }

    [Fact]
    public void CaseSensitive_WithWrongCase_ShouldReturnInvalid()
    {
        // Arrange
        var model = new ContainsModel
        {
            BasicContains = "This is a test string",
            CaseSensitive = "This contains case in lowercase",
            CaseInsensitive = "valid required text",
            EmailContains = "user@domain.com",
            WhitespaceContains = "Word with spaces"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("CaseSensitive"));
    }

    [Fact]
    public void CaseInsensitive_WithDifferentCase_ShouldReturnValid()
    {
        // Arrange
        var model = new ContainsModel
        {
            BasicContains = "This is a test string",
            CaseSensitive = "Valid CASE",
            CaseInsensitive = "This contains REQUIRED in uppercase",
            EmailContains = "user@domain.com",
            WhitespaceContains = "Word with spaces"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Theory]
    [InlineData("test")]
    [InlineData("testing")]
    [InlineData("This test works")]
    [InlineData("pretest")]
    [InlineData("testpost")]
    public void BasicContains_WithVariousValidInputs_ShouldReturnValid(string input)
    {
        // Arrange
        var model = new ContainsModel
        {
            BasicContains = input,
            CaseSensitive = "Valid CASE",
            CaseInsensitive = "valid required text",
            EmailContains = "user@domain.com",
            WhitespaceContains = "Word with spaces"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
    }

    [Theory]
    [InlineData("TEST")]
    [InlineData("Test")]
    [InlineData("no match")]
    [InlineData("tes")]
    [InlineData("est")]
    public void BasicContains_WithInvalidInputs_ShouldReturnInvalid(string input)
    {
        // Arrange
        var model = new ContainsModel
        {
            BasicContains = input
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("BasicContains"));
    }

    [Fact]
    public void NullString_ShouldReturnInvalid()
    {
        // Arrange
        var model = new ContainsModel
        {
            BasicContains = null
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
    }

    [Fact]
    public void EmptyString_ShouldReturnInvalid()
    {
        // Arrange
        var model = new ContainsModel
        {
            BasicContains = ""
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
    }

    [Fact]
    public void MultipleFailures_ShouldReturnMultipleErrors()
    {
        // Arrange
        var model = new ContainsModel
        {
            BasicContains = "no match",
            CaseSensitive = "wrong case",
            CaseInsensitive = "valid required text",
            EmailContains = "no at symbol",
            WhitespaceContains = "NoSpaces"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.True(result.GetAllErrors().Count() >= 3);
    }
}
