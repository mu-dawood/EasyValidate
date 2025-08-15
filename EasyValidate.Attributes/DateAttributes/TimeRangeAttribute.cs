using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a time falls within a specified range.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [TimeRange("09:00:00", "17:00:00")]
    ///     public DateTime WorkingHours { get; set; } // Valid: 2024-01-01 14:30:00, Invalid: 2024-01-01 20:00:00
    ///     
    ///     [TimeRange("18:00:00", "23:59:59")]
    ///     public DateTime EveningEvent { get; set; } // Valid: 2024-01-01 19:30:00, Invalid: 2024-01-01 10:00:00
    /// }
    /// </code>
    /// </example>
    public class TimeRangeAttribute : DateValidationAttributeBase
    {
        /// <inheritdoc/>
        public TimeRangeAttribute(string minimum, string maximum)
        {
            if (string.IsNullOrWhiteSpace(minimum))
                throw new ArgumentException("Minimum time cannot be null or empty.", nameof(minimum));
            if (string.IsNullOrWhiteSpace(maximum))
                throw new ArgumentException("Maximum time cannot be null or empty.", nameof(maximum));

            if (!TimeSpan.TryParse(minimum, out TimeSpan minTime))
                throw new ArgumentException("Invalid format for minimum time.", nameof(minimum));
            if (!TimeSpan.TryParse(maximum, out TimeSpan maxTime))
                throw new ArgumentException("Invalid format for maximum time.", nameof(maximum));
            if (minTime > maxTime)
                throw new ArgumentException("Minimum time must be less than or equal to maximum time.");
            Minimum = minTime;
            Maximum = maxTime;
        }

        /// <inheritdoc/>
        public TimeRangeAttribute(TimeSpan minimum, TimeSpan maximum)
        {
            if (minimum > maximum)
                throw new ArgumentException("Minimum time must be less than or equal to maximum time.");

            Minimum = minimum;
            Maximum = maximum;
        }
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsInvalid.
        /// </summary>

        /// <summary>
        /// The minimum allowed time (inclusive).
        /// </summary>
        public TimeSpan Minimum { get; }

        /// <summary>
        /// The maximum allowed time (inclusive).
        /// </summary>
        public TimeSpan Maximum { get; }


        private string _errorCode = "TimeRangeValidationError";
        /// <inheritdoc/>
        public override string ErrorCode
        {
            get => _errorCode;
            set => _errorCode = value;
        }

        private AttributeResult ValidateTime(string propertyName, TimeSpan time)
        {
            if (time >= Minimum && time <= Maximum)
                return AttributeResult.Success();
            return AttributeResult.Fail("The field {0} must be between {1} and {2}.", propertyName, Minimum, Maximum);
        }

        /// <summary>
        /// Validates a DateTime value for allowed time range.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            return ValidateTime(propertyName, value.TimeOfDay);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Validates a DateOnly value for allowed time range (always valid, as DateOnly has no time).
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateOnly value)
        {
            return ValidateTime(propertyName, TimeSpan.MinValue);
        }
#endif

        /// <summary>
        /// Validates a DateTimeOffset value for allowed time range.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTimeOffset value)
        {
            return ValidateTime(propertyName, value.TimeOfDay);
        }
    }
}
