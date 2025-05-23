using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotContainAttribute<T>(T forbiddenValue) : CollectionValidationAttributeBase<T>
    {
        public T ForbiddenValue { get; } = forbiddenValue;

        public override string ErrorCode => "DoesNotContainValidationError";

        protected override AttributeResult ValidateCollection(string propertyName, IEnumerable<T> value)
        {
            if (value.Contains(ForbiddenValue))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must not contain the value {1}.",
                    MessageArgs = [propertyName, ForbiddenValue]
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
