using System;
using System.Collections.Generic;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SingleAttribute<T>(T value) : CollectionValidationAttributeBase<T>
    {
        public T Value { get; } = value;

        public override string ErrorCode => "SingleValidationError";

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
                            Message = $"The collection '{{propertyName}}' must contain the value '{{Value}}' exactly once.",
                            MessageArgs = new object?[] { propertyName, Value }
                        };
                    }
                    found = true;
                }
            }
            if (found)
                return new AttributeResult { IsValid = true };
            return new AttributeResult
            {
                IsValid = false,
                Message = $"The collection '{{propertyName}}' must contain the value '{{Value}}' exactly once.",
                MessageArgs = new object?[] { propertyName, Value }
            };
        }
    }
}
