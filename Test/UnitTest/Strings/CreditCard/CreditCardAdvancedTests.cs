using EasyValidate.Abstractions;
using System;
using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.CreditCard
{
    public class CreditCardAdvancedTests
    {
        [Fact]
        public void CreditCard_NestedObjectValidation_ReturnsCorrectErrors()
        {
            // Arrange
            var model = new CreditCardNestedModel
            {
                PrimaryCard = "4532015112830366", // Valid Visa
                NestedCards = new CreditCardModel
                {
                    CardNumber = "1234567890123456", // Invalid - fails Luhn check
                    PaymentCard = "4000000000000002", // Valid
                    OptionalCard = "invalid-card" // Invalid
                }
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.False(result.Property(nameof(model.PrimaryCard))?.HasErrors());
            Assert.True(result.Property("NestedCards")?.HasErrors("CardNumber"));
            Assert.False(result.Property("NestedCards")?.HasErrors("PaymentCard"));
            Assert.True(result.Property("NestedCards")?.HasErrors("OptionalCard"));

            var nestedCardErrors = result.GetAllErrors().Where(e => e.PropertyName == "NestedCards.CardNumber").ToList();
            Assert.Single(nestedCardErrors);
            Assert.Contains("must be a valid credit card number", nestedCardErrors[0].FormattedMessage);
        }

        [Fact]
        public void CreditCard_CustomFormatter_ReturnsFormattedMessage()
        {
            // Arrange
            var model = new CreditCardModel
            {
                CardNumber = "1234567890123456", // Invalid
                PaymentCard = "4000000000000002",
                OptionalCard = null
            };
            var formatter = new TestFormatter();

            // Act
            var result = model.Validate((conf) => conf.SetFormatter(formatter));

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());

            var cardErrors = result.GetAllErrors().Where(e => e.PropertyName == nameof(model.CardNumber)).ToList();
            Assert.Single(cardErrors);
            Assert.Equal("CUSTOM: The CardNumber field must be a valid credit card number.", cardErrors[0].FormattedMessage);
        }

        [Fact]
        public void CreditCard_MultipleErrorsInNestedObject_ReturnsAllErrors()
        {
            // Arrange
            var model = new CreditCardNestedModel
            {
                PrimaryCard = "1111111111111111", // Invalid
                NestedCards = new CreditCardModel
                {
                    CardNumber = "2222222222222222", // Invalid
                    PaymentCard = "not-a-card", // Invalid
                    OptionalCard = "4532015112830366" // Valid
                }
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.True(result.Property(nameof(model.PrimaryCard))?.HasErrors());
            Assert.True(result.Property("NestedCards")?.HasErrors("CardNumber"));
            Assert.True(result.Property("NestedCards")?.HasErrors("PaymentCard"));
            Assert.False(result.Property("NestedCards")?.HasErrors("OptionalCard"));
        }

        [Fact]
        public void CreditCard_NullNestedObject_ReturnsValidResult()
        {
            // Arrange
            var model = new CreditCardNestedModel
            {
                PrimaryCard = "4532015112830366",
                NestedCards = null
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void CreditCard_CustomConfigureValidator_ReturnsConfiguredValidation()
        {
            // Arrange
            var model = new CreditCardModel
            {
                CardNumber = "1234567890123456", // Invalid
                PaymentCard = "4000000000000002",
                OptionalCard = null
            };
            var serviceProvider = new TestServiceProvider();

            // Act
            var result = model.Validate(serviceProvider);

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());

            var cardErrors = result.GetAllErrors().Where(e => e.PropertyName == nameof(model.CardNumber)).ToList();
            Assert.Single(cardErrors);
            Assert.Contains("must be a valid credit card number", cardErrors[0].FormattedMessage);
        }

        [Fact]
        public void CreditCard_CustomFormatterAndConfigurator_ReturnsBothCustomizations()
        {
            // Arrange
            var model = new CreditCardModel
            {
                CardNumber = "1234567890123456", // Invalid
                PaymentCard = "4000000000000002",
                OptionalCard = null
            };
            var formatter = new TestFormatter();
            var configurator = new TestServiceProvider();

            // Act
            var result = model.Validate((conf) => conf.SetFormatter(formatter));

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());

            var cardErrors = result.GetAllErrors().Where(e => e.PropertyName == nameof(model.CardNumber)).ToList();
            Assert.Single(cardErrors);
            Assert.Equal("CUSTOM: The CardNumber field must be a valid credit card number.", cardErrors[0].FormattedMessage);
        }

        [Fact]
        public void CreditCard_ValidateWithFormatter_OnlyUsesCustomFormatter()
        {
            // Arrange
            var model = new CreditCardModel
            {
                CardNumber = "1234567890123456", // Invalid
                PaymentCard = "4000000000000002",
                OptionalCard = null
            };
            var formatter = new TestFormatter();

            // Act
            var result = model.Validate((conf) => conf.SetFormatter(formatter));

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());

            var cardErrors = result.GetAllErrors().Where(e => e.PropertyName == nameof(model.CardNumber)).ToList();
            Assert.Single(cardErrors);
            Assert.Equal("CUSTOM: The CardNumber field must be a valid credit card number.", cardErrors[0].FormattedMessage);
        }

        private class TestFormatter : IFormatter
        {
            public string Format<T>(AttributeResult result, T value)
            {
                return $"CUSTOM: {string.Format(result.MessageTemplate, result.MessageArgs)}";
            }
        }

        private class TestServiceProvider : IServiceProvider
        {
            public object? GetService(Type serviceType)
            {
                // For demonstration purposes, return null for all services
                // In real scenarios, this could return actual service implementations
                return null;
            }
        }
    }
}
