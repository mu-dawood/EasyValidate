using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a numeric value is odd.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class OddNumberAttribute : NumericValidationAttributeBase
    {
        public override string ErrorCode => "OddNumberValidationError";

        /// <inheritdoc/>
        public override AttributeResult ValidateNumber(string propertyName, decimal value)
        {
            if (value % 2 == 0)
            {
                return new AttributeResult { IsValid = false, Message = "The field {0} must be an odd number.", MessageArgs = new object?[] { propertyName } };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
