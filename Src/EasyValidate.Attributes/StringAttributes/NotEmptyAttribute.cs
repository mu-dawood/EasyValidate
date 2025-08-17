using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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

        /// <summary>
        /// Gets or sets the nullable behavior for this attribute.
        /// </summary>


        public override string ErrorCode { get; set; } = "NotEmptyValidationError";

        /// Arguments propertyName


        public override AttributeResult Validate(string propertyName, string value)
        {
            bool isValid = !string.IsNullOrWhiteSpace(value);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must not be empty.", propertyName);
        }
    }
}