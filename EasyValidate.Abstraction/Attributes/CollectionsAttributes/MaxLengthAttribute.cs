using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MaxLengthAttribute<T>(int maximum) : CollectionValidationAttributeBase<T>
    {
        public int Maximum { get; } = maximum;

        public override string ErrorCode => "MaxLengthValidationError";

        protected override AttributeResult ValidateCollection(string propertyName, IEnumerable<T> value)
        {
            if (value.Count() > Maximum)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must not exceed a length of {1}.",
                    MessageArgs = [propertyName, Maximum]
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
