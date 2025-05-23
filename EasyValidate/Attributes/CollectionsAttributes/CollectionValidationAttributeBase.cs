using System;
using System.Collections.Generic;

using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public abstract class CollectionValidationAttributeBase<T> : ValidationAttributeBase
    {
        // Common validation logic for IEnumerable<T> properties
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

        protected abstract AttributeResult ValidateCollection(string propertyName, IEnumerable<T> value);
    }
}
