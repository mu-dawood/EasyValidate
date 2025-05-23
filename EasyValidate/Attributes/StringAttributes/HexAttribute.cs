using System;
using System.Linq;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string contains only hexadecimal characters (0-9, a-f, A-F).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class HexAttribute : StringValidationAttributeBase
    {
        public override string ErrorCode => "HexValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (!string.IsNullOrEmpty(value) && value.Any(c => !IsHexChar(c)))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must contain only hexadecimal characters (0-9, a-f, A-F).",
                    MessageArgs = new object?[] { propertyName }
                };
            }
            return new AttributeResult { IsValid = true };
        }

        private static bool IsHexChar(char c)
        {
            return (c >= '0' && c <= '9') ||
                   (c >= 'a' && c <= 'f') ||
                   (c >= 'A' && c <= 'F');
        }
    }
}
