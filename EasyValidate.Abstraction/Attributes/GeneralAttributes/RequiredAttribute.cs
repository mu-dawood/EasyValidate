using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RequiredAttribute : ValidationAttributeBase
    {
        public override string ErrorCode => "RequiredValidationError";

        public AttributeResult Validate(string propertyName, object value)
        {
            if (value == null || (value is string str && string.IsNullOrWhiteSpace(str)))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} is required.",
                    MessageArgs = new object[] { propertyName }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}