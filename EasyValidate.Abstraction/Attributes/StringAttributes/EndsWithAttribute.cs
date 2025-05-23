using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EndsWithAttribute(string suffix) : ValidationAttributeBase
    {
        public string Suffix { get; } = suffix;

        public override string ErrorCode => "EndsWithValidationError";

        public AttributeResult Validate(string propertyName, string value)
        {
            if (value == null || !value.EndsWith(Suffix))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must end with {1}.",
                    MessageArgs = [propertyName, Suffix]
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
