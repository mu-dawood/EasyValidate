using EasyValidate.Abstractions;

using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.AlphaNumeric;

public class AlphaNumericAdvancedTests
{
    [Fact]
    public void Validate_NestedObjectWithValidAlphaNumericValues_ReturnsValidResult()
    {
        // Arrange
        var model = new AlphaNumericNestedModel
        {
            MainCode = "ABC123",
            Details = new AlphaNumericModel
            {
                Username = "user123",
                ProductCode = "PROD456",
                OptionalCode = "OPT789"
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Validate_NestedObjectWithInvalidAlphaNumericValues_ReturnsInvalidResultWithPrefixedErrors()
    {
        // Arrange
        var model = new AlphaNumericNestedModel
        {
            MainCode = "ABC123",
            Details = new AlphaNumericModel
            {
                Username = "user@123", // Invalid: contains @
                ProductCode = "PROD456",
                OptionalCode = "OPT-789" // Invalid: contains -
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(2, result.GetAllErrors().Count());

        var detailsUsernameErrors = result.GetAllErrors().Where(e => e.PropertyName == "Details.Username").ToList();
        var detailsOptionalCodeErrors = result.GetAllErrors().Where(e => e.PropertyName == "Details.OptionalCode").ToList();

        Assert.Single(detailsUsernameErrors);
        Assert.Single(detailsOptionalCodeErrors);
        Assert.Contains("must contain only alphanumeric characters", detailsUsernameErrors[0].FormattedMessage);
        Assert.Contains("must contain only alphanumeric characters", detailsOptionalCodeErrors[0].FormattedMessage);
    }

    [Fact]
    public void Validate_NullNestedObject_ReturnsValidResult()
    {
        // Arrange
        var model = new AlphaNumericNestedModel
        {
            MainCode = "ABC123",
            Details = null
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Validate_WithCustomFormatter_FormatsErrorsCorrectly()
    {
        // Arrange
        var model = new AlphaNumericModel
        {
            Username = "user@123",
            ProductCode = "PROD456",
            OptionalCode = "OPT789"
        };
        var formatter = new CustomTestFormatter();

        // Act
        var result = model.Validate((conf) => conf.SetFormatter(formatter));

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.GetAllErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName == "Username");
        var usernameErrors = result.GetAllErrors().Where(e => e.PropertyName == "Username").ToList();
        Assert.Single(usernameErrors);
        Assert.Contains("CUSTOM:", usernameErrors[0].FormattedMessage);
        Assert.Contains("must contain only alphanumeric characters", usernameErrors[0].FormattedMessage);
    }

    [Fact]
    public void Validate_AlphaNumericWithSpaces_ReturnsInvalidResult()
    {
        // Arrange
        var model = new AlphaNumericModel
        {
            Username = "user 123", // Invalid: contains space
            ProductCode = "PROD456",
            OptionalCode = "OPT789"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.GetAllErrors());
        var usernameErrors = result.GetAllErrors().Where(e => e.PropertyName == "Username").ToList();
        Assert.Single(usernameErrors);
        Assert.Contains("must contain only alphanumeric characters", usernameErrors[0].FormattedMessage);
    }

    [Fact]
    public void Validate_AlphaNumericWithSpecialCharacters_ReturnsInvalidResult()
    {
        // Arrange
        var model = new AlphaNumericModel
        {
            Username = "user#123", // Invalid: contains #
            ProductCode = "PROD456",
            OptionalCode = "OPT789"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.GetAllErrors());
        var usernameErrors = result.GetAllErrors().Where(e => e.PropertyName == "Username").ToList();
        Assert.Single(usernameErrors);
        Assert.Contains("must contain only alphanumeric characters", usernameErrors[0].FormattedMessage);
    }

    [Fact]
    public void Validate_OnlyLetters_ReturnsValidResult()
    {
        // Arrange
        var model = new AlphaNumericModel
        {
            Username = "USERNAME",
            ProductCode = "PRODUCT",
            OptionalCode = "optional"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Validate_OnlyNumbers_ReturnsValidResult()
    {
        // Arrange
        var model = new AlphaNumericModel
        {
            Username = "123456",
            ProductCode = "789012",
            OptionalCode = "345678"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Validate_MixedUpperLowerCaseWithNumbers_ReturnsValidResult()
    {
        // Arrange
        var model = new AlphaNumericModel
        {
            Username = "User123",
            ProductCode = "Prod456XYZ",
            OptionalCode = "Opt789abc"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Validate_NullOptionalCode_ReturnsValidResult()
    {
        // Arrange
        var model = new AlphaNumericModel
        {
            Username = "user123",
            ProductCode = "PROD456",
            OptionalCode = null // Null optional code should be valid
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Validate_BothMainAndNestedInvalidAlphaNumericValues_ReturnsAllErrors()
    {
        // Arrange
        var model = new AlphaNumericNestedModel
        {
            MainCode = "ABC-123", // Invalid main code with hyphen
            Details = new AlphaNumericModel
            {
                Username = "user.123", // Invalid nested username with dot
                ProductCode = "PROD456",
                OptionalCode = "OPT_789" // Invalid nested optional code with underscore
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(3, result.GetAllErrors().Count());

        var mainCodeErrors = result.GetAllErrors().Where(e => e.PropertyName == "MainCode").ToList();
        var detailsUsernameErrors = result.GetAllErrors().Where(e => e.PropertyName == "Details.Username").ToList();
        var detailsOptionalCodeErrors = result.GetAllErrors().Where(e => e.PropertyName == "Details.OptionalCode").ToList();

        Assert.Single(mainCodeErrors);
        Assert.Single(detailsUsernameErrors);
        Assert.Single(detailsOptionalCodeErrors);
        Assert.Contains("must contain only alphanumeric characters", mainCodeErrors[0].FormattedMessage);
        Assert.Contains("must contain only alphanumeric characters", detailsUsernameErrors[0].FormattedMessage);
        Assert.Contains("must contain only alphanumeric characters", detailsOptionalCodeErrors[0].FormattedMessage);
    }

    [Fact]
    public void Validate_LongAlphaNumericString_ReturnsValidResult()
    {
        // Arrange
        var model = new AlphaNumericModel
        {
            Username = "VeryLongUserName123456789WithManyCharacters",
            ProductCode = "VERYLONGPRODUCTCODE987654321ABCDEFGHIJKLMN",
            OptionalCode = "OptionalCode123456789987654321"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Equal(0, result.ErrorsCount);
    }
}

public class CustomTestFormatter : IFormatter
{
    public string Format<T>(AttributeResult result, T value)
    {
        return $"CUSTOM: {string.Format(result.MessageTemplate, result.MessageArgs)}";
    }
}
