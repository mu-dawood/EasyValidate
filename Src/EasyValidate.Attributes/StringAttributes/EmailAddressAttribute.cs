using System;
using System.Text.RegularExpressions;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string is a valid email address.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [EmailAddress]
    ///     public string Email { get; set; } // Valid: "user@example.com", "test.email@domain.org", Invalid: "invalid-email", "user@"
    ///     
    ///     [EmailAddress]
    ///     public string ContactEmail { get; set; } // Valid: "john.doe@company.com", Invalid: "john.doe", "@company.com"
    /// }
    /// </code>
    /// </example>
    public partial class EmailAddressAttribute : StringValidationAttributeBase
    {


#if NET7_0_OR_GREATER
        [GeneratedRegex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", RegexOptions.Compiled | RegexOptions.IgnoreCase)]
        private static partial Regex MyRegex();
#else
        private static readonly Regex _regex = new Regex(@"^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?(?:\.[a-zA-Z0-9](?:[a-zA-Z0-9-]{0,61}[a-zA-Z0-9])?)*$", RegexOptions.Compiled | RegexOptions.IgnoreCase);
        private static Regex MyRegex() => _regex;
#endif
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute.
        /// </summary>


        public override string ErrorCode { get; set; } = "EmailAddressValidationError";


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
            // Fast path: Quick checks before expensive regex
            if (string.IsNullOrWhiteSpace(value))
                return AttributeResult.Fail("The {0} field must be a valid email address.", propertyName);

            // Quick length and basic character checks
            if (value.Length < 5 || value.Length > 254) // RFC 5321 limit
                return AttributeResult.Fail("The {0} field must be a valid email address.", propertyName);

            // Quick @ symbol validation
            int atIndex = value.IndexOf('@');
            if (atIndex <= 0 || atIndex == value.Length - 1 || value.IndexOf('@', atIndex + 1) != -1)
                return AttributeResult.Fail("The {0} field must be a valid email address.", propertyName);

            // Quick dot after @ check
            if (value.IndexOf('.', atIndex) == -1)
                return AttributeResult.Fail("The {0} field must be a valid email address.", propertyName);

            // Only use regex for final validation after quick checks pass
            bool isValid = MyRegex().IsMatch(value);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be a valid email address.", propertyName);
        }
    }
}