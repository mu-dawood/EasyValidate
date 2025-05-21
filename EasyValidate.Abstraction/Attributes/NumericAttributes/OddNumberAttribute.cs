using System;

namespace EasyValidate.Abstraction.Attributes.NumericAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class OddNumberAttribute : ValidationAttributeBase
    {
        public override string ErrorCode => "OddNumberValidationError";

        public AttributeResult Validate(string propertyName, int value)
        {
            if (value % 2 == 0)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be an odd number.",
                    MessageArgs = new object[] { propertyName }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
