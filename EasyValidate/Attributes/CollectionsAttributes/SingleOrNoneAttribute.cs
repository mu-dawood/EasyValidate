using System;
using System.Collections.Generic;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that the specified value appears at most once (zero or one time) in the collection.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class SingleOrNoneAttribute<T>(T value) : CollectionValidationAttributeBase<T>
    {
        /// <summary>
        /// The value that must appear at most once in the collection.
        /// </summary>
        public T Value { get; } = value;

        /// <inheritdoc/>
        public override string ErrorCode => "SingleOrNoneValidationError";

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
                            Message = "The field {0} must contain the value {1} at most once.",
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
