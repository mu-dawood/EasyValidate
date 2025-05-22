using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DistinctByAttribute<T, TKey>(Expression<Func<T, TKey>> keySelector) : ValidationAttributeBase
    {
        public Expression<Func<T, TKey>> KeySelector { get; } = keySelector;

        public override string ErrorCode => "DistinctByValidationError";

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

            var compiledKeySelector = KeySelector.Compile();
            var seenValues = new HashSet<TKey>();
            foreach (var item in value)
            {
                var key = compiledKeySelector(item);
                if (!seenValues.Add(key))
                {
                    return new AttributeResult
                    {
                        IsValid = false,
                        Message = "The field {0} must have distinct values based on the specified key selector.",
                        MessageArgs = [propertyName]
                    };
                }
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
