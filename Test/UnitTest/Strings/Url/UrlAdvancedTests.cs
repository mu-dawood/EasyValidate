using EasyValidate.Abstractions;

using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Url
{
    public class UrlAdvancedTests
    {
        [Fact]
        public void Url_NestedObjectValidation_ReturnsCorrectErrors()
        {
            // Arrange
            var model = new UrlNestedModel
            {
                PrimaryUrl = "https://valid.com", // Valid
                NestedUrls = new UrlModel
                {
                    Homepage = "invalid-url", // Invalid
                    ApiEndpoint = "http://api.example.com", // Valid
                    OptionalUrl = "not-a-url" // Invalid
                }
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.False(result.Property(nameof(model.PrimaryUrl))?.HasErrors());
            Assert.True(result.Property("NestedUrls")?.HasErrors("Homepage"));
            Assert.False(result.Property("NestedUrls")?.HasErrors("ApiEndpoint"));
            Assert.True(result.Property("NestedUrls")?.HasErrors("OptionalUrl"));

            var nestedHomepageErrors = result.GetAllErrors().Where(e => e.PropertyName == "NestedUrls.Homepage").ToList();
            Assert.Single(nestedHomepageErrors);
            Assert.Contains("must be a valid URL", nestedHomepageErrors[0].FormattedMessage);
        }

        [Fact]
        public void Url_CustomFormatter_ReturnsFormattedMessage()
        {
            // Arrange
            var model = new UrlModel
            {
                Homepage = "invalid-url", // Invalid
                ApiEndpoint = "http://api.example.com",
                OptionalUrl = null
            };
            var formatter = new TestFormatter();

            // Act
            var result = model.Validate((conf) => conf.SetFormatter(formatter));

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());

            var homepageErrors = result.GetAllErrors().Where(e => e.PropertyName == nameof(model.Homepage)).ToList();
            Assert.Single(homepageErrors);
            Assert.Equal("CUSTOM: The Homepage field must be a valid URL.", homepageErrors[0].FormattedMessage);
        }

        [Fact]
        public void Url_MultipleErrorsInNestedObject_ReturnsAllErrors()
        {
            // Arrange
            var model = new UrlNestedModel
            {
                PrimaryUrl = "not-a-url", // Invalid
                NestedUrls = new UrlModel
                {
                    Homepage = "also-invalid", // Invalid
                    ApiEndpoint = "invalid-endpoint", // Invalid
                    OptionalUrl = "https://valid.com" // Valid
                }
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.True(result.Property(nameof(model.PrimaryUrl))?.HasErrors());
            Assert.True(result.Property("NestedUrls")?.HasErrors("Homepage"));
            Assert.True(result.Property("NestedUrls")?.HasErrors("ApiEndpoint"));
            Assert.False(result.Property("NestedUrls")?.HasErrors("OptionalUrl"));
        }

        [Fact]
        public void Url_NullNestedObject_ReturnsValidResult()
        {
            // Arrange
            var model = new UrlNestedModel
            {
                PrimaryUrl = "https://example.com",
                NestedUrls = null
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        private class TestFormatter : IFormatter
        {
            public string Format<T>(AttributeResult result, T value)
            {
                return $"CUSTOM: {string.Format(result.MessageTemplate, result.MessageArgs)}";
            }
        }
    }
}
