using System;
using System.Linq;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string contains only common printable characters (letters, digits, punctuation, and space).
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [CommonPrintable]
    ///     public string DisplayName { get; set; } // Valid: "John Doe", "User-123", Invalid: "Text\nWithNewline"
    ///     
    ///     [CommonPrintable]
    ///     public string Description { get; set; } // Valid: "Product: ABC-123!", Invalid: "Text\tWithTab"
    /// }
    /// </code>
    /// </example>
    public class CommonPrintableAttribute : StringValidationAttributeBase
    {
        private static readonly Lazy<CommonPrintableAttribute> _instance = new(() => new CommonPrintableAttribute());
        public static CommonPrintableAttribute Instance => _instance.Value;
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "CommonPrintableValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, string value, out string output)
        {  
            bool valid = !value.Any(c => !IsCommonPrintable(c));
            output = value;
            return valid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must contain only common printable characters.", propertyName);
        }

        private static bool IsCommonPrintable(char c)
        {
            // Letters, digits, punctuation, and space
            return char.IsLetterOrDigit(c) || char.IsPunctuation(c) || c == ' ';
        }
    }
}
