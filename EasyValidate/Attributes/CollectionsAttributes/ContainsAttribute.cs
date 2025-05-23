using System;
using System.Collections.Generic;
using System.Linq;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a collection contains the specified value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ContainsAttribute<T>(T expectedValue) : CollectionValidationAttributeBase<T>
    {
        /// <summary>
        /// The value that must be present in the collection.
        /// </summary>
        public T ExpectedValue { get; } = expectedValue;

        /// <inheritdoc/>
        public override string ErrorCode => "ContainsValidationError";

        /// <inheritdoc/>
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
