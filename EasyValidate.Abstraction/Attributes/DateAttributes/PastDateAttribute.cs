using System;

namespace EasyValidate.Abstraction.Attributes.DateAttributes
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
                    MessageArgs = new object[] { propertyName }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
