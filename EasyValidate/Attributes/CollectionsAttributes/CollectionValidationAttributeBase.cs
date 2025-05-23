using System;
using System.Collections.Generic;

using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Base attribute for collection validation attributes. Provides common validation logic for IEnumerable{T} properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public abstract class CollectionValidationAttributeBase<T> : ValidationAttributeBase
    {
        /// <summary>
        /// Validates the collection property value.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="value">The collection value to validate.</param>
        /// <returns>An <see cref="AttributeResult"/> indicating the result of validation.</returns>
        public AttributeResult Validate(string propertyName, IEnumerable<T> value)
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
            return ValidateCollection(propertyName, value);
        }

        /// <summary>
        /// When implemented in a derived class, validates the collection value.
        /// </summary>
        protected abstract AttributeResult ValidateCollection(string propertyName, IEnumerable<T> value);
    }
}
