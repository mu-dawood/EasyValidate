using System;
using System.Text.RegularExpressions;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string is a valid credit card number (basic Luhn check).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CreditCardAttribute : StringValidationAttributeBase
    {
        public override string ErrorCode => "CreditCardValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || !IsValidCreditCard(value!))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be a valid credit card number.",
                    MessageArgs = [propertyName]
                };
            }
            return new AttributeResult { IsValid = true };
        }

        private static bool IsValidCreditCard(string number)
        {
            number = number.Replace(" ", "").Replace("-", "");
            if (!Regex.IsMatch(number, "^\\d{13,19}$")) return false;
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