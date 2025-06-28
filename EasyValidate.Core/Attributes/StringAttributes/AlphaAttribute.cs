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
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "AlphaValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must contain only alphabetic characters.";

        /// Arguments propertyName

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {
            bool isValid = !string.IsNullOrWhiteSpace(value) && IsAlpha(value!);
            return new AttributeResult<string>(isValid, value, propertyName);
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
