using System;
using System.Collections;

namespace EasyValidate.Abstraction.Attributes.GeneralAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class UniqueAttribute : ValidationAttributeBase
    {
        public override string ErrorCode => "UniqueValidationError";

        public AttributeResult Validate(string propertyName, IEnumerable collection, object value)
        {
            foreach (var item in collection)
            {
                if (item.Equals(value))
                {
                    return new AttributeResult
                    {
                        IsValid = false,
                        Message = "The field {0} must be unique.",
                        MessageArgs = new object[] { propertyName }
                    };
                }
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
