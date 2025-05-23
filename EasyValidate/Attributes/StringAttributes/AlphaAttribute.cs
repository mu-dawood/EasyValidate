using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string contains only alphabetic characters (A-Z, a-z).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class AlphaAttribute : StringValidationAttributeBase
    {
        public override string ErrorCode => "AlphaValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || !IsAlpha(value!))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must contain only alphabetic characters.",
                    MessageArgs = [propertyName]
                };
            }
            return new AttributeResult { IsValid = true };
        }

        private static bool IsAlpha(string s)
        {
            foreach (char c in s)
            {
                if (!char.IsLetter(c))
                    return false;
            }
            return true;
        }
    }
}
