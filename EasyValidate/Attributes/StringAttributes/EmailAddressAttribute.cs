using System;
using System.Text.RegularExpressions;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string is a valid email address.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EmailAddressAttribute : StringValidationAttributeBase
    {
        public override string ErrorCode => "EmailAddressValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be a valid email address.",
                    MessageArgs = [propertyName]
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}