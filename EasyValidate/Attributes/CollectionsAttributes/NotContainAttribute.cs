using System;
using System.Collections.Generic;
using System.Linq;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a collection does not contain the specified value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotContainAttribute<T>(T forbiddenValue) : CollectionValidationAttributeBase<T>
    {
        /// <summary>
        /// The value that must not be present in the collection.
        /// </summary>
        public T ForbiddenValue { get; } = forbiddenValue;

        /// <inheritdoc/>
        public override string ErrorCode => "DoesNotContainValidationError";

        /// <inheritdoc/>
        protected override AttributeResult ValidateCollection(string propertyName, IEnumerable<T> value)
        {
            if (value.Contains(ForbiddenValue))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must not contain the value {1}.",
                    MessageArgs = new object?[] { propertyName, ForbiddenValue }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
