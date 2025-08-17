using EasyValidate.Abstractions;

using EasyValidate.Test.Extensions;
using System.Linq;
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
                    HashValue = "FF00A1", // Valid
                    OptionalHex = "GHIJKL" // Invalid
                }
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.False(result.Property(nameof(model.PrimaryHex))?.HasErrors());
            Assert.True(result.Property("NestedHex")?.HasErrors("ColorCode"));
            Assert.False(result.Property("NestedHex")?.HasErrors("HashValue"));
            Assert.True(result.Property("NestedHex")?.HasErrors("OptionalHex"));

            var nestedColorErrors = result.GetAllErrors().Where(e => e.PropertyName == "NestedHex.ColorCode").ToList();
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
            var result = model.Validate((conf) => conf.SetFormatter(formatter));

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());

            var colorCodeErrors = result.GetAllErrors().Where(e => e.PropertyName == nameof(model.ColorCode)).ToList();
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
                    OptionalHex = "FF00A1" // Valid
                }
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.True(result.Property(nameof(model.PrimaryHex))?.HasErrors());
            Assert.True(result.Property("NestedHex")?.HasErrors("ColorCode"));
            Assert.True(result.Property("NestedHex")?.HasErrors("HashValue"));
            Assert.False(result.Property("NestedHex")?.HasErrors("OptionalHex"));
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
            public string Format<T>(AttributeResult result, T value)
            {
                return $"CUSTOM: {string.Format(result.MessageTemplate, result.MessageArgs)}";
            }
        }
    }
}
