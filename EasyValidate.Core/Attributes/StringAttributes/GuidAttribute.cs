using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string is a valid GUID (Globally Unique Identifier).
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Guid]
    ///     public string UserId { get; set; } // Valid: "550e8400-e29b-41d4-a716-446655440000", Invalid: "invalid-guid"
    ///     
    ///     [Guid]
    ///     public string SessionId { get; set; } // Valid: "6ba7b810-9dad-11d1-80b4-00c04fd430c8", Invalid: "123456789"
    /// }
    /// </code>
    /// </example>
    public class GuidAttribute : StringValidationAttributeBase
    {
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "GuidValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be a valid GUID.";

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {
            bool isValid = !string.IsNullOrWhiteSpace(value) && System.Guid.TryParse(value, out _);
            return new AttributeResult<string>(isValid, value , propertyName);
        }
    }
}
