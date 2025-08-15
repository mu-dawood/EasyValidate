using System;
using System.Text.RegularExpressions;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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
    public partial class ISBNAttribute : StringValidationAttributeBase
    {


#if NET7_0_OR_GREATER
        [GeneratedRegex(@"^(?:\d{9}[\dXx]|\d{13})$", RegexOptions.Compiled)]
        private static partial Regex MyRegex();
#else
        private static Regex MyRegex() => new Regex(@"^(?:\d{9}[\dXx]|\d{13})$", RegexOptions.Compiled);
#endif


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "ISBNValidationError";


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
            bool isValid = !string.IsNullOrWhiteSpace(value) && IsISBN(value!);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be a valid ISBN.", propertyName);
        }

        private static bool IsISBN(string value)
        {
            return MyRegex().IsMatch(value);
        }
    }
}
