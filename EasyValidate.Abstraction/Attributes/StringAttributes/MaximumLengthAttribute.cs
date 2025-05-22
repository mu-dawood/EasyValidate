using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MaximumLengthAttribute : ValidationAttributeBase
    {
        public int MaximumLength { get; }

        public MaximumLengthAttribute(int maximumLength)
        {
            MaximumLength = maximumLength;
        }

        public override string ErrorCode => "MaximumLengthValidationError";

        public AttributeResult Validate(string propertyName, string value)
        {
            if (value != null && value.Length > MaximumLength)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be at most {1} characters long.",
                    MessageArgs = new object[] { propertyName, MaximumLength }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}