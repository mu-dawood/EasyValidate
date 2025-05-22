using System;
using System.Text.RegularExpressions;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CreditCardAttribute : ValidationAttributeBase
    {
        private const string CreditCardPattern = @"^(\d{4}[- ]?){3}\d{4}$";

        public override string ErrorCode => "CreditCardValidationError";

        public AttributeResult Validate(string propertyName, string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, CreditCardPattern))
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
    }
}