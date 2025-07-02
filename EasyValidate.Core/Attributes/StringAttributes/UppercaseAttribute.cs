using System;
using System.Linq;
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
        public override AttributeResult Validate(object obj, string propertyName, string value, out string output)
        {
            bool isValid = string.IsNullOrEmpty(value) || value.All(char.IsUpper);
            output = value;
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be uppercase.", propertyName);
        }
    }
}
