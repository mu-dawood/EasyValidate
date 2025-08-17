using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string does not contain a specific substring.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [NotContains("admin")]
    ///     public string Username { get; set; } // Valid: "user123", "guest", Invalid: "admin", "administrator"
    ///     
    ///     [NotContains("test")]
    ///     public string ProductionCode { get; set; } // Valid: "prod_v1", "release", Invalid: "test_code", "testing"
    /// }
    /// </code>
    /// </example>
    public class NotContainsAttribute(string substring) : StringValidationAttributeBase
    {
        /// <summary>
        /// The substring that must not be present.
        /// </summary>
        public string Substring { get; } = substring;


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "StringNotContainsValidationError";

        /// <summary>
        /// The comparison type to use when checking for the substring.
        /// Defaults to <see cref="StringComparison.Ordinal"/>.
        /// </summary>
        public StringComparison Comparison { get; set; } = StringComparison.Ordinal;


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
#if NET5_0_OR_GREATER
            bool isValid = string.IsNullOrEmpty(value) || !value.Contains(Substring, Comparison);
#else
            bool isValid = string.IsNullOrEmpty(value) || value!.IndexOf(Substring, Comparison) == 0;
#endif
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must not contain '{1}'.", propertyName, Substring);
        }
    }
}
