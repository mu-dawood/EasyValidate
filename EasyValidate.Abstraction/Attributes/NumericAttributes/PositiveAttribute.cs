using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PositiveAttribute : ValidationAttributeBase
    {
        public override string ErrorCode => "PositiveValidationError";

        public AttributeResult Validate<T>(string propertyName, T value) where T : IComparable<T>
        {
            if (value.CompareTo(default(T)) <= 0)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be positive.",
                    MessageArgs = new object[] { propertyName }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
