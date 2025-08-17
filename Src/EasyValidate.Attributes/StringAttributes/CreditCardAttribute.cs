using System;
using System.Text.RegularExpressions;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string is a valid credit card number (basic Luhn check).
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [CreditCard]
    ///     public string CardNumber { get; set; } // Valid: "4532015112830366", "4000000000000002", Invalid: "1234567890123456"
    ///     
    ///     [CreditCard]
    ///     public string PaymentCard { get; set; } // Valid: valid Luhn algorithm numbers, Invalid: random digits
    /// }
    /// </code>
    /// </example>
    public partial class CreditCardAttribute : StringValidationAttributeBase
    {


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "CreditCardValidationError";


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
            bool isValid = !string.IsNullOrWhiteSpace(value) && IsCreditCard(value!);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be a valid credit card number.", propertyName);
        }

#if NET7_0_OR_GREATER
        [GeneratedRegex("^\\d{13,19}$", RegexOptions.Compiled)]
        private static partial Regex MyRegex();
#else
        private static Regex MyRegex() => new Regex("^\\d{13,19}$", RegexOptions.Compiled);
#endif
        private static bool IsCreditCard(string number)
        {
            number = number.Replace(" ", "").Replace("-", "");
            if (!MyRegex().IsMatch(number)) return false;
            int sum = 0;
            bool alternate = false;
            for (int i = number.Length - 1; i >= 0; i--)
            {
                int n = number[i] - '0';
                if (alternate)
                {
                    n *= 2;
                    if (n > 9) n -= 9;
                }
                sum += n;
                alternate = !alternate;
            }
            return sum % 10 == 0;
        }
    }
}