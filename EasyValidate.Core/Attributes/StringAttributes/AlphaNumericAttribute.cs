using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string contains only alphanumeric characters (A-Z, a-z, 0-9).
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [AlphaNumeric]
    ///     public string Username { get; set; } // Valid: "User123", "JohnDoe1", Invalid: "user_name", "john-doe", "user@123"
    ///     
    ///     [AlphaNumeric]
    ///     public string ProductCode { get; set; } // Valid: "ABC123", "Product001", Invalid: "ABC-123", "Product_001"
    /// }
    /// </code>
    /// </example>
    public class AlphaNumericAttribute : StringValidationAttributeBase
    {
        private static readonly Lazy<AlphaNumericAttribute> _instance = new(() => new AlphaNumericAttribute());
        public static AlphaNumericAttribute Instance => _instance.Value;
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "AlphaNumericValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, string value, out string output)
        {
            bool isValid = string.IsNullOrEmpty(value) || IsAlphaNumeric(value!);
            output = value;
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must contain only alphanumeric characters.", propertyName);
        }

        private static bool IsAlphaNumeric(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsLetterOrDigit(c))
                    return false;
            }
            return true;
        }
    }
}
