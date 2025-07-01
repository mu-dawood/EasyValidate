using System;
using System.Text.RegularExpressions;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string is a valid ISBN-10 or ISBN-13.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [ISBN]
    ///     public string BookCode { get; set; } // Valid: "1234567890", "123456789X", "1234567890123", Invalid: "123456789", "abcd567890"
    ///     
    ///     [ISBN]
    ///     public string ProductISBN { get; set; } // Valid: "0123456789", "012345678X", Invalid: "01234567890"
    /// }
    /// </code>
    /// </example>
    public class ISBNAttribute : StringValidationAttributeBase
    {
        private static readonly Lazy<ISBNAttribute> _instance = new(() => new ISBNAttribute());
        public static ISBNAttribute Instance => _instance.Value;
        private static readonly Regex IsbnRegex = new(
            @"^(?:\d{9}[\dXx]|\d{13})$",
            RegexOptions.Compiled);

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "ISBNValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be a valid ISBN-10 or ISBN-13.";

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {
            bool valid = IsbnRegex.IsMatch(value);
            return new AttributeResult<string>(valid, value, propertyName);
        }
    }
}
