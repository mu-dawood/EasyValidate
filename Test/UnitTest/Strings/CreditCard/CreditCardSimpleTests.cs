using EasyValidate.Abstractions;

using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.CreditCard
{
    public class CreditCardSimpleTests
    {
        [Fact]
        public void CreditCard_ValidCreditCardString_ReturnsValidResult()
        {
            // Arrange
            var model = new CreditCardModel
            {
                CardNumber = "4532015112830366", // Valid Visa
                PaymentCard = "4000000000000002", // Valid test card
                OptionalCard = "5555555555554444" // Valid MasterCard
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void CreditCard_InvalidCreditCardString_ReturnsInvalidResult()
        {
            // Arrange
            var model = new CreditCardModel
            {
                CardNumber = "1234567890123456", // Invalid - fails Luhn check
                PaymentCard = "4000000000000002",
                OptionalCard = "5555555555554444"
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.CardNumber)));
            Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.PaymentCard)));
            Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalCard)));
            var cardErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.CardNumber))).ToList();
            Assert.Single(cardErrors);
            Assert.Contains("must be a valid credit card number", cardErrors[0].FormattedMessage);
        }

        [Fact]
        public void CreditCard_DifferentValidCards_ReturnsValidResult()
        {
            // Arrange
            var model = new CreditCardModel
            {
                CardNumber = "4111111111111111", // Valid Visa
                PaymentCard = "5555555555554444", // Valid MasterCard
                OptionalCard = "378282246310005" // Valid American Express
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void CreditCard_EmptyOptionalString_ReturnsValidResult()
        {
            // Arrange
            var model = new CreditCardModel
            {
                CardNumber = "4532015112830366",
                PaymentCard = "4000000000000002",
                OptionalCard = null
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void CreditCard_InvalidCardFormats_ReturnsInvalidResult()
        {
            // Arrange
            var model = new CreditCardModel
            {
                CardNumber = "123456", // Invalid - too short
                PaymentCard = "not-a-card", // Invalid - not numeric
                OptionalCard = "4532015112830366"
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.CardNumber)));
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.PaymentCard)));
            Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalCard)));
        }

        [Fact]
        public void CreditCard_ValidateWithDefaultConfigurator_ReturnsValidResult()
        {
            // Arrange
            var model = new CreditCardModel
            {
                CardNumber = "4532015112830366", // Valid Visa
                PaymentCard = "4000000000000002", // Valid test card
                OptionalCard = "5555555555554444" // Valid MasterCard
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void CreditCard_ValidateWithDefaultFormatter_ReturnsValidResult()
        {
            // Arrange
            var model = new CreditCardModel
            {
                CardNumber = "4532015112830366", // Valid Visa
                PaymentCard = "4000000000000002", // Valid test card
                OptionalCard = "5555555555554444" // Valid MasterCard
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }
    }
}
