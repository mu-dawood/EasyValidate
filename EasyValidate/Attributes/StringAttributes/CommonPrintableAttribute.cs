using System;
using System.Linq;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string contains only common printable characters (letters, digits, punctuation, and space).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CommonPrintableAttribute : StringValidationAttributeBase
    {
        public override string ErrorCode => "CommonPrintableValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (!string.IsNullOrEmpty(value) && value.Any(c => !IsCommonPrintable(c)))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must contain only common printable characters (letters, digits, punctuation, and space).",
                    MessageArgs = [propertyName]
                };
            }
            return new AttributeResult { IsValid = true };
        }

        private static bool IsCommonPrintable(char c)
        {
            // Letters, digits, punctuation, and space
            return char.IsLetterOrDigit(c) || char.IsPunctuation(c) || c == ' ';
        }
    }
}
