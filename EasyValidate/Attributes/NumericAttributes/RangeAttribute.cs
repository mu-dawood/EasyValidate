using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    public enum RangeBoundary
    {
        Inclusive,
        Exclusive,
        InclusiveMinExclusiveMax,
        ExclusiveMinInclusiveMax
    }

    /// <summary>
    /// Validates that a numeric value is within a specified range, with configurable inclusivity/exclusivity for each bound.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RangeAttribute(double minimum, double maximum, RangeBoundary boundary = RangeBoundary.Inclusive) : NumericValidationAttributeBase
    {
        public double Minimum { get; } = minimum;
        public double Maximum { get; } = maximum;
        public RangeBoundary Boundary { get; } = boundary;

        public override string ErrorCode => "RangeValidationError";

        /// <inheritdoc/>
        public override AttributeResult ValidateNumber(string propertyName, decimal value)
        {
            bool valid = Boundary switch
            {
                RangeBoundary.Inclusive => value >= (decimal)Minimum && value <= (decimal)Maximum,
                RangeBoundary.Exclusive => value > (decimal)Minimum && value < (decimal)Maximum,
                RangeBoundary.InclusiveMinExclusiveMax => value >= (decimal)Minimum && value < (decimal)Maximum,
                RangeBoundary.ExclusiveMinInclusiveMax => value > (decimal)Minimum && value <= (decimal)Maximum,
                _ => value >= (decimal)Minimum && value <= (decimal)Maximum
            };
            if (!valid)
            {
                string boundaryMsg = Boundary switch
                {
                    RangeBoundary.Inclusive => "inclusive",
                    RangeBoundary.Exclusive => "exclusive",
                    RangeBoundary.InclusiveMinExclusiveMax => "inclusive minimum, exclusive maximum",
                    RangeBoundary.ExclusiveMinInclusiveMax => "exclusive minimum, inclusive maximum",
                    _ => "inclusive"
                };
                return new AttributeResult
                {
                    IsValid = false,
                    Message = $"The field {{0}} must be between {{1}} and {{2}} ({{3}}).",
                    MessageArgs = new object?[] { propertyName, Minimum, Maximum, boundaryMsg }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}