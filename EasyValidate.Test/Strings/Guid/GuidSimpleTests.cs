using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Guid
{
    public class GuidSimpleTests
    {
        [Fact]
        public void Guid_ValidGuidString_ReturnsValidResult()
        {
            // Arrange
            var model = new GuidModel
            {
                UserId = "550e8400-e29b-41d4-a716-446655440000", // Valid GUID
                SessionId = "6ba7b810-9dad-11d1-80b4-00c04fd430c8", // Valid GUID
                OptionalGuid = "123e4567-e89b-12d3-a456-426614174000" // Valid GUID optional
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void Guid_InvalidGuidString_ReturnsInvalidResult()
        {
            // Arrange
            var model = new GuidModel
            {
                UserId = "invalid-guid", // Invalid GUID
                SessionId = "6ba7b810-9dad-11d1-80b4-00c04fd430c8",
                OptionalGuid = "123e4567-e89b-12d3-a456-426614174000"
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.UserId)));
            Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.SessionId)));
            Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalGuid)));
            var userIdErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.UserId))).ToList();
            Assert.Single(userIdErrors);
            Assert.Contains("must be a valid GUID", userIdErrors[0].FormattedMessage);
        }

        [Fact]
        public void Guid_DifferentValidFormats_ReturnsValidResult()
        {
            // Arrange
            var model = new GuidModel
            {
                UserId = "00000000-0000-0000-0000-000000000000", // Empty GUID
                SessionId = "FFFFFFFF-FFFF-FFFF-FFFF-FFFFFFFFFFFF", // All F's GUID
                OptionalGuid = "12345678-1234-5678-9012-123456789012" // Regular GUID
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void Guid_EmptyOptionalString_ReturnsValidResult()
        {
            // Arrange
            var model = new GuidModel
            {
                UserId = "550e8400-e29b-41d4-a716-446655440000",
                SessionId = "6ba7b810-9dad-11d1-80b4-00c04fd430c8",
                OptionalGuid = null
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void Guid_InvalidGuidFormats_ReturnsInvalidResult()
        {
            // Arrange
            var model = new GuidModel
            {
                UserId = "123456789", // Invalid - wrong format
                SessionId = "not-a-guid", // Invalid - not a GUID
                OptionalGuid = "550e8400-e29b-41d4-a716-446655440000"
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.UserId)));
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.SessionId)));
            Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalGuid)));
        }
    }
}
