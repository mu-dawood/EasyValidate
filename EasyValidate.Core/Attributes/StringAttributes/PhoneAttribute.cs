using System;
using System.Text.RegularExpressions;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string is a valid phone number (basic international/US format check).
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Phone]
    ///     public string ContactNumber { get; set; } // Valid: "+1-234-567-8900", "123-456-7890", Invalid: "123456", "abc-def-ghij"
    ///     
    ///     [Phone]
    ///     public string EmergencyContact { get; set; } // Valid: "(555) 123-4567", "+44 20 7946 0958", Invalid: "555"
    /// }
    /// </code>
    /// </example>
    public class PhoneAttribute : StringValidationAttributeBase
    {
        private static readonly Lazy<PhoneAttribute> _instance = new(() => new PhoneAttribute());
        public static PhoneAttribute Instance => _instance.Value;
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsInvalid.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "PhoneValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, string value, out string output)
        {
            bool isValid = string.IsNullOrEmpty(value) || IsPhone(value!);
            output = value;
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be a valid phone number.", propertyName);
        }

        private static bool IsPhone(string value)
        {
            return Regex.IsMatch(value, @"^\+?[0-9 .\-()]{7,}$");
        }
    }
}