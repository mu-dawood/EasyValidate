using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a numeric value is greater than or equal to a specified comparison value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class GreaterThanOrEqualToAttribute : NumericValidationAttributeBase
    {
        public double ComparisonValue { get; }

        public GreaterThanOrEqualToAttribute(double comparisonValue)
        {
            ComparisonValue = comparisonValue;
        }

        public override string ErrorCode => "GreaterThanOrEqualToValidationError";

        /// <inheritdoc/>
        public override AttributeResult ValidateNumber(string propertyName, decimal value)
        {
            if (value < (decimal)ComparisonValue)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be greater than or equal to {1}.",
                    MessageArgs = new object?[] { propertyName, ComparisonValue }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}