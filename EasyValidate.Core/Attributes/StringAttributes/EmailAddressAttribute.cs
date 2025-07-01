using System;
using System.Text.RegularExpressions;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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
    public class EmailAddressAttribute : StringValidationAttributeBase
    {
        private static readonly Lazy<EmailAddressAttribute> _instance = new(() => new EmailAddressAttribute());
        public static EmailAddressAttribute Instance => _instance.Value;
        private static readonly Regex _emailRegex = new(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.Compiled | RegexOptions.CultureInvariant);

        /// <summary>
        /// Gets or sets the nullable behavior for this attribute.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "EmailAddressValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be a valid email address.";

        /// Arguments propertyName

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {
            bool isValid = !string.IsNullOrWhiteSpace(value) && _emailRegex.IsMatch(value);
            return new AttributeResult<string>(isValid, value, propertyName);
        }
    }
}