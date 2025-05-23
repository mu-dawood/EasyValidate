using System;
using System.Collections.Generic;
using System.Linq;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that the collection has at least a minimum number of elements.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MinLengthAttribute<T>(int minimum) : CollectionValidationAttributeBase<T>
    {
        /// <summary>
        /// The minimum required length of the collection.
        /// </summary>
        public int Minimum { get; } = minimum;

        /// <inheritdoc/>
        public override string ErrorCode => "MinLengthValidationError";

        /// <inheritdoc/>
        protected override AttributeResult ValidateCollection(string propertyName, IEnumerable<T> value)
        {
            if (value.Count() < Minimum)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must have a minimum length of {1}.",
                    MessageArgs = new object?[] { propertyName, Minimum }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
