using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a date falls on one of the specified days of the week.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [DayOfWeek(DayOfWeek.Monday, DayOfWeek.Tuesday, DayOfWeek.Wednesday, DayOfWeek.Thursday, DayOfWeek.Friday)]
    ///     public DateTime WorkDay { get; set; } // Valid: weekdays, Invalid: Saturday, Sunday
    ///     
    ///     [DayOfWeek(DayOfWeek.Saturday, DayOfWeek.Sunday)]
    ///     public DateTime WeekendEvent { get; set; } // Valid: weekend days, Invalid: weekdays
    /// }
    /// </code>
    /// </example>
    public class DayOfWeekAttribute(params DayOfWeek[] days) : DateValidationAttributeBase
    {
        /// <summary>
        /// The array of allowed days of the week.
        /// </summary>
        public DayOfWeek[] Days { get; } = days;

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "DayOfWeekValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be one of the following days: {1}.";

        /// <inheritdoc/>
        protected override AttributeResult<DateTime> ValidateUtc(object obj, string propertyName, DateTime value)
        {
            bool isValid = Array.Exists(Days, day => day == value.DayOfWeek);
            return new AttributeResult<DateTime>(isValid, value, propertyName, string.Join(", ", Days));
        }
    }
}
