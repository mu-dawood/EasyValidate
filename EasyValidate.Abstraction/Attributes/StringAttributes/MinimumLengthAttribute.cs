using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MinimumLengthAttribute : ValidationAttributeBase
    {
        public int MinimumLength { get; }

        public MinimumLengthAttribute(int minimumLength)
        {
            MinimumLength = minimumLength;
        }

        public override string ErrorCode => "MinimumLengthValidationError";

        public AttributeResult Validate(string propertyName, string value)
        {
            if (value == null || value.Length < MinimumLength)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be at least {1} characters long.",
                    MessageArgs = new object[] { propertyName, MinimumLength }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}