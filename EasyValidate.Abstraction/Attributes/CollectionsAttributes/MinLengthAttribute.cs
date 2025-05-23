using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MinLengthAttribute<T>(int minimum) : CollectionValidationAttributeBase<T>
    {
        public int Minimum { get; } = minimum;

        public override string ErrorCode => "MinLengthValidationError";

        protected override AttributeResult ValidateCollection(string propertyName, IEnumerable<T> value)
        {
            if (value.Count() < Minimum)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must have a minimum length of {1}.",
                    MessageArgs = [propertyName, Minimum]
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
