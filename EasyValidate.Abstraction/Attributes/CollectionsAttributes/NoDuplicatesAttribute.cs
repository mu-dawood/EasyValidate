using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DistinctAttribute<T> : ValidationAttributeBase
    {
        public override string ErrorCode => "NoDuplicatesValidationError";

        public AttributeResult Validate(string propertyName, IEnumerable<T> value)
        {
            if (value == null)
            {
                throw new InvalidOperationException($"The property '{propertyName}' must be of type IEnumerable to use the NoDuplicatesAttribute.");
            }

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
