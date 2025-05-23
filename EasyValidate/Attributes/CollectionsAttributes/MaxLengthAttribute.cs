using System;
using System.Collections.Generic;
using System.Linq;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that the collection does not exceed a maximum number of elements.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MaxLengthAttribute<T>(int maximum) : CollectionValidationAttributeBase<T>
    {
        /// <summary>
        /// The maximum allowed length of the collection.
        /// </summary>
        public int Maximum { get; } = maximum;

        /// <inheritdoc/>
        public override string ErrorCode => "MaxLengthValidationError";

        /// <inheritdoc/>
        protected override AttributeResult ValidateCollection(string propertyName, IEnumerable<T> value)
        {
            if (value.Count() > Maximum)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must not exceed a length of {1}.",
                    MessageArgs = [propertyName, Maximum]
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
