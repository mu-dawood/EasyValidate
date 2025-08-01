using EasyValidate.Test.Extensions;
using System.Linq;
namespace EasyValidate.Test.Strings.Hex
{
    public class HexSimpleTests
    {
        [Fact]
        public void Hex_ValidHexString_ReturnsValidResult()
        {
            // Arrange
            var model = new HexModel
            {
                ColorCode = "FF0000", // Valid hex color
                HashValue = "1a2b3c", // Valid hex hash
                OptionalHex = "ABCDEF" // Valid hex optional
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void Hex_InvalidHexString_ReturnsInvalidResult()
        {
            // Arrange
            var model = new HexModel
            {
                ColorCode = "GG0000", // Invalid due to 'GG'
                HashValue = "1a2b3c",
                OptionalHex = "ABCDEF"
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.ColorCode)));
            Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.HashValue)));
            Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalHex)));
            var colorCodeErrors = result.GetAllErrors().Where(e => e.PropertyName.Contains(nameof(model.ColorCode))).ToList();
            Assert.Single(colorCodeErrors);
            Assert.Contains("must be a valid hexadecimal value", colorCodeErrors[0].FormattedMessage);
        }

        [Fact]
        public void Hex_MixedCaseValidHex_ReturnsValidResult()
        {
            // Arrange
            var model = new HexModel
            {
                ColorCode = "AbCdEf",
                HashValue = "123ABC",
                OptionalHex = "deadBEEF"
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void Hex_EmptyOptionalString_ReturnsValidResult()
        {
            // Arrange
            var model = new HexModel
            {
                ColorCode = "FF0000",
                HashValue = "123456",
                OptionalHex = null
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.True(result.IsValid());
            Assert.False(result.HasErrors());
        }

        [Fact]
        public void Hex_InvalidCharacters_ReturnsInvalidResult()
        {
            // Arrange
            var model = new HexModel
            {
                ColorCode = "xyz123", // Invalid due to 'xyz'
                HashValue = "hello", // Invalid due to 'hello'
                OptionalHex = "FF00A1"
            };

            // Act
            var result = model.Validate();

            // Assert
            Assert.False(result.IsValid());
            Assert.True(result.HasErrors());
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.ColorCode)));
            Assert.Contains(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.HashValue)));
            Assert.DoesNotContain(result.GetAllErrors(), e => e.PropertyName.Contains(nameof(model.OptionalHex)));
        }
    }
}
