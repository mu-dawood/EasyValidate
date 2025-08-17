using EasyValidate.Abstractions;

using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Phone
{
    public class PhoneAdvancedTests
    {
        [Fact]
        public void Phone_NestedObjectValidation_ReturnsCorrectErrors()
        {
            // Arrange
            var model = new PhoneNestedModel
            {
                PrimaryPhone = "+1-234-567-8900", // Valid
                NestedPhones = new PhoneModel
                {
                    ContactNumber = "123", // Invalid - too short
                    EmergencyContact = "123-456-7890", // Valid
                    OptionalPhone = "invalid-phone" // Invalid
                }
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.False(result.Property(nameof(model.PrimaryPhone))?.HasErrors());
            Assert.True(result.Property("NestedPhones")?.HasErrors("ContactNumber"));
            Assert.False(result.Property("NestedPhones")?.HasErrors("EmergencyContact"));
            Assert.True(result.Property("NestedPhones")?.HasErrors("OptionalPhone"));

            var nestedContactErrors = result.GetAllErrors().Where(e => e.PropertyName == "NestedPhones.ContactNumber").ToList();
            Assert.Single(nestedContactErrors);
            Assert.Contains("must be a valid phone number", nestedContactErrors[0].FormattedMessage);
        }

        [Fact]
        public void Phone_CustomFormatter_ReturnsFormattedMessage()
        {
            // Arrange
            var model = new PhoneModel
            {
                ContactNumber = "abc", // Invalid
                EmergencyContact = "123-456-7890",
                OptionalPhone = null
            };
            var formatter = new TestFormatter();

            // Act
            var result = model.Validate((conf) => conf.SetFormatter(formatter));

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());

            var contactErrors = result.GetAllErrors().Where(e => e.PropertyName == nameof(model.ContactNumber)).ToList();
            Assert.Single(contactErrors);
            Assert.Equal("CUSTOM: The ContactNumber field must be a valid phone number.", contactErrors[0].FormattedMessage);
        }

        [Fact]
        public void Phone_MultipleErrorsInNestedObject_ReturnsAllErrors()
        {
            // Arrange
            var model = new PhoneNestedModel
            {
                PrimaryPhone = "invalid", // Invalid
                NestedPhones = new PhoneModel
                {
                    ContactNumber = "123", // Invalid
                    EmergencyContact = "abc-def-ghij", // Invalid
                    OptionalPhone = "123-456-7890" // Valid
                }
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.True(result.Property(nameof(model.PrimaryPhone))?.HasErrors());
            Assert.True(result.Property("NestedPhones")?.HasErrors("ContactNumber"));
            Assert.True(result.Property("NestedPhones")?.HasErrors("EmergencyContact"));
            Assert.False(result.Property("NestedPhones")?.HasErrors("OptionalPhone"));
        }

        [Fact]
        public void Phone_NullNestedObject_ReturnsValidResult()
        {
            // Arrange
            var model = new PhoneNestedModel
            {
                PrimaryPhone = "123-456-7890",
                NestedPhones = null
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
