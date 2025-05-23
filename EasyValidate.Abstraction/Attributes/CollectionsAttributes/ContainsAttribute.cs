using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ContainsAttribute<T>(T expectedValue) : CollectionValidationAttributeBase<T>
    {
        public T ExpectedValue { get; } = expectedValue;

        public override string ErrorCode => "ContainsValidationError";

        protected override AttributeResult ValidateCollection(string propertyName, IEnumerable<T> value)
        {
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
