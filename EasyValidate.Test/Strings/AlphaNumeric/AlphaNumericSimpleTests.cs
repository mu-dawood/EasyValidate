namespace EasyValidate.Test.Strings.AlphaNumeric;

public class AlphaNumericSimpleTests
{
    [Fact]
    public void AlphaNumeric_ValidAlphaNumericStrings_ReturnsValidResult()
    {
        // Arrange
        var model = new AlphaNumericModel
        {
            Username = "user123",
            ProductCode = "ABC123",
            OptionalCode = "CODE456"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void AlphaNumeric_InvalidStringWithSpecialCharacters_ReturnsInvalidResult()
    {
        // Arrange
        var model = new AlphaNumericModel
        {
            Username = "user@123", // Invalid due to @ character
            ProductCode = "ABC123",
            OptionalCode = "CODE456"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.Errors, e => e.Path.Contains(nameof(model.Username)));
        Assert.DoesNotContain(result.Errors, e => e.Path.Contains(nameof(model.ProductCode)));
        Assert.DoesNotContain(result.Errors, e => e.Path.Contains(nameof(model.OptionalCode)));
        var usernameErrors = result.Errors.Where(e => e.Path.Contains(nameof(model.Username))).ToList();
        Assert.Single(usernameErrors);
        Assert.Contains("must contain only alphanumeric characters", usernameErrors[0].FormattedMessage);
    }

    [Fact]
    public void AlphaNumeric_EmptyString_ReturnsInvalidResult()
    {
        // Arrange
        var model = new AlphaNumericModel
        {
            Username = "", // Empty string should fail AlphaNumeric validation
            ProductCode = "ABC123",
            OptionalCode = "CODE456"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.Errors, e => e.Path.Contains(nameof(model.Username)));
        var usernameErrors = result.Errors.Where(e => e.Path.Contains(nameof(model.Username))).ToList();
        Assert.Single(usernameErrors);
        Assert.Contains("must contain only alphanumeric characters", usernameErrors[0].FormattedMessage);
    }

    [Fact]
    public void AlphaNumeric_MultipleInvalidValues_ReturnsMultipleErrors()
    {
        // Arrange
        var model = new AlphaNumericModel
        {
            Username = "user@123", // Invalid
            ProductCode = "ABC-123", // Invalid
            OptionalCode = "CODE@456" // Invalid
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(3, result.Errors.Count);
        Assert.Contains(result.Errors, e => e.Path.Contains(nameof(model.Username)));
        Assert.Contains(result.Errors, e => e.Path.Contains(nameof(model.ProductCode)));
        Assert.Contains(result.Errors, e => e.Path.Contains(nameof(model.OptionalCode)));
    }

    [Theory]
    [InlineData("user123")]
    [InlineData("ABC")]
    [InlineData("123")]
    [InlineData("Code456")]
    [InlineData("TEST")]
    public void AlphaNumeric_ValidAlphaNumericValues_ReturnsValidResult(string value)
    {
        // Arrange
        var model = new AlphaNumericModel
        {
            Username = value,
            ProductCode = "ABC123",
            OptionalCode = "CODE456"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData("user@123")]
    [InlineData("ABC-123")]
    [InlineData("user 123")]
    [InlineData("user!123")]
    [InlineData("test#code")]
    public void AlphaNumeric_InvalidNonAlphaNumericValues_ReturnsInvalidResult(string value)
    {
        // Arrange
        var model = new AlphaNumericModel
        {
            Username = value,
            ProductCode = "ABC123",
            OptionalCode = "CODE456"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.Errors, e => e.Path.Contains(nameof(model.Username)));
        Assert.DoesNotContain(result.Errors, e => e.Path.Contains(nameof(model.ProductCode)));
        Assert.DoesNotContain(result.Errors, e => e.Path.Contains(nameof(model.OptionalCode)));
        var usernameErrors = result.Errors.Where(e => e.Path.Contains(nameof(model.Username))).ToList();
        Assert.Single(usernameErrors);
        Assert.Contains("must contain only alphanumeric characters", usernameErrors[0].FormattedMessage);
    }
}
