using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date is in the past.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PastDateAttribute : DateValidationAttributeBase
    {
        /// <inheritdoc/>
        public override string ErrorCode => "PastDateValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            if (value >= DateTime.Now)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be a past date.",
                    MessageArgs = [propertyName]
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
