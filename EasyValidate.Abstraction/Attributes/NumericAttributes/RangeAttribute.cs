using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RangeAttribute : ValidationAttributeBase
    {
        public double Minimum { get; }
        public double Maximum { get; }

        public RangeAttribute(double minimum, double maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public override string ErrorCode => "RangeValidationError";

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

        // Generic validation logic moved to private helper
        private AttributeResult ValidateGeneric<T>(string propertyName, T value) where T : IComparable<T>
        {
            if (!NumericHelper.IsNumericType(value))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be a numeric type.",
                    MessageArgs = new object[] { propertyName }
                };
            }

            if (value.CompareTo((T)Convert.ChangeType(Minimum, typeof(T))) < 0 || value.CompareTo((T)Convert.ChangeType(Maximum, typeof(T))) > 0)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be between {1} and {2}.",
                    MessageArgs = new object[] { propertyName, Minimum, Maximum }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}