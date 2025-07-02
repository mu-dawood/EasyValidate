using System;
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
        private static readonly Lazy<AsciiAttribute> _instance = new(() => new AsciiAttribute());
        public static AsciiAttribute Instance => _instance.Value;
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "AsciiValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, string value, out string output)
        {        
            bool valid = string.IsNullOrEmpty(value) || !value.Any(c => c > 127);
            output = value;
            return valid ? AttributeResult.Success() : AttributeResult.Fail("The field {0} must contain only ASCII characters.", propertyName);
        }
    }
}
