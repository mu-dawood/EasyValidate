using EasyValidate.Core.Abstraction;

namespace EasyValidate.Test.Strings.NotEmpty;

public class NotEmptyAdvancedTests
{
    [Fact]
    public void Validate_NestedObjectWithValidData_ReturnsValidResult()
    {
        // Arrange
        var model = new NotEmptyNestedModel
        {
            Title = "Main Title",
            Details = new NotEmptyModel
            {
                Name = "Nested Name",
                Email = "nested@example.com",
                Description = "Nested description"
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
    public void Validate_NestedObjectWithEmptyFields_ReturnsInvalidResultWithPrefixedErrors()
    {
        // Arrange
        var model = new NotEmptyNestedModel
        {
            Title = "Main Title",
            Details = new NotEmptyModel
            {
                Name = "",
                Email = "nested@example.com",
                Description = null
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(2, result.Errors.Count);
        Assert.Contains(result.Errors, e => e.Path.SequenceEqual(new[] { "Details", "Name" }));
        Assert.Contains(result.Errors, e => e.Path.SequenceEqual(new[] { "Details", "Description" }));
        Assert.Contains(result.Errors.Where(e => e.Path.SequenceEqual(new[] { "Details", "Name" })), e => e.FormattedMessage.Contains("must not be empty"));
        Assert.Contains(result.Errors.Where(e => e.Path.SequenceEqual(["Details", "Description"])), e => e.FormattedMessage.Contains("cannot be null"));
    }

    [Fact]
    public void Validate_NullNestedObject_ReturnsValidResult()
    {
        // Arrange
        var model = new NotEmptyNestedModel
        {
            Title = "Main Title",
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
        var model = new NotEmptyModel
        {
            Name = "",
            Email = "valid@example.com",
            Description = "Valid description"
        };
        var formatter = new CustomTestFormatter();

        // Act
        var result = model.Validate(formatter);

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.Path.SequenceEqual(new[] { "Name" }));
        var nameErrors = result.Errors.Where(e => e.Path.SequenceEqual(new[] { "Name" })).ToList();
        Assert.Single(nameErrors);
        Assert.Contains("CUSTOM:", nameErrors[0].FormattedMessage);
        Assert.Contains("must not be empty", nameErrors[0].FormattedMessage);
    }

    [Fact]
    public void Validate_StringWithOnlySpaces_ReturnsInvalidResult()
    {
        // Arrange
        var model = new NotEmptyModel
        {
            Name = "                    ", // 20 spaces
            Email = "valid@example.com",
            Description = "Valid description"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.Path.SequenceEqual(new[] { "Name" }));
        Assert.Contains(result.Errors.Where(e => e.Path.SequenceEqual(new[] { "Name" })), e => e.FormattedMessage.Contains("must not be empty"));
    }

    [Fact]
    public void Validate_StringWithUnicodeWhitespace_ReturnsInvalidResult()
    {
        // Arrange
        var model = new NotEmptyModel
        {
            Name = "\u00A0\u2000\u2001\u2002\u2003", // Various Unicode whitespace characters
            Email = "valid@example.com",
            Description = "Valid description"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.Path.SequenceEqual(new[] { "Name" }));
        Assert.Contains(result.Errors.Where(e => e.Path.SequenceEqual(new[] { "Name" })), e => e.FormattedMessage.Contains("must not be empty"));
    }

    [Fact]
    public void Validate_EmptyStringAfterTrim_ReturnsInvalidResult()
    {
        // Arrange
        var model = new NotEmptyModel
        {
            Name = "   \t\n\r   ",
            Email = "valid@example.com",
            Description = "Valid description"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.Path.SequenceEqual(new[] { "Name" }));
        Assert.Contains(result.Errors.Where(e => e.Path.SequenceEqual(new[] { "Name" })), e => e.FormattedMessage.Contains("must not be empty"));
    }

    [Fact]
    public void Validate_StringWithValidContent_ReturnsValidResult()
    {
        // Arrange
        var model = new NotEmptyModel
        {
            Name = "  Valid Name  ", // Has valid content even with surrounding whitespace
            Email = "valid@example.com",
            Description = "Valid description"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
       Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void Validate_BothMainAndNestedEmptyFields_ReturnsAllErrors()
    {
        // Arrange
        var model = new NotEmptyNestedModel
        {
            Title = "", // Empty main field
            Details = new NotEmptyModel
            {
                Name = "", // Empty nested field
                Email = "valid@example.com",
                Description = "   " // Whitespace nested field
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(3, result.Errors.Count);
        Assert.Contains(result.Errors, e => e.Path.SequenceEqual(new[] { "Title" }));
        Assert.Contains(result.Errors, e => e.Path.SequenceEqual(new[] { "Details", "Name" }));
        Assert.Contains(result.Errors, e => e.Path.SequenceEqual(new[] { "Details", "Description" }));
        Assert.Contains(result.Errors.Where(e => e.Path.SequenceEqual(new[] { "Title" })), e => e.FormattedMessage.Contains("must not be empty"));
        Assert.Contains(result.Errors.Where(e => e.Path.SequenceEqual(new[] { "Details", "Name" })), e => e.FormattedMessage.Contains("must not be empty"));
        Assert.Contains(result.Errors.Where(e => e.Path.SequenceEqual(new[] { "Details", "Description" })), e => e.FormattedMessage.Contains("must not be empty"));
    }
}

public class CustomTestFormatter : IFormatter
{
    public string Format<T>(AttributeResult result, T value)
    {
        return $"CUSTOM: {string.Format(result.MessageTemplate, result.MessageArgs)}";
    }
}
