using EasyValidate.Abstractions;

using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Contains;

public class ContainsAdvancedTests
{
    [Fact]
    public void NestedModel_WithValidContains_ShouldReturnValid()
    {
        // Arrange
        var model = new ContainsNestedModel
        {
            MainProperty = "This contains main text",
            Details = new ContainsModel
            {
                BasicContains = "This contains test",
                CaseSensitive = "Contains CASE",
                CaseInsensitive = "Contains REQUIRED",
                EmailContains = "user@domain.com",
                WhitespaceContains = "Has spaces"
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void NestedModel_WithInvalidMainProperty_ShouldReturnInvalid()
    {
        // Arrange
        var model = new ContainsNestedModel
        {
            MainProperty = "This does not contain the required word",
            Details = new ContainsModel
            {
                BasicContains = "Valid test"
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName == nameof(model.MainProperty));
    }

    [Theory]
    [InlineData("REQUIRED text", true)]
    [InlineData("required text", true)]
    [InlineData("ReQuIrEd text", true)]
    public void CaseInsensitiveFlag_ShouldWorkCorrectly(string input, bool expectedValid)
    {
        // Arrange
        var model = new ContainsModel
        {
            CaseInsensitive = input
        };

        // Act
        var result = model.Validate();

        // Assert
        if (expectedValid)
        {
            Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName == nameof(model.CaseInsensitive)));
        }
        else
        {
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName == nameof(model.CaseInsensitive));
        }
    }

    [Fact]
    public void SpecialCharacters_ShouldBeHandledCorrectly()
    {
        // Arrange
        var model = new ContainsModel
        {
            BasicContains = "Special chars: test!@#$%",
            CaseSensitive = "Contains CASE with symbols!",
            CaseInsensitive = "Has required + symbols",
            EmailContains = "email@domain.com",
            WhitespaceContains = "Multiple   spaces   here"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void Unicode_ShouldBeHandledCorrectly()
    {
        // Arrange
        var model = new ContainsModel
        {
            BasicContains = "Unicode test: café, naïve, résumé",
            CaseSensitive = "CASE with émoji 🎉",
            CaseInsensitive = "required with special chars áéíóú",
            EmailContains = "user@domain.com",
            WhitespaceContains = "Space test here"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void VeryLongStrings_ShouldWork()
    {
        // Arrange
        var longString = new string('a', 1000) + "test" + new string('b', 1000);
        var model = new ContainsModel
        {
            BasicContains = longString
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void EdgeCase_SearchTextAtBeginning_ShouldWork()
    {
        // Arrange
        var model = new ContainsModel
        {
            BasicContains = "test is at the beginning"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void EdgeCase_SearchTextAtEnd_ShouldWork()
    {
        // Arrange
        var model = new ContainsModel
        {
            BasicContains = "This ends with test"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void CustomFormatter_WithContainsErrors_ShouldFormatCorrectly()
    {
        // Arrange
        var model = new ContainsModel
        {
            BasicContains = "no match here",
            CaseSensitive = "wrong case"
        };

        // Use a custom formatter implementing IFormatter
        var formatter = new TestFormatter();

        // Act
        var result = model.Validate((conf) => conf.SetFormatter(formatter));

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        var formattedMessage = result.ToString();
        Assert.Contains("CUSTOM:", formattedMessage);
    }

    [Theory]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void WhitespaceSearchText_ShouldWorkCorrectly(string whitespace)
    {
        // Arrange
        var model = new ContainsModel
        {
            WhitespaceContains = $"Text{whitespace}with{whitespace}whitespace"
        };

        // Act
        var result = model.Validate();

        // Assert
        // Should be valid if the string contains the specific whitespace character
        if (whitespace == " ")
        {
            Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName == nameof(model.WhitespaceContains)));
        }
    }

    // Helper test formatter for CustomFormatter_WithContainsErrors_ShouldFormatCorrectly
    private class TestFormatter : IFormatter
    {
        public string Format(string messageTemplate, object[] args)
        {
            return $"CUSTOM: {string.Format(messageTemplate, args)}";
        }
    }
}
