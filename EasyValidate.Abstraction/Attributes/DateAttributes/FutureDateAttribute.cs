using System;

namespace EasyValidate.Abstraction.Attributes.DateAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class FutureDateAttribute : ValidationAttributeBase
    {
        public override string ErrorCode => "FutureDateValidationError";

        public AttributeResult Validate(string propertyName, DateTime value)
        {
            if (value <= DateTime.Now)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be a future date.",
                    MessageArgs = new object[] { propertyName }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
