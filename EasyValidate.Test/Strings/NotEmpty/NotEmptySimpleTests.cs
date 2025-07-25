namespace EasyValidate.Test.Strings.NotEmpty;

public class NotEmptySimpleTests
{
    [Fact]
    public void NotEmpty_ValidString_ReturnsValidResult()
    {
        // Arrange
        var model = new NotEmptyModel
        {
            Name = "John Doe",
            Email = "test@example.com",
            Description = "Test description"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
       Assert.Equal(0, result.ErrorsCount);
    }

    [Fact]
    public void NotEmpty_EmptyString_ReturnsInvalidResult()
    {
        // Arrange
        var model = new NotEmptyModel
        {
            Name = "", // Empty string should fail NotEmpty validation
            Email = "test@example.com",
            Description = "Test description"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.Path.Contains("Name"));
        Assert.Contains("must not be empty", result.Errors.First(e => e.Path.Contains("Name")).FormattedMessage);
    }

    [Fact]
    public void NotEmpty_NullString_ReturnsInvalidResult()
    {
        // Arrange
        var model = new NotEmptyModel
        {
            Name = null, // Null string should fail NotEmpty validation
            Email = "test@example.com",
            Description = "Test description"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.Path.Contains("Name"));
        Assert.Contains("cannot be null", result.Errors.First(e => e.Path.Contains("Name")).FormattedMessage);
    }

    [Fact]
    public void NotEmpty_MultipleErrors_ReturnsInvalidResult()
    {
        // Arrange
        var model = new NotEmptyModel
        {
            Name = "", // Empty
            Email = "", // Empty
            Description = null // Null
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(3, result.Errors.Count);
        Assert.Contains(result.Errors, e => e.Path.Contains("Name"));
        Assert.Contains(result.Errors, e => e.Path.Contains("Email"));
        Assert.Contains(result.Errors, e => e.Path.Contains("Description"));
    }

    [Theory]
    [InlineData("a")]
    [InlineData("John")]
    [InlineData("Valid Name")]
    [InlineData("Name with spaces")]
    public void NotEmpty_ValidValues_ReturnValidResult(string value)
    {
        // Arrange
        var model = new NotEmptyModel
        {
            Name = value,
            Email = "test@example.com",
            Description = "Test description"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
       Assert.Equal(0, result.ErrorsCount);
    }

    [Theory]
    [InlineData("")]
    public void NotEmpty_InvalidValues_ReturnInvalidResult(string value)
    {
        // Arrange
        var model = new NotEmptyModel
        {
            Name = value,
            Email = "test@example.com",
            Description = "Test description"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.Path.Contains("Name"));
        Assert.Contains("must not be empty", result.Errors.First(e => e.Path.Contains("Name")).FormattedMessage);
    }

    [Fact]
    public void NotEmpty_NullDescription_ReturnsInvalidResult()
    {
        // Arrange
        var model = new NotEmptyModel
        {
            Name = "John Doe",
            Email = "test@example.com",
            Description = null
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.Path.Contains("Description"));
        Assert.Contains("cannot be null", result.Errors.First(e => e.Path.Contains("Description")).FormattedMessage);
    }

    [Fact]
    public void NotEmpty_NestedValidation_ReturnsValidationResult()
    {
        // Arrange
        var model = new NotEmptyNestedModel
        {
            Title = "Valid Title",
            Details = new NotEmptyModel
            {
                Name = "", // This should fail validation
                Email = "test@example.com",
                Description = "Test description"
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Single(result.Errors);
        Assert.Contains(result.Errors, e => e.Path.Contains("Details") && e.Path.Contains("Name"));
        Assert.Contains("must not be empty", result.Errors.First(e => e.Path.Contains("Details") && e.Path.Contains("Name")).FormattedMessage);
    }

    [Fact]
    public void NotEmpty_NestedValidation_MultipleErrors_ReturnsValidationResult()
    {
        // Arrange
        var model = new NotEmptyNestedModel
        {
            Title = "", // This should fail validation
            Details = new NotEmptyModel
            {
                Name = "", // This should fail validation
                Email = "test@example.com",
                Description = "Test description"
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(2, result.Errors.Count);
        Assert.Contains(result.Errors, e => e.Path.Contains("Title"));
        Assert.Contains(result.Errors, e => e.Path.Contains("Details") && e.Path.Contains("Name"));
    }

    [Fact]
    public void NotEmpty_NestedValidation_ValidData_ReturnsValidResult()
    {
        // Arrange
        var model = new NotEmptyNestedModel
        {
            Title = "Valid Title",
            Details = new NotEmptyModel
            {
                Name = "John Doe",
                Email = "test@example.com",
                Description = "Test description"
            }
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
       Assert.Equal(0, result.ErrorsCount);
    }
}
