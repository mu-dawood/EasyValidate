using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MaxLengthAttribute<T> : ValidationAttributeBase
    {
        public int Maximum { get; }

        public MaxLengthAttribute(int maximum)
        {
            Maximum = maximum;
        }

        public override string ErrorCode => "MaxLengthValidationError";

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
