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
    public class NotContainsAttribute(string substring) : StringValidationAttributeBase
    {
        /// <summary>
        /// The substring that must not be present.
        /// </summary>
        public string Substring { get; } = substring;

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "StringNotContainsValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must not contain '{1}'.";

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {
            bool valid = !value.Contains(Substring);
            return new AttributeResult<string>(valid, value, propertyName, Substring);
        }
    }
}
