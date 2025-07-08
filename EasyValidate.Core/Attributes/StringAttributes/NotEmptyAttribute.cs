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
        public static readonly Lazy<NotEmptyAttribute> Instance = new(() => new NotEmptyAttribute());
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NotEmptyValidationError";

        /// Arguments propertyName

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, string value)
        {
            bool isValid = !string.IsNullOrWhiteSpace(value);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must not be empty.", propertyName);
        }
    }
}