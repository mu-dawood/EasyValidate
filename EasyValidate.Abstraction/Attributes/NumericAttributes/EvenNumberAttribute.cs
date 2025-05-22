using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EvenNumberAttribute : ValidationAttributeBase
    {
        public override string ErrorCode => "EvenNumberValidationError";

        public AttributeResult Validate<T>(string propertyName, T value) where T : struct, IComparable<T>
        {
            if (value is int intValue && intValue % 2 != 0)
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
