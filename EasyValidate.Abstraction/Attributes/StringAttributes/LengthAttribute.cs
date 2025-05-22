using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class LengthAttribute : ValidationAttributeBase
    {
        public int MinimumLength { get; }
        public int MaximumLength { get; }

        public LengthAttribute(int minimumLength, int maximumLength)
        {
            MinimumLength = minimumLength;
            MaximumLength = maximumLength;
        }

        public override string ErrorCode => "LengthValidationError";

        public AttributeResult Validate(string propertyName, string value)
        {
            if (value == null || value.Length < MinimumLength || value.Length > MaximumLength)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be between {1} and {2} characters long.",
                    MessageArgs = new object[] { propertyName, MinimumLength, MaximumLength }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}