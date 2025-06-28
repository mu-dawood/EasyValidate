using EasyValidate.Core.Abstraction;

namespace EasyValidate.Test.Strings.Hex
{
    public class HexAdvancedTests
    {
        [Fact]
        public void Hex_NestedObjectValidation_ReturnsCorrectErrors()
        {
            // Arrange
            var model = new HexNestedModel
            {
                PrimaryHex = "ABC123", // Valid
                NestedHex = new HexModel
                {
                    ColorCode = "XYZ000", // Invalid
                    HashValue = "valid123", // Valid
                    OptionalHex = "GHIJKL" // Invalid
                }
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.False(result.HasErrors(nameof(model.PrimaryHex)));
            Assert.True(result.HasErrors("NestedHex.ColorCode"));
            Assert.False(result.HasErrors("NestedHex.HashValue"));
            Assert.True(result.HasErrors("NestedHex.OptionalHex"));

            var nestedColorErrors = result.Errors.Where(e => e.Path.SequenceEqual(new[] { "NestedHex", "ColorCode" })).ToList();
            Assert.Single(nestedColorErrors);
            Assert.Contains("must be a valid hexadecimal value", nestedColorErrors[0].FormattedMessage);
        }

        [Fact]
        public void Hex_CustomFormatter_ReturnsFormattedMessage()
        {
            // Arrange
            var model = new HexModel
            {
                ColorCode = "ZZZZZ", // Invalid
                HashValue = "valid123",
                OptionalHex = null
            };
            var formatter = new TestFormatter();

            // Act
            var result = model.Validate(formatter);

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());

            var colorCodeErrors = result.Errors.Where(e => e.Path.SequenceEqual(new[] { nameof(model.ColorCode) })).ToList();
            Assert.Single(colorCodeErrors);
            Assert.Equal("CUSTOM: The ColorCode field must be a valid hexadecimal value.", colorCodeErrors[0].FormattedMessage);
        }

        [Fact]
        public void Hex_MultipleErrorsInNestedObject_ReturnsAllErrors()
        {
            // Arrange
            var model = new HexNestedModel
            {
                PrimaryHex = "INVALID", // Invalid
                NestedHex = new HexModel
                {
                    ColorCode = "NOTHEX", // Invalid
                    HashValue = "ALSOINVALID", // Invalid
                    OptionalHex = "VALID123" // Valid
                }
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.True(result.HasErrors(nameof(model.PrimaryHex)));
            Assert.True(result.HasErrors("NestedHex.ColorCode"));
            Assert.True(result.HasErrors("NestedHex.HashValue"));
            Assert.False(result.HasErrors("NestedHex.OptionalHex"));
        }

        [Fact]
        public void Hex_NullNestedObject_ReturnsValidResult()
        {
            // Arrange
            var model = new HexNestedModel
            {
                PrimaryHex = "ABC123",
                NestedHex = null
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        private class TestFormatter : IFormatter
        {
            public string Format(string message, params object[] args)
            {
                return $"CUSTOM: {string.Format(message, args)}";
            }

            public string GetFormatedMessage<TInput, TOutput>(IValidationAttribute<TInput, TOutput> attribute, object?[] args)
            {
                return $"CUSTOM: {string.Format(attribute.ErrorMessage, args)}";
            }
        }
    }
}
