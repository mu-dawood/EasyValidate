using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date is today or in the future (not in the past).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotInPastAttribute : DateValidationAttributeBase
    {
        public override string ErrorCode => "NotInPastValidationError";

        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            if (value < DateTime.Today)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must not be in the past.",
                    MessageArgs = new object?[] { propertyName }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
