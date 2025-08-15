using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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

        /// <summary>
        /// Gets or sets the nullable behavior for this attribute.
        /// </summary>


        public override string ErrorCode { get; set; } = "AlphaNumericValidationError";


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
            bool isValid = !string.IsNullOrEmpty(value) && IsAlphaNumeric(value!);
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
