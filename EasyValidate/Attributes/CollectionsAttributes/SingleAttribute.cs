using System;
using System.Collections.Generic;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that the specified value appears exactly once in the collection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SingleAttribute<T>(T value) : CollectionValidationAttributeBase<T>
    {
        /// <summary>
        /// The value that must appear exactly once in the collection.
        /// </summary>
        public T Value { get; } = value;

        /// <inheritdoc/>
        public override string ErrorCode => "SingleValidationError";

        /// <inheritdoc/>
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
                            Message = "The field {0} must contain the value {1} exactly once.",
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
                Message = "The field {0} must contain the value {1} exactly once.",
                MessageArgs = new object?[] { propertyName, Value }
            };
        }
    }
}
