using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date is today or in the past (not in the future).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotInFutureAttribute : DateValidationAttributeBase
    {
        public override string ErrorCode => "NotInFutureValidationError";

        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            if (value > DateTime.Now)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must not be in the future.",
                    MessageArgs = new object?[] { propertyName }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
