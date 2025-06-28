using System.Linq;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string contains only ASCII characters (0x00-0x7F).
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Ascii]
    ///     public string Code { get; set; } // Valid: "ABC123", "hello@world.com", Invalid: "café", "naïve", "Москва"
    ///     
    ///     [Ascii]
    ///     public string SystemPath { get; set; } // Valid: "/usr/bin/app", "C:\\Windows", Invalid: "/home/用户"
    /// }
    /// </code>
    /// </example>
    public class AsciiAttribute : StringValidationAttributeBase
    {
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "AsciiValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The field {0} must contain only ASCII characters.";

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                bool isValid = true;
                return new AttributeResult<string>(isValid, value, propertyName);
            }
                
            bool valid = !value.Any(c => c > 127);
            return new AttributeResult<string>(valid, value!, propertyName);
        }
    }
}
