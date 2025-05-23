using System;
using System.Collections.Generic;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SingleOrNoneAttribute<T>(T value) : CollectionValidationAttributeBase<T>
    {
        public T Value { get; } = value;

        public override string ErrorCode => "SingleOrNoneValidationError";

        protected override AttributeResult ValidateCollection(string propertyName, IEnumerable<T> value)
        {
            bool found = false;
            foreach (var item in value)
            {
                if (EqualityComparer<T>.Default.Equals(item, Value))
                {
                    if (found)
                    {
                        return new AttributeResult
                        {
                            IsValid = false,
                            Message = $"The collection '{propertyName}' must contain the value '{Value}' at most once.",
                            MessageArgs = [propertyName, Value]
                        };
                    }
                    found = true;
                }
            }
            // Valid if not found (empty or value not present) or found exactly once
            return new AttributeResult { IsValid = true };
        }
    }
}
