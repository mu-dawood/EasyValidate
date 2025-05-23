using System;

using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
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
                    MessageArgs = [propertyName]
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}