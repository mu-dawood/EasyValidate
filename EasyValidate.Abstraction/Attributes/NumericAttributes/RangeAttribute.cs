using System;

namespace EasyValidate.Abstraction.Attributes.NumericAttributes
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

        public AttributeResult Validate(string propertyName, double value)
        {
            if (value < Minimum || value > Maximum)
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