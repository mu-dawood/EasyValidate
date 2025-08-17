using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string contains a specific substring.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Contains("@")]
    ///     public string Email { get; set; } // Valid: "user@example.com", Invalid: "userexample.com"
    ///     
    ///     [Contains("API")]
    ///     public string EndpointUrl { get; set; } // Valid: "https://api.example.com", Invalid: "https://example.com"
    /// }
    /// </code>
    /// </example>
    public class ContainsAttribute(string substring) : StringValidationAttributeBase
    {
        /// <summary>
        /// The substring that must be present.
        /// </summary>
        public string Substring { get; } = substring;


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "ContainsValidationError";

        /// <summary>
        /// The comparison type to use when checking for the substring.
        /// Defaults to <see cref="StringComparison.Ordinal"/>.
        /// </summary>
        public StringComparison Comparison { get; set; } = StringComparison.Ordinal;

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
#if NET5_0_OR_GREATER
            bool isValid = value.Contains(Substring, Comparison);
#else
            bool isValid = value!.IndexOf(Substring, Comparison) >= 0;
#endif
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must contain '{1}'.", propertyName, Substring);
        }
    }
}
