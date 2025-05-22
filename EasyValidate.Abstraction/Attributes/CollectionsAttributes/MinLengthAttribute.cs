using System;
using System.Collections.Generic;
using System.Linq;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MinLengthAttribute<T> : ValidationAttributeBase
    {
        public int Minimum { get; }

        public MinLengthAttribute(int minimum)
        {
            Minimum = minimum;
        }

        public override string ErrorCode => "MinLengthValidationError";

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
