using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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
    public class NotContainsAttribute : StringValidationAttributeBase
    {
        /// <summary>
        /// The substring that must not be present.
        /// </summary>
        public string Substring { get; }
        public NotContainsAttribute(string substring)
        {
            Substring = substring;
        }

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "StringNotContainsValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, string value, out string output)
        {
            bool isValid = string.IsNullOrEmpty(value) || !value.Contains(Substring);
            output = value;
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must not contain '{1}'.", propertyName, Substring);
        }
    }
}
