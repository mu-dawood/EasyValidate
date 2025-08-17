using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Phone
{
    public class PhoneSimpleTests
    {
        [Fact]
        public void Phone_ValidPhoneString_ReturnsValidResult()
        {
            // Arrange
            var model = new PhoneModel
            {
                ContactNumber = "+1-234-567-8900", // Valid international format
                EmergencyContact = "123-456-7890", // Valid US format
                OptionalPhone = "(555) 123-4567" // Valid phone optional
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void Phone_InvalidPhoneString_ReturnsInvalidResult()
        {
            // Arrange
            var model = new PhoneModel
            {
                ContactNumber = "123456", // Invalid - too short
                EmergencyContact = "123-456-7890",
                OptionalPhone = "(555) 123-4567"
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.ContactNumber)));
            Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.EmergencyContact)));
            Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalPhone)));
            var contactErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.ContactNumber))).ToList();
            Assert.Single(contactErrors);
            Assert.Contains("must be a valid phone number", contactErrors[0].FormattedMessage);
        }

        [Fact]
        public void Phone_DifferentValidFormats_ReturnsValidResult()
        {
            // Arrange
            var model = new PhoneModel
            {
                ContactNumber = "+44 20 7946 0958", // UK format
                EmergencyContact = "(555) 123-4567", // US format with parentheses
                OptionalPhone = "1234567890" // Simple format
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void Phone_EmptyOptionalString_ReturnsValidResult()
        {
            // Arrange
            var model = new PhoneModel
            {
                ContactNumber = "123-456-7890",
                EmergencyContact = "+1-555-123-4567",
                OptionalPhone = null
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void Phone_InvalidPhoneFormats_ReturnsInvalidResult()
        {
            // Arrange
            var model = new PhoneModel
            {
                ContactNumber = "abc-def-ghij", // Invalid - letters
                EmergencyContact = "555", // Invalid - too short
                OptionalPhone = "+1-234-567-8900"
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.ContactNumber)));
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.EmergencyContact)));
            Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalPhone)));
        }
    }
}
