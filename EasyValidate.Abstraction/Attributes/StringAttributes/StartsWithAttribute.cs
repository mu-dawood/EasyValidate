using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class StartsWithAttribute(string prefix) : ValidationAttributeBase
    {
        public string Prefix { get; } = prefix;

        public override string ErrorCode => "StartsWithValidationError";

        public AttributeResult Validate(string propertyName, string value)
        {
            if (value == null || !value.StartsWith(Prefix))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must start with {1}.",
                    MessageArgs = [propertyName, Prefix]
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
