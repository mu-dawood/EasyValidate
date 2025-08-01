using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.EndsWith;

public class EndsWithSimpleTests
{
    [Fact]
    public void ValidEndsWithStrings_ShouldReturnValid()
    {
        // Arrange
        var model = new EndsWithModel
        {
            Domain = "example.com",
            TestFile = "myfile_test",
            Exclamation = "Hello World!",
            Email = "user@domain.org",
            EndsWithSpace = "Text with trailing space "
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Fact]
    public void Domain_NotEndingWithCom_ShouldReturnInvalid()
    {
        // Arrange
        var model = new EndsWithModel
        {
            Domain = "example.org",
            TestFile = "file_test",
            Exclamation = "Hello!",
            Email = "user@domain.org",
            EndsWithSpace = "text "
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("Domain"));
    }

    [Fact]
    public void TestFile_NotEndingWithTest_ShouldReturnInvalid()
    {
        // Arrange
        var model = new EndsWithModel
        {
            Domain = "example.com",
            TestFile = "myfile_production",
            Exclamation = "Hello!",
            Email = "user@domain.org",
            EndsWithSpace = "text "
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("TestFile"));
    }

    [Theory]
    [InlineData("google.com")]
    [InlineData("subdomain.example.com")]
    [InlineData("www.test.com")]
    [InlineData(".com")]
    [InlineData("a.com")]
    public void Domain_WithValidComDomains_ShouldReturnValid(string domain)
    {
        // Arrange
        var model = new EndsWithModel
        {
            Domain = domain
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("Domain")));
    }

    [Theory]
    [InlineData("example.org")]
    [InlineData("example.net")]
    [InlineData("example.COM")] // Case sensitive
    [InlineData("example.co")]
    [InlineData("")]
    [InlineData("com")]
    [InlineData("example.com.backup")]
    public void Domain_WithInvalidDomains_ShouldReturnInvalid(string domain)
    {
        // Arrange
        var model = new EndsWithModel
        {
            Domain = domain
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("Domain"));
    }

    [Fact]
    public void NullString_ShouldReturnInvalid()
    {
        // Arrange
        var model = new EndsWithModel
        {
            Domain = null
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
        var model = new EndsWithModel
        {
            Domain = ""
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
        var model = new EndsWithModel
        {
            Domain = ".com",
            TestFile = "_test",
            Exclamation = "!",
            Email = "@domain.org",
            EndsWithSpace = " "
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Theory]
    [InlineData("my_test")]
    [InlineData("file_test")]
    [InlineData("unit_test")]
    [InlineData("integration_test")]
    [InlineData("_test")]
    public void TestFile_WithValidEndings_ShouldReturnValid(string filename)
    {
        // Arrange
        var model = new EndsWithModel
        {
            TestFile = filename
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("TestFile")));
    }

    [Theory]
    [InlineData("Hello!")]
    [InlineData("World!")]
    [InlineData("Exclamation!")]
    [InlineData("!")]
    [InlineData("Multiple words end with exclamation!")]
    public void Exclamation_WithValidEndings_ShouldReturnValid(string text)
    {
        // Arrange
        var model = new EndsWithModel
        {
            Exclamation = text
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("Exclamation")));
    }

    [Theory]
    [InlineData("Hello?")]
    [InlineData("World.")]
    [InlineData("No exclamation")]
    [InlineData("!wrong")]
    [InlineData("Exclamation! ")]  // Has space after
    public void Exclamation_WithInvalidEndings_ShouldReturnInvalid(string text)
    {
        // Arrange
        var model = new EndsWithModel
        {
            Exclamation = text
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("Exclamation"));
    }

    [Fact]
    public void MultipleFailures_ShouldReturnMultipleErrors()
    {
        // Arrange
        var model = new EndsWithModel
        {
            Domain = "example.org",
            TestFile = "production_file",
            Exclamation = "Hello?",
            Email = "user@domain.com",
            EndsWithSpace = "NoSpace"
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.True(result.HasErrors());
        Assert.Equal(5, result.GetAllErrors().Count());
    }

    [Fact]
    public void CaseSensitive_ShouldMatter()
    {
        // Arrange
        var model = new EndsWithModel
        {
            Domain = "example.COM"  // Wrong case
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.False(result.IsValid());
        Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains("Domain"));
    }

    [Fact]
    public void SpecialCharacters_ShouldWork()
    {
        // Arrange
        var model = new EndsWithModel
        {
            Domain = "special-chars$.com",
            TestFile = "file with spaces_test",
            Exclamation = "Special chars: àéî!",
            Email = "üser@domain.org",
            EndsWithSpace = "Unicode text with space "
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }

    [Theory]
    [InlineData("user@domain.org")]
    [InlineData("test@domain.org")]
    [InlineData("admin@domain.org")]
    [InlineData("@domain.org")]
    public void Email_WithValidEndings_ShouldReturnValid(string email)
    {
        // Arrange
        var model = new EndsWithModel
        {
            Email = email
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid() || !result.GetAllErrors().Any(e => e.PropertyName.Contains("Email")));
    }

    [Fact]
    public void VeryLongString_EndingCorrectly_ShouldWork()
    {
        // Arrange
        var longString = new string('a', 1000) + ".com";
        var model = new EndsWithModel
        {
            Domain = longString
        };

        // Act
        var result = model.Validate();

        // Assert
        Assert.True(result.IsValid());
        Assert.False(result.HasErrors());
    }
}
