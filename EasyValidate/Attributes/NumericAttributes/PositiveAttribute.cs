using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a numeric value is positive (greater than zero).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PositiveAttribute : NumericValidationAttributeBase
    {
        public override string ErrorCode => "PositiveValidationError";

        /// <inheritdoc/>
        public override AttributeResult ValidateNumber(string propertyName, decimal value)
        {
            if (value <= 0)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be positive.",
                    MessageArgs = [propertyName]
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
