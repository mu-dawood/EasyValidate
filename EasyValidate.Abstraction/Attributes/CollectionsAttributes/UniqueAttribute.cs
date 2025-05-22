using System;
using System.Collections;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class UniqueAttribute : ValidationAttributeBase
    {
        private readonly object _uniqueValue;

        public UniqueAttribute(object uniqueValue)
        {
            _uniqueValue = uniqueValue;
        }

        public override string ErrorCode => "UniqueValidationError";

        public AttributeResult Validate(string propertyName, IEnumerable collection)
        {
            int count = 0;

            foreach (var item in collection)
            {
                if (item.Equals(_uniqueValue))
                {
                    count++;
                    if (count > 1)
                    {
                        return new AttributeResult
                        {
                            IsValid = false,
                            Message = "The field {0} must be unique.",
                            MessageArgs = new object[] { propertyName }
                        };
                    }
                }
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
