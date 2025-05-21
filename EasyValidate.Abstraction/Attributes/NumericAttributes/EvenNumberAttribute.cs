using System;

namespace EasyValidate.Abstraction.Attributes.NumericAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EvenNumberAttribute : ValidationAttributeBase
    {
        public override string ErrorCode => "EvenNumberValidationError";

        public AttributeResult Validate(string propertyName, int value)
        {
            if (value % 2 != 0)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be an even number.",
                    MessageArgs = new object[] { propertyName }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
