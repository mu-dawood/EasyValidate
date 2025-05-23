using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a numeric value is a multiple of a specified number.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MultipleOfAttribute(double factor) : NumericValidationAttributeBase
    {
        public decimal Factor { get; } = (decimal)factor;

        public override string ErrorCode => "MultipleOfValidationError";

        /// <inheritdoc/>
        public override AttributeResult ValidateNumber(string propertyName, decimal value)
        {
            if (Factor == 0 || value % Factor != 0)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be a multiple of {1}.",
                    MessageArgs = [propertyName, Factor]
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
