using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ExactLengthAttribute(int length) : ValidationAttributeBase
    {
        public int Length { get; } = length;

        public override string ErrorCode => "ExactLengthValidationError";

        public AttributeResult Validate(string propertyName, string value)
        {
            if (value == null || value.Length != Length)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be exactly {1} characters long.",
                    MessageArgs = [propertyName, Length]
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
