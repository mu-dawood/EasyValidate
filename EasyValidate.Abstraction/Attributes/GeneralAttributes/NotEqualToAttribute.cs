using System;

namespace EasyValidate.Abstraction.Attributes.GeneralAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotEqualToAttribute : ValidationAttributeBase
    {
        public object ComparisonValue { get; }

        public NotEqualToAttribute(object comparisonValue)
        {
            ComparisonValue = comparisonValue;
        }

        public override string ErrorCode => "NotEqualToValidationError";

        public AttributeResult Validate(string propertyName, object value)
        {
            if (Equals(value, ComparisonValue))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must not be equal to {1}.",
                    MessageArgs = new object[] { propertyName, ComparisonValue }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}