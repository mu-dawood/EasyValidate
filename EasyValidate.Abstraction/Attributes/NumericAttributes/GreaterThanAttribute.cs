using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class GreaterThanAttribute : ValidationAttributeBase
    {
        public double ComparisonValue { get; }

        public GreaterThanAttribute(double comparisonValue)
        {
            ComparisonValue = comparisonValue;
        }

        public override string ErrorCode => "GreaterThanValidationError";

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

        // Rename generic Validate to private helper and enforce numeric type
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

            if (value.CompareTo((T)Convert.ChangeType(ComparisonValue, typeof(T))) <= 0)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be greater than {1}.",
                    MessageArgs = [propertyName, ComparisonValue]
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}