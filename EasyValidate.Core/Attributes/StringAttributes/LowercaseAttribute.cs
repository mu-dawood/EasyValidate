using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string is all lowercase.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Lowercase]
    ///     public string Username { get; set; } // Valid: "john_doe", "user123", Invalid: "John_Doe", "USER123"
    ///     
    ///     [Lowercase]
    ///     public string EmailPrefix { get; set; } // Valid: "john.doe", "admin", Invalid: "John.Doe", "ADMIN"
    /// }
    /// </code>
    /// </example>
    public class LowercaseAttribute : StringValidationAttributeBase
    {
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "LowercaseValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be lowercase.";

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {
            bool valid = value == value.ToLowerInvariant();
            return new AttributeResult<string>(valid, value, propertyName);
        }
    }
}
