using System;
using System.Collections.Generic;
using System.Linq;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that the collection has exactly the specified number of elements.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class LengthAttribute<T>(int length) : CollectionValidationAttributeBase<T>
    {
        /// <summary>
        /// The required length of the collection.
        /// </summary>
        public int Length { get; } = length;

        /// <inheritdoc/>
        public override string ErrorCode => "LengthValidationError";

        /// <inheritdoc/>
        protected override AttributeResult ValidateCollection(string propertyName, IEnumerable<T> value)
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

            int count = value.Count();
            if (count != Length)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must have exactly {1} items.",
                    MessageArgs = [propertyName, Length]
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
