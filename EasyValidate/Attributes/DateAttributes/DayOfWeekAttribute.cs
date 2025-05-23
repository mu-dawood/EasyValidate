using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date falls on a specific day of the week.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DayOfWeekAttribute(DayOfWeek day) : DateValidationAttributeBase
    {
        /// <summary>
        /// The required day of the week.
        /// </summary>
        public DayOfWeek Day { get; } = day;

        /// <inheritdoc/>
        public override string ErrorCode => "DayOfWeekValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            if (value.DayOfWeek != Day)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must fall on a {1}.",
                    MessageArgs = new object?[] { propertyName, Day }
                };
            }
            return new AttributeResult { IsValid = true };
        }

    }
}
