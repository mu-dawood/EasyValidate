using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date falls within a specified range.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateRangeAttribute(string minimum, string maximum) : DateValidationAttributeBase
    {
        /// <summary>
        /// The minimum allowed date (inclusive).
        /// </summary>
        public DateTime Minimum { get; } = DateTime.Parse(minimum);
        /// <summary>
        /// The maximum allowed date (inclusive).
        /// </summary>
        public DateTime Maximum { get; } = DateTime.Parse(maximum);

        /// <inheritdoc/>
        public override string ErrorCode => "DateRangeValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            if (value < Minimum || value > Maximum)
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
