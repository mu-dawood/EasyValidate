using System;
using System.Collections.Generic;

namespace EasyValidate.Abstraction.Attributes.GeneralAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CompositeAttribute : ValidationAttributeBase
    {
        private readonly List<ValidationAttributeBase> _attributes;

        public CompositeAttribute(params ValidationAttributeBase[] attributes)
        {
            _attributes = new List<ValidationAttributeBase>(attributes);
        }

        public override string ErrorCode => "CompositeValidationError";

        public AttributeResult Validate(string propertyName, object value)
        {
            foreach (var attribute in _attributes)
            {
                var result = attribute.Validate(propertyName, value);
                if (!result.IsValid)
                {
                    return result;
                }
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
