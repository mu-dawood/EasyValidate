using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Url
{
    public class UrlSimpleTests
    {
        [Fact]
        public void Url_ValidUrlString_ReturnsValidResult()
        {
            // Arrange
            var model = new UrlModel
            {
                Homepage = "https://www.example.com", // Valid URL
                ApiEndpoint = "http://api.example.com/v1", // Valid URL
                OptionalUrl = "https://docs.example.com" // Valid URL optional
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void Url_InvalidUrlString_ReturnsInvalidResult()
        {
            // Arrange
            var model = new UrlModel
            {
                Homepage = "not-a-url", // Invalid URL
                ApiEndpoint = "http://api.example.com/v1",
                OptionalUrl = "https://docs.example.com"
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Homepage)));
            Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.ApiEndpoint)));
            Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalUrl)));
            var homepageErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.Homepage))).ToList();
            Assert.Single(homepageErrors);
            Assert.Contains("must be a valid URL", homepageErrors[0].FormattedMessage);
        }

        [Fact]
        public void Url_DifferentValidSchemes_ReturnsValidResult()
        {
            // Arrange
            var model = new UrlModel
            {
                Homepage = "https://www.example.com",
                ApiEndpoint = "ftp://files.example.com",
                OptionalUrl = "mailto:test@example.com"
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void Url_EmptyOptionalString_ReturnsValidResult()
        {
            // Arrange
            var model = new UrlModel
            {
                Homepage = "https://www.example.com",
                ApiEndpoint = "http://api.example.com",
                OptionalUrl = null
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void Url_InvalidUrls_ReturnsInvalidResult()
        {
            // Arrange
            var model = new UrlModel
            {
                Homepage = "invalid-url", // Invalid
                ApiEndpoint = "just-text", // Invalid
                OptionalUrl = "https://valid.com"
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.Homepage)));
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.ApiEndpoint)));
            Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalUrl)));
        }
    }
}
