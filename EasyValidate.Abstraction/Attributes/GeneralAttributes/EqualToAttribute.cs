using System;

namespace EasyValidate.Abstraction.Attributes
{
    public class EqualToAttribute<T>(T comparisonValue) : ValidationAttributeBase
    {
        public T ComparisonValue { get; } = comparisonValue;

        public override string ErrorCode => "EqualToValidationError";

        public AttributeResult Validate(string propertyName, T value)
        {
            if (!Equals(value, ComparisonValue))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be equal to {1}.",
                    MessageArgs = [propertyName, ComparisonValue]
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }

}