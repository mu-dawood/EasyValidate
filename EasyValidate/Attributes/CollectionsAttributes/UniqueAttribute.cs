using System;
using System.Collections.Generic;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that all values in the collection are unique.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class UniqueAttribute<T> : CollectionValidationAttributeBase<T>
    {
        /// <inheritdoc/>
        public override string ErrorCode => "UniqueValidationError";

        /// <inheritdoc/>
        protected override AttributeResult ValidateCollection(string propertyName, IEnumerable<T> collection)
        {
            var seen = new HashSet<T>();
            foreach (var item in collection)
            {
                if (!seen.Add(item))
                {
                    return new AttributeResult
                    {
                        IsValid = false,
                        Message = "The field {0} must contain only unique values.",
                        MessageArgs = [propertyName]
                    };
                }
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
