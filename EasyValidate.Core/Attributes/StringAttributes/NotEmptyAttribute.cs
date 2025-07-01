using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string is not null, empty, or whitespace.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [NotEmpty]
    ///     public string Name { get; set; } // Valid: "John", Invalid: "", "   ", null
    ///     
    ///     [NotEmpty]
    ///     public string Email { get; set; } // Valid: "user@example.com", Invalid: "", null
    /// }
    /// </code>
    /// </example>
    public class NotEmptyAttribute : StringValidationAttributeBase
    {
        private static readonly Lazy<NotEmptyAttribute> _instance = new(() => new NotEmptyAttribute());
        public static NotEmptyAttribute Instance => _instance.Value;
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NotEmptyValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must not be empty.";

        /// Arguments propertyName

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {
            bool isValid = !string.IsNullOrWhiteSpace(value);
            return new AttributeResult<string>(isValid, value , propertyName);
        }
    }
}