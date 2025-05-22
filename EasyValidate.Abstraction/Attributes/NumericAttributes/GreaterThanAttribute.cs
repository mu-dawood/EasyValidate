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

        public AttributeResult Validate<T>(string propertyName, T value) where T : IComparable<T>
        {
            if (value.CompareTo((T)Convert.ChangeType(ComparisonValue, typeof(T))) <= 0)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be greater than {1}.",
                    MessageArgs = new object[] { propertyName, ComparisonValue }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}