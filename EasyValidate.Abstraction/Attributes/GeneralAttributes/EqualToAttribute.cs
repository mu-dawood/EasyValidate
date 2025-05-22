using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EqualToAttribute : ValidationAttributeBase
    {
        public object ComparisonValue { get; }

        public EqualToAttribute(object comparisonValue)
        {
            ComparisonValue = comparisonValue;
        }

        public override string ErrorCode => "EqualToValidationError";

        public AttributeResult Validate(string propertyName, object value)
        {
            if (!Equals(value, ComparisonValue))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be equal to {1}.",
                    MessageArgs = new object[] { propertyName, ComparisonValue }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}