using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must end with '{1}'.";
        /// <summary>
        /// The comparison type to use when checking for the substring.
        /// Defaults to <see cref="StringComparison.Ordinal"/>.
        /// </summary>
        public StringComparison Comparison { get; set; } = StringComparison.Ordinal;

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {
            bool isValid = value.EndsWith(Suffix, Comparison);
            return new AttributeResult<string>(isValid, value, propertyName, Suffix);
        }
    }
}
