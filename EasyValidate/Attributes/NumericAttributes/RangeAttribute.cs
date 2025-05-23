using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a numeric value is within a specified range (inclusive).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RangeAttribute : NumericValidationAttributeBase
    {
        public double Minimum { get; }
        public double Maximum { get; }

        public RangeAttribute(double minimum, double maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public override string ErrorCode => "RangeValidationError";

        /// <inheritdoc/>
        public override AttributeResult ValidateNumber(string propertyName, decimal value)
        {
            if (value < (decimal)Minimum || value > (decimal)Maximum)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be between {1} and {2}.",
                    MessageArgs = new object?[] { propertyName, Minimum, Maximum }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}