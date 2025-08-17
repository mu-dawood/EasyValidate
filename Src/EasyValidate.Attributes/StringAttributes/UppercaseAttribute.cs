using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string is all uppercase.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Uppercase]
    ///     public string CountryCode { get; set; } // Valid: "USA", "UK", Invalid: "usa", "Usa"
    ///     
    ///     [Uppercase]
    ///     public string Status { get; set; } // Valid: "ACTIVE", "PENDING", Invalid: "active", "Active"
    /// }
    /// </code>
    /// </example>
    public class UppercaseAttribute : StringValidationAttributeBase
    {


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "UppercaseValidationError";


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {

            bool isValid = value.ToUpperInvariant().Equals(value, StringComparison.InvariantCulture);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be uppercase.", propertyName);
        }
    }
}
