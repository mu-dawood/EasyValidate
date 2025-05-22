using System;

namespace EasyValidate.Abstraction.Attributes.GeneralAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotNullAttribute : ValidationAttributeBase
    {
        public override string ErrorCode => "NotNullValidationError";

        public AttributeResult Validate(string propertyName, object value)
        {
            if (value == null)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} cannot be null.",
                    MessageArgs = new object[] { propertyName }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}