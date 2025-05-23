using System;

using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
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
                    MessageArgs = [propertyName]
                };
            }

            return new AttributeResult { IsValid = true };
        }

        // Numeric overloads
        public AttributeResult Validate(string propertyName, byte value) => ValidateGeneric(propertyName, value);
        public AttributeResult Validate(string propertyName, sbyte value) => ValidateGeneric(propertyName, value);
        public AttributeResult Validate(string propertyName, short value) => ValidateGeneric(propertyName, value);
        public AttributeResult Validate(string propertyName, ushort value) => ValidateGeneric(propertyName, value);
        public AttributeResult Validate(string propertyName, int value) => ValidateGeneric(propertyName, value);
        public AttributeResult Validate(string propertyName, uint value) => ValidateGeneric(propertyName, value);
        public AttributeResult Validate(string propertyName, long value) => ValidateGeneric(propertyName, value);
        public AttributeResult Validate(string propertyName, ulong value) => ValidateGeneric(propertyName, value);
        public AttributeResult Validate(string propertyName, float value) => ValidateGeneric(propertyName, value);
        public AttributeResult Validate(string propertyName, double value) => ValidateGeneric(propertyName, value);
        public AttributeResult Validate(string propertyName, decimal value) => ValidateGeneric(propertyName, value);

        // Private generic helper
        private AttributeResult ValidateGeneric<T>(string propertyName, T value) where T : IComparable<T>
        {
            if (!NumericHelper.IsNumericType(value))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be a numeric type.",
                    MessageArgs = [propertyName]
                };
            }

            if (value.CompareTo(default(T)) <= 0)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be positive.",
                    MessageArgs = [propertyName]
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
