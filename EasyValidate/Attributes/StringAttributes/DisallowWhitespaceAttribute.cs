using System;
using System.Linq;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string does not contain any whitespace characters.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DisallowWhitespaceAttribute : StringValidationAttributeBase
    {
        public override string ErrorCode => "DisallowWhitespaceValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (!string.IsNullOrEmpty(value) && value.Any(char.IsWhiteSpace))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must not contain whitespace characters.",
                    MessageArgs = new object?[] { propertyName }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
