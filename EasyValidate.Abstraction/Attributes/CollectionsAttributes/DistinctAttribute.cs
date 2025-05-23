using System;
using System.Collections.Generic;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DistinctAttribute<T> : CollectionValidationAttributeBase<T>
    {
        public override string ErrorCode => "NoDuplicatesValidationError";

        protected override AttributeResult ValidateCollection(string propertyName, IEnumerable<T> value)
        {
            var seenItems = new HashSet<T>();
            foreach (var item in value)
            {
                if (!seenItems.Add(item))
                {
                    return new AttributeResult
                    {
                        IsValid = false,
                        Message = $"The collection '{propertyName}' contains duplicate values.",
                        MessageArgs = [propertyName]
                    };
                }
            }
            return new AttributeResult { IsValid = true };
        }
    }

}
