using System;
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
        private static readonly Lazy<GuidAttribute> _instance = new(() => new GuidAttribute());
        public static GuidAttribute Instance => _instance.Value;
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "GuidValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, string value, out string output)
        {
            bool isValid = !string.IsNullOrWhiteSpace(value) && Guid.TryParse(value, out _);
            output = value;
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be a valid GUID.", propertyName);
        }
    }
}
