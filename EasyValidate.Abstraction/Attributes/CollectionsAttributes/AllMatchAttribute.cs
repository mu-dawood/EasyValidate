using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class AllMatchAttribute<T>(Func<T, bool> predicate) : ValidationAttributeBase
    {
        public Func<T, bool> Predicate { get; } = predicate;

        public override string ErrorCode => "AllMatchValidationError";

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

            if (!value.All(Predicate))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must have all items matching the specified condition.",
                    MessageArgs = [propertyName]
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
