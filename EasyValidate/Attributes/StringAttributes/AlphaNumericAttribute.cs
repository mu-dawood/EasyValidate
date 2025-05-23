using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string contains only alphanumeric characters (A-Z, a-z, 0-9).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class AlphaNumericAttribute : StringValidationAttributeBase
    {
        public override string ErrorCode => "AlphaNumericValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || !IsAlphaNumeric(value!))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must contain only alphanumeric characters.",
                    MessageArgs = new object?[] { propertyName }
                };
            }
            return new AttributeResult { IsValid = true };
        }

        private static bool IsAlphaNumeric(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsLetterOrDigit(c))
                    return false;
            }
            return true;
        }
    }
}
