using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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
        public TimeRangeAttribute(string minimum, string maximum)
        {
            if (string.IsNullOrWhiteSpace(minimum))
                throw new ArgumentException("Minimum time cannot be null or empty.", nameof(minimum));
            if (string.IsNullOrWhiteSpace(maximum))
                throw new ArgumentException("Maximum time cannot be null or empty.", nameof(maximum));

            if (!TimeSpan.TryParse(minimum, out _))
                throw new ArgumentException("Invalid format for minimum time.", nameof(minimum));
            if (!TimeSpan.TryParse(maximum, out _))
                throw new ArgumentException("Invalid format for maximum time.", nameof(maximum));
        }

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

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "TimeRangeValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The field {0} must be between {1} and {2}.";

        /// Arguments propertyName, Minimum, Maximum

        /// <inheritdoc/>
        protected override AttributeResult<DateTime> ValidateUtc(object obj, string propertyName, DateTime value)
        {
            var time = value.TimeOfDay;
            bool isValid = time >= Minimum && time <= Maximum;
            return new AttributeResult<DateTime>(isValid, value, propertyName);
        }
    }
}
