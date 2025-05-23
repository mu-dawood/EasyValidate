using System;
using System.Text.RegularExpressions;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string is a valid URL.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class UrlAttribute : StringValidationAttributeBase
    {
        public override string ErrorCode => "UrlValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || !Uri.IsWellFormedUriString(value, UriKind.Absolute))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be a valid URL.",
                    MessageArgs = [propertyName]
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}