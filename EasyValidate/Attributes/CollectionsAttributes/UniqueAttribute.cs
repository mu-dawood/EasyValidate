using System;
using System.Collections.Generic;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class UniqueAttribute<T> : CollectionValidationAttributeBase<T>
    {
        public override string ErrorCode => "UniqueValidationError";

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
                        Message = $"The collection '{{propertyName}}' must contain only unique values.",
                        MessageArgs = new object?[] { propertyName }
                    };
                }
            }
            return new AttributeResult { IsValid = true };
        }
    }
    
}
