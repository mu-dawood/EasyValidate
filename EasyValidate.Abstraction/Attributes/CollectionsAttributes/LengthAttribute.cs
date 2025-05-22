using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class LengthAttribute<T>(int length) : ValidationAttributeBase
    {
        public int Length { get; } = length;

        public override string ErrorCode => "LengthValidationError";

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
