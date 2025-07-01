using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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
        private static readonly Lazy<UppercaseAttribute> _instance = new(() => new UppercaseAttribute());
        public static UppercaseAttribute Instance => _instance.Value;
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "UppercaseValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be uppercase.";

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {               
            bool valid = value == value.ToUpperInvariant();
            return new AttributeResult<string>(valid, value, propertyName);
        }
    }
}
