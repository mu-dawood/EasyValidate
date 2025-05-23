using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a numeric value has at least a specified number of digits.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MinDigitsAttribute(int minDigits) : NumericValidationAttributeBase
    {
        public int MinDigits { get; } = minDigits;

        public override string ErrorCode => "MinDigitsValidationError";

        /// <inheritdoc/>
        public override AttributeResult ValidateNumber(string propertyName, decimal value)
        {
            int digits = value == 0 ? 1 : (int)Math.Floor(Math.Log10(Math.Abs((double)value)) + 1);
            if (digits < MinDigits)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must have at least {1} digits.",
                    MessageArgs = [propertyName, MinDigits]
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
