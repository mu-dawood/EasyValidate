using System;

using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PastDateAttribute : ValidationAttributeBase
    {
        public override string ErrorCode => "PastDateValidationError";

        public AttributeResult Validate(string propertyName, DateTime value)
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
