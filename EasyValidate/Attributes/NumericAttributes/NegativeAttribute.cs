using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a numeric value is negative (less than zero).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NegativeAttribute : NumericValidationAttributeBase
    {
        public override string ErrorCode => "NegativeValidationError";

        /// <inheritdoc/>
        public override AttributeResult ValidateNumber(string propertyName, decimal value)
        {
            if (value >= 0)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be negative.",
                    MessageArgs = new object?[] { propertyName }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
