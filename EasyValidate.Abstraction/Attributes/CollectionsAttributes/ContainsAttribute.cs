using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ContainsAttribute<T>(T expectedValue) : ValidationAttributeBase
    {
        public T ExpectedValue { get; } = expectedValue;

        public override string ErrorCode => "ContainsValidationError";

        public AttributeResult Validate(string propertyName, IEnumerable<T> value)
        {
            if (value == null)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} cannot be null.",
                    MessageArgs = [propertyName]
                };
            }

            if (!value.Contains(ExpectedValue))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must contain the value {1}.",
                    MessageArgs = [propertyName, ExpectedValue]
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
