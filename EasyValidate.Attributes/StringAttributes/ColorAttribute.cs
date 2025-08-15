using System;
using System.Text.RegularExpressions;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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
    public partial class ColorAttribute : StringValidationAttributeBase
    {

        private static readonly Regex ColorRegex = MyRegex();


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "ColorValidationError";


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
            bool isValid = !string.IsNullOrWhiteSpace(value) && ColorRegex.IsMatch(value);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be a valid hexadecimal color.", propertyName);
        }

#if NET7_0_OR_GREATER
        [GeneratedRegex("^#(?:[0-9a-fA-F]{3}){1,2}$", RegexOptions.Compiled)]
        private static partial Regex MyRegex();
#else
        private static Regex MyRegex() => new Regex("^#(?:[0-9a-fA-F]{3}){1,2}$", RegexOptions.Compiled);
#endif
    }
}
