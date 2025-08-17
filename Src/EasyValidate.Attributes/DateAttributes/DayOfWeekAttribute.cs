using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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


        private string _errorCode = "DayOfWeekValidationError";
        /// <inheritdoc/>
        public override string ErrorCode
        {
            get => _errorCode;
            set => _errorCode = value;
        }

        // ...existing code...

        /// <summary>
        /// Validates a DateTime value for allowed days of the week.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            return ValidateDayOfWeek(propertyName, value.DayOfWeek);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Validates a DateOnly value for allowed days of the week.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateOnly value)
        {
            // DateOnly does not have DayOfWeek, so convert to DateTime
            var dayOfWeek = value.ToDateTime(TimeOnly.MinValue).DayOfWeek;
            return ValidateDayOfWeek(propertyName, dayOfWeek);
        }
#endif

        /// <summary>
        /// Validates a DateTimeOffset value for allowed days of the week.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTimeOffset value)
        {
            return ValidateDayOfWeek(propertyName, value.DayOfWeek);
        }

        private AttributeResult ValidateDayOfWeek(string propertyName, DayOfWeek dayOfWeek)
        {
            if (Array.Exists(Days, d => d == dayOfWeek))
                return AttributeResult.Success();
            return AttributeResult.Fail("The {0} field must be one of the following days: {1}.", propertyName, string.Join(", ", Days));
        }
    }
}
