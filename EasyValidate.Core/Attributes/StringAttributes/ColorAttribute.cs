using System.Text.RegularExpressions;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string is a valid color in hexadecimal format.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Color]
    ///     public string BackgroundColor { get; set; } // Valid: "#FF0000", "#abc", "#123456", Invalid: "red", "FF0000", "#GG0000"
    ///     
    ///     [Color]
    ///     public string BorderColor { get; set; } // Valid: "#000", "#ffffff", Invalid: "#12345", "blue"
    /// }
    /// </code>
    /// </example>
    public class ColorAttribute : StringValidationAttributeBase
    {
        private static readonly Regex ColorRegex = new(
            "^#(?:[0-9a-fA-F]{3}){1,2}$",
            RegexOptions.Compiled);

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "ColorValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be a valid hexadecimal color.";

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {
            bool isValid = string.IsNullOrWhiteSpace(value) || ColorRegex.IsMatch(value);
            return new AttributeResult<string>(isValid, value , propertyName);
        }
    }
}
