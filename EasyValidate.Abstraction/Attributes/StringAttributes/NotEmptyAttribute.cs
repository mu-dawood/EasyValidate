using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotEmptyAttribute : ValidationAttributeBase
    {
        public override string ErrorCode => "NotEmptyValidationError";

        public AttributeResult Validate(string propertyName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} cannot be empty.",
                    MessageArgs = [propertyName]
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}