using System;

namespace EasyValidate.Abstraction.Attributes.StringAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ContainsAttribute : ValidationAttributeBase
    {
        public string Substring { get; }

        public ContainsAttribute(string substring)
        {
            Substring = substring;
        }

        public override string ErrorCode => "ContainsValidationError";

        public AttributeResult Validate(string propertyName, string value)
        {
            if (value == null || !value.Contains(Substring))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must contain {1}.",
                    MessageArgs = new object[] { propertyName, Substring }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
