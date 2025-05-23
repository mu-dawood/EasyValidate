using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a numeric value has at most a specified number of digits.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MaxDigitsAttribute(int maxDigits) : NumericValidationAttributeBase
    {
        public int MaxDigits { get; } = maxDigits;

        public override string ErrorCode => "MaxDigitsValidationError";

        /// <inheritdoc/>
        public override AttributeResult ValidateNumber(string propertyName, decimal value)
        {
            int digits = value == 0 ? 1 : (int)Math.Floor(Math.Log10(Math.Abs((double)value)) + 1);
            if (digits > MaxDigits)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must have at most {1} digits.",
                    MessageArgs = [propertyName, MaxDigits]
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
