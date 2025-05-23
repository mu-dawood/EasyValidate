using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date is in the future.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class FutureDateAttribute : DateValidationAttributeBase
    {
        /// <inheritdoc/>
        public override string ErrorCode => "FutureDateValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            if (value <= DateTime.Now)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be a future date.",
                    MessageArgs = new object?[] { propertyName }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
