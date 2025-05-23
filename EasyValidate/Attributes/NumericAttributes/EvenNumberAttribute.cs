using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a numeric value is even.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EvenNumberAttribute : NumericValidationAttributeBase
    {
        public override string ErrorCode => "EvenNumberValidationError";

        /// <inheritdoc/>
        public override AttributeResult ValidateNumber(string propertyName, decimal value)
        {
            if (value % 2 != 0)
            {
                return new AttributeResult { IsValid = false, Message = "The field {0} must be an even number.", MessageArgs = new object?[] { propertyName } };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
