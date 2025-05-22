using System;
using System.Text.RegularExpressions;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EmailAddressAttribute : ValidationAttributeBase
    {
        private const string EmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";

        public override string ErrorCode => "EmailAddressValidationError";

        public AttributeResult Validate(string propertyName, string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, EmailPattern))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be a valid email address.",
                    MessageArgs = new object[] { propertyName }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}