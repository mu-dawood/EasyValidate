using System;
using System.Linq;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string contains only hexadecimal characters (0-9, a-f, A-F).
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Hex]
    ///     public string ColorCode { get; set; } // Valid: "FF0000", "1a2b3c", "ABCDEF", Invalid: "GG0000", "xyz123"
    ///     
    ///     [Hex]
    ///     public string HashValue { get; set; } // Valid: "abc123", "DEF456", Invalid: "xyz789", "hello"
    /// }
    /// </code>
    /// </example>
    public class HexAttribute : StringValidationAttributeBase
    {
        private static readonly Lazy<HexAttribute> _instance = new(() => new HexAttribute());
        public static HexAttribute Instance => _instance.Value;
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "HexValidationError";

        /// Arguments propertyName

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, string value, out string output)
        {
            bool isValid = !string.IsNullOrWhiteSpace(value) && value.All(c => IsHexChar(c));
            output = value;
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be a valid hexadecimal value.", propertyName);
        }

        private static bool IsHexChar(char c)
        {
            return (c >= '0' && c <= '9') ||
                   (c >= 'a' && c <= 'f') ||
                   (c >= 'A' && c <= 'F');
        }
    }
}
