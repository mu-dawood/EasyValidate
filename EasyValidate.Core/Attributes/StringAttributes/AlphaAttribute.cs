using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string contains only alphabetic characters (A-Z, a-z).
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Alpha]
    ///     public string FirstName { get; set; } // Valid: "John", "Mary", Invalid: "John123", "John-Doe", "John_Doe"
    ///     
    ///     [Alpha]
    ///     public string LastName { get; set; } // Valid: "Smith", "Johnson", Invalid: "O'Connor", "Smith-Jones"
    /// }
    /// </code>
    /// </example>
    public class AlphaAttribute : StringValidationAttributeBase
    {
        public static readonly Lazy<AlphaAttribute> Instance = new(() => new AlphaAttribute());

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "AlphaValidationError";

        /// Arguments propertyName

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, string value)
        {
            bool isValid = !string.IsNullOrEmpty(value) && IsAlpha(value!);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must contain only alphabetic characters.", propertyName);
        }

        private static bool IsAlpha(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsLetter(c))
                    return false;
            }
            return true;
        }
    }
}
