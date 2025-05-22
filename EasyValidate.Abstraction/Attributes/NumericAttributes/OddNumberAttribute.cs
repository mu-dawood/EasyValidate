using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class OddNumberAttribute : ValidationAttributeBase
    {
        public override string ErrorCode => "OddNumberValidationError";

        public AttributeResult Validate<T>(string propertyName, T value) where T : struct, IComparable<T>
        {
            if (value is int intValue && intValue % 2 == 0)
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
