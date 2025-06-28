using System.Linq;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string does not contain any whitespace.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [NoWhitespace]
    ///     public string ApiKey { get; set; } // Valid: "abc123xyz", "TOKEN456", Invalid: "abc 123", "TOKEN 456"
    ///     
    ///     [NoWhitespace]
    ///     public string Identifier { get; set; } // Valid: "user_id_123", "ITEM-001", Invalid: "user id 123", "ITEM 001"
    /// }
    /// </code>
    /// </example>
    public class NoWhitespaceAttribute : StringValidationAttributeBase
    {
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NoWhitespaceValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must not contain any whitespace.";

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {
            bool isValid = !value.Any(char.IsWhiteSpace);
            return new AttributeResult<string>(isValid, value, propertyName);
        }
    }
}
