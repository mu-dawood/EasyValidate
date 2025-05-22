using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class LengthAttribute<T>(int minimum, int maximum) : ValidationAttributeBase
    {
        public int Minimum { get; } = minimum;
        public int Maximum { get; } = maximum;

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
            if (count < Minimum || count > Maximum)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must have a length between {1} and {2}.",
                    MessageArgs = [propertyName, Minimum, Maximum]
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
