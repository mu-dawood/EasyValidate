using System;
using System.Text.RegularExpressions;

using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PhoneAttribute : ValidationAttributeBase
    {
        public override string ErrorCode => "PhoneValidationError";

        public AttributeResult Validate(string propertyName, string value)
        {
            if (string.IsNullOrEmpty(value) || !Regex.IsMatch(value, "^\\+?[1-9]\\d{1,14}$"))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be a valid phone number.",
                    MessageArgs = [propertyName]
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}