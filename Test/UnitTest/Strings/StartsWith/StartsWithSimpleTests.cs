using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.StartsWith;

public class StartsWithSimpleTests
{
    [Fact]
    public void ValidStartsWithStrings_ShouldReturnValid()
    {
        // Arrange
        var model = new StartsWithModel
        {
            Greeting = "Hello World",
            SecureUrl = "https://www.example.com",
            FormalTitle = "Mr. John Smith",
            NumberString = "0123456789",
            StartsWithSpace = " Leading space text"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void Greeting_NotStartingWithHello_ShouldReturnInvalid()
    {
        // Arrange
        var model = new StartsWithModel
        {
            Greeting = "Hi there",
            SecureUrl = "https://example.com",
            FormalTitle = "Mr. Smith",
            NumberString = "0123",
            StartsWithSpace = " text"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("Greeting"));
    }

    [Fact]
    public void SecureUrl_NotStartingWithHttps_ShouldReturnInvalid()
    {
        // Arrange
        var model = new StartsWithModel
        {
            Greeting = "Hello World",
            SecureUrl = "http://www.example.com",
            FormalTitle = "Mr. Smith",
            NumberString = "0123",
            StartsWithSpace = " text"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("SecureUrl"));
    }

    [Theory]
    [InlineData("Hello")]
    [InlineData("Hello there")]
    [InlineData("Hello World!")]
    [InlineData("Hello123")]
    [InlineData("HelloWorld")]
    public void Greeting_WithValidStarts_ShouldReturnValid(string input)
    {
        // Arrange
        var model = new StartsWithModel
        {
            Greeting = input
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("Greeting")));
    }

    [Theory]
    [InlineData("hello")]  // Case sensitive
    [InlineData("HELLO")]  // Case sensitive
    [InlineData("Hi")]
    [InlineData("Hey")]
    [InlineData("")]
    [InlineData("Hell")]   // Partial match
    public void Greeting_WithInvalidStarts_ShouldReturnInvalid(string input)
    {
        // Arrange
        var model = new StartsWithModel
        {
            Greeting = input
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("Greeting"));
    }

    [Fact]
    public void NullString_ShouldReturnInvalid()
    {
        // Arrange
        var model = new StartsWithModel
        {
            Greeting = null
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
        var model = new StartsWithModel
        {
            Greeting = ""
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
    }

    [Fact]
    public void ExactMatch_ShouldReturnValid()
    {
        // Arrange
        var model = new StartsWithModel
        {
            Greeting = "Hello",
            SecureUrl = "https://",
            FormalTitle = "Mr.",
            NumberString = "0",
            StartsWithSpace = " "
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void MultipleFailures_ShouldReturnMultipleErrors()
    {
        // Arrange
        var model = new StartsWithModel
        {
            Greeting = "Hi there",
            SecureUrl = "http://example.com",
            FormalTitle = "Dr. Smith",
            NumberString = "123",
            StartsWithSpace = "NoSpace"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(5, result.GetAllErrors().Count());
    }

    [Theory]
    [InlineData("https://google.com")]
    [InlineData("https://")]
    [InlineData("https://www.example.com/path?query=value")]
    public void SecureUrl_WithValidHttpsUrls_ShouldReturnValid(string url)
    {
        // Arrange
        var model = new StartsWithModel
        {
            SecureUrl = url
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("SecureUrl")));
    }

    [Theory]
    [InlineData("0")]
    [InlineData("01")]
    [InlineData("0123456789")]
    [InlineData("0.5")]
    [InlineData("0xFF")]
    public void NumberString_WithValidStarts_ShouldReturnValid(string number)
    {
        // Arrange
        var model = new StartsWithModel
        {
            NumberString = number
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("NumberString")));
    }
}
