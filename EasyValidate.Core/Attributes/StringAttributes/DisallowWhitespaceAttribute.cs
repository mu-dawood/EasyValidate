using System;
using System.Linq;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string does not contain any whitespace characters.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [DisallowWhitespace]
    ///     public string Username { get; set; } // Valid: "john_doe", "user123", Invalid: "john doe"
    ///     
    ///     [DisallowWhitespace]
    ///     public string ApiKey { get; set; } // Valid: "abc123def456", Invalid: "abc 123 def"
    /// }
    /// </code>
    /// </example>
    public class DisallowWhitespaceAttribute : StringValidationAttributeBase
    {
        private static readonly Lazy<DisallowWhitespaceAttribute> _instance = new(() => new DisallowWhitespaceAttribute());
        public static DisallowWhitespaceAttribute Instance => _instance.Value;
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "DisallowWhitespaceValidationError";

        /// Arguments propertyName

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, string value, out string output)
        {
            bool isValid = string.IsNullOrEmpty(value) || !value.Any(char.IsWhiteSpace);
            output = value;
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must not contain whitespace characters.", propertyName);
        }
    }
}
