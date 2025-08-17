using EasyValidate.Abstractions;

using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Guid
{
    public class GuidAdvancedTests
    {
        [Fact]
        public void Guid_NestedObjectValidation_ReturnsCorrectErrors()
        {
            // Arrange
            var model = new GuidNestedModel
            {
                PrimaryGuid = "550e8400-e29b-41d4-a716-446655440000", // Valid
                NestedGuids = new GuidModel
                {
                    UserId = "invalid-guid", // Invalid
                    SessionId = "6ba7b810-9dad-11d1-80b4-00c04fd430c8", // Valid
                    OptionalGuid = "not-a-guid" // Invalid
                }
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.False(result.Property(nameof(model.PrimaryGuid))?.HasErrors());
            Assert.True(result.Property("NestedGuids")?.HasErrors("UserId"));
            Assert.False(result.Property("NestedGuids")?.HasErrors("SessionId"));
            Assert.True(result.Property("NestedGuids")?.HasErrors("OptionalGuid"));

            var nestedUserIdErrors = result.GetAllErrors().Where(e => e.PropertyName == "NestedGuids.UserId").ToList();
            Assert.Single(nestedUserIdErrors);
            Assert.Contains("must be a valid GUID", nestedUserIdErrors[0].FormattedMessage);
        }

        [Fact]
        public void Guid_CustomFormatter_ReturnsFormattedMessage()
        {
            // Arrange
            var model = new GuidModel
            {
                UserId = "invalid-guid", // Invalid
                SessionId = "6ba7b810-9dad-11d1-80b4-00c04fd430c8",
                OptionalGuid = null
            };
            var formatter = new TestFormatter();

            // Act
            var result = model.Validate((conf) => conf.SetFormatter(formatter));

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());

            var userIdErrors = result.GetAllErrors().Where(e => e.PropertyName == nameof(model.UserId)).ToList();
            Assert.Single(userIdErrors);
            Assert.Equal("CUSTOM: The UserId field must be a valid GUID.", userIdErrors[0].FormattedMessage);
        }

        [Fact]
        public void Guid_MultipleErrorsInNestedObject_ReturnsAllErrors()
        {
            // Arrange
            var model = new GuidNestedModel
            {
                PrimaryGuid = "not-a-guid", // Invalid
                NestedGuids = new GuidModel
                {
                    UserId = "also-invalid", // Invalid
                    SessionId = "invalid-too", // Invalid
                    OptionalGuid = "550e8400-e29b-41d4-a716-446655440000" // Valid
                }
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.True(result.Property(nameof(model.PrimaryGuid))?.HasErrors());
            Assert.True(result.Property("NestedGuids")?.HasErrors("UserId"));
            Assert.True(result.Property("NestedGuids")?.HasErrors("SessionId"));
            Assert.False(result.Property("NestedGuids")?.HasErrors("OptionalGuid"));
        }

        [Fact]
        public void Guid_NullNestedObject_ReturnsValidResult()
        {
            // Arrange
            var model = new GuidNestedModel
            {
                PrimaryGuid = "550e8400-e29b-41d4-a716-446655440000",
                NestedGuids = null
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
