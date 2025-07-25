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
        public static readonly Lazy<CommonPrintableAttribute> Instance = new(() => new CommonPrintableAttribute());
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "CommonPrintableValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, string value)
        {
            bool valid = !value.Any(c => !IsCommonPrintable(c));
            return valid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must contain only common printable characters.", propertyName);
        }

        private static bool IsCommonPrintable(char c)
        {
            // Letters, digits, punctuation, and space
            return char.IsLetterOrDigit(c) || char.IsPunctuation(c) || c == ' ';
        }
    }
}
