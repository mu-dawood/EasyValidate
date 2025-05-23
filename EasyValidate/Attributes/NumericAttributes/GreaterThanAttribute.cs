using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a numeric value is greater than a specified comparison value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class GreaterThanAttribute(double comparisonValue) : NumericValidationAttributeBase
    {
        public double ComparisonValue { get; } = comparisonValue;

        public override string ErrorCode => "GreaterThanValidationError";

        /// <inheritdoc/>
        public override AttributeResult ValidateNumber(string propertyName, decimal value)
        {
            if (value <= (decimal)ComparisonValue)
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