using System;
using EasyValidate.Core.Abstraction;

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
            Assert.False(result.HasErrors(nameof(model.PrimaryUrl)));
            Assert.True(result.HasErrors("NestedUrls", "Homepage"));
            Assert.False(result.HasErrors("NestedUrls", "ApiEndpoint"));
            Assert.True(result.HasErrors("NestedUrls", "OptionalUrl"));

            var nestedHomepageErrors = result.Errors.Where(e => e.Path.SequenceEqual(new[] { "NestedUrls", "Homepage" })).ToList();
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
            var result = model.Validate(formatter);

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());

            var homepageErrors = result.Errors.Where(e => e.Path.SequenceEqual(new[] { nameof(model.Homepage) })).ToList();
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
            Assert.True(result.HasErrors(nameof(model.PrimaryUrl)));
            Assert.True(result.HasErrors("NestedUrls", "Homepage"));
            Assert.True(result.HasErrors("NestedUrls", "ApiEndpoint"));
            Assert.False(result.HasErrors("NestedUrls", "OptionalUrl"));
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
