using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string ends with the specified suffix.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [EndsWith(".com")]
    ///     public string Email { get; set; } // Valid: "user@example.com", Invalid: "user@example.org"
    ///     
    ///     [EndsWith(".pdf")]
    ///     public string DocumentPath { get; set; } // Valid: "document.pdf", Invalid: "document.txt"
    /// }
    /// </code>
    /// </example>
    public class EndsWithAttribute(string suffix) : StringValidationAttributeBase
    {
        /// <summary>
        /// The required suffix.
        /// </summary>
        public string Suffix { get; } = suffix;


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "EndsWithValidationError";

        /// <summary>
        /// The comparison type to use when checking for the substring.
        /// Defaults to <see cref="StringComparison.Ordinal"/>.
        /// </summary>
        public StringComparison Comparison { get; set; } = StringComparison.Ordinal;


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
            bool isValid = value.EndsWith(Suffix, Comparison);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must end with '{1}'.", propertyName, Suffix);
        }
    }
}
