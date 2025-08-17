using System;
using System.Linq;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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
        public override AttributeResult Validate(string propertyName, string value)
        {
            bool isValid = !string.IsNullOrWhiteSpace(value) && !value.Any(char.IsWhiteSpace);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must not contain any whitespace.", propertyName);
        }
    }
}
