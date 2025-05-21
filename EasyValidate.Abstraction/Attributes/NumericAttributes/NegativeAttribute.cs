using System;

namespace EasyValidate.Abstraction.Attributes.NumericAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NegativeAttribute : ValidationAttributeBase
    {
        public override string ErrorCode => "NegativeValidationError";

        public AttributeResult Validate(string propertyName, double value)
        {
            if (value >= 0)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be negative.",
                    MessageArgs = new object[] { propertyName }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
