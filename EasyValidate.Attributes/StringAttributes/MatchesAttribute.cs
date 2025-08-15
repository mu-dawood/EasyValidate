using System;
using System.Text.RegularExpressions;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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
    public class MatchesAttribute(string pattern, RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase) : StringValidationAttributeBase
    {

        /// <summary>
        /// The regular expression pattern to match.
        /// </summary>
        public Regex Pattern { get; } = new Regex(pattern, options, TimeSpan.FromSeconds(1.0));


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "MatchesValidationError";


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
            if (value is null)
            {
                return AttributeResult.Fail("The {0} field cannot be null.", propertyName);
            }
            bool isValid = Pattern.IsMatch(value!);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must match the pattern '{1}'.", propertyName, Pattern);
        }
    }
}