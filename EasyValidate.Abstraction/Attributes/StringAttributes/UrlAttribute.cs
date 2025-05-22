using System;
using System.Text.RegularExpressions;

namespace EasyValidate.Abstraction.Attributes.StringAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class UrlAttribute : ValidationAttributeBase
    {
        private const string UrlPattern = @"^(https?:\/\/)?([\w\-]+\.)+[\w\-]+(\/[\w\-\._~:\/?#[\]@!$&'()*+,;=]*)?$";

        public override string ErrorCode => "UrlValidationError";

        public AttributeResult Validate(string propertyName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new AttributeResult { IsValid = true };
            }

            if (Regex.IsMatch(value, UrlPattern))
            {
                return new AttributeResult { IsValid = true };
            }

            return new AttributeResult
            {
                IsValid = false,
                Message = "The field {0} must be a valid URL.",
                MessageArgs = new object[] { propertyName }
            };
        }
    }
}