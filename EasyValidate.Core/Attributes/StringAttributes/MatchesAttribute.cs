using System.Text.RegularExpressions;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string matches the specified regular expression pattern.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Matches(@"^\d{3}-\d{3}-\d{4}$")]
    ///     public string PhoneNumber { get; set; } // Valid: "123-456-7890", Invalid: "1234567890"
    ///     
    ///     [Matches(@"^[A-Z]{2}\d{4}$")]
    ///     public string ProductCode { get; set; } // Valid: "AB1234", Invalid: "abc1234", "AB12345"
    /// }
    /// </code>
    /// </example>
    public class MatchesAttribute(string pattern) : StringValidationAttributeBase
    {
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsInvalid.
        /// </summary>

        /// <summary>
        /// The regular expression pattern to match.
        /// </summary>
        public string Pattern { get; } = pattern;

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "MatchesValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must match the specified pattern.";

        /// Arguments propertyName, Pattern

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {
            bool isValid = !string.IsNullOrWhiteSpace(value) && Regex.IsMatch(value, Pattern);
            return new AttributeResult<string>(isValid, value , propertyName, Pattern);
        }
    }
}