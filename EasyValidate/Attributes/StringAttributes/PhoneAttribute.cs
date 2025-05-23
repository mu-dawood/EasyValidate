using System;
using System.Text.RegularExpressions;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string is a valid phone number (basic international/US format check).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PhoneAttribute : StringValidationAttributeBase
    {
        public override string ErrorCode => "PhoneValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, @"^\+?[0-9 .\-()]{7,}$"))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be a valid phone number.",
                    MessageArgs = new object?[] { propertyName }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}