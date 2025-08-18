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
    public class MatchesAttribute(
#if NET7_0_OR_GREATER
		[System.Diagnostics.CodeAnalysis.StringSyntax(System.Diagnostics.CodeAnalysis.StringSyntaxAttribute.Regex)]
#endif
        string pattern,
        RegexOptions options = RegexOptions.Compiled | RegexOptions.IgnoreCase
    ) : StringValidationAttributeBase
    {

        Regex? _regex;
        private Regex GetRegex()
        {
            _regex ??= new Regex(pattern, options, TimeSpan.FromSeconds(2.0));
            return _regex;
        }

        private static RegexOptions Normalize(RegexOptions opts)
        {
            // Add CultureInvariant for speed + determinism unless caller opted out
            if ((opts & RegexOptions.CultureInvariant) == 0)
                opts |= RegexOptions.CultureInvariant;
            return opts;
        }

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "MatchesValidationError";


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
            if (value is null)
            {
                return AttributeResult.Fail("The {0} field cannot be null.", propertyName);
            }
            Match m = this.GetRegex().Match(value);
            bool isValid = m.Success && m.Index == 0 && m.Length == value.Length;
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must match the pattern '{1}'.", propertyName, pattern);
        }
    }
}