using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date falls within a specified range.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [DateRange("2024-01-01", "2024-12-31")]
    ///     public DateTime EventDate { get; set; } // Valid: 2024-06-15, Invalid: 2025-01-01
    ///     
    ///     [DateRange("2020-01-01", "2030-12-31")]
    ///     public DateTime ProjectDate { get; set; } // Valid: 2025-05-25, Invalid: 2019-12-31
    /// }
    /// </code>
    /// </example>
    public class DateRangeAttribute : DateValidationAttributeBase
    {
        private readonly Func<DateTime, AttributeResult> _validateDateTime;
        private readonly Func<DateTimeOffset, AttributeResult> _validateDateTimeOffset;
#if NET6_0_OR_GREATER
        private readonly Func<DateOnly, AttributeResult> _validateDateOnly;
#endif
        /// <inheritdoc/>
        public DateRangeAttribute(string minimum, string maximum)
        {
            if (string.IsNullOrWhiteSpace(minimum))
                throw new ArgumentException("Minimum date cannot be null or empty.", nameof(minimum));
            if (string.IsNullOrWhiteSpace(maximum))
                throw new ArgumentException("Maximum date cannot be null or empty.", nameof(maximum));

            if (!DateTime.TryParse(minimum, out DateTime minDate))
                throw new ArgumentException("Invalid minimum date format.", nameof(minimum));
            if (!DateTime.TryParse(maximum, out DateTime maxDate))
                throw new ArgumentException("Invalid maximum date format.", nameof(maximum));

            if (minDate > maxDate)
                throw new ArgumentException("Minimum date cannot be greater than maximum date.");

            _validateDateTime = dt => dt >= minDate && dt <= maxDate ?
                AttributeResult.Success() :
                AttributeResult.Fail("The date must be between {0} and {1}.", minDate, maxDate);
            _validateDateTimeOffset = dto => dto.UtcDateTime >= minDate && dto.UtcDateTime <= maxDate ?
                AttributeResult.Success() :
                AttributeResult.Fail("The date must be between {0} and {1}.", minDate, maxDate);
#if NET6_0_OR_GREATER
             _validateDateOnly = dateOnly => dateOnly >= DateOnly.FromDateTime(minDate) &&
                                             dateOnly <= DateOnly.FromDateTime(maxDate) ?
                AttributeResult.Success() :
                AttributeResult.Fail("The date must be between {0} and {1}.", minDate, maxDate);
#endif

        }
        /// <inheritdoc/>
        public DateRangeAttribute(DateTime minimum, DateTime maximum)
        {
            if (minimum > maximum)
                throw new ArgumentException("Minimum date cannot be greater than maximum date.");
            _validateDateTime = dt => dt >= minimum && dt <= maximum ?
                AttributeResult.Success() :
                AttributeResult.Fail("The date must be between {0} and {1}.", minimum, maximum);
            _validateDateTimeOffset = dto => dto.UtcDateTime >= minimum && dto.UtcDateTime <= maximum ?
                AttributeResult.Success() :
                AttributeResult.Fail("The date must be between {0} and {1}.", minimum, maximum);
#if NET6_0_OR_GREATER
            _validateDateOnly = dateOnly => dateOnly >= DateOnly.FromDateTime(minimum) &&
                                             dateOnly <= DateOnly.FromDateTime(maximum) ?
                AttributeResult.Success() :
                AttributeResult.Fail("The date must be between {0} and {1}.", minimum, maximum);
#endif
        }

#if NET6_0_OR_GREATER
        /// <inheritdoc/>
        public DateRangeAttribute(DateOnly minimum, DateOnly maximum)
        {
            if (minimum > maximum)
                throw new ArgumentException("Minimum date cannot be greater than maximum date.");
            _validateDateOnly = dateOnly => dateOnly >= minimum && dateOnly <= maximum ?
                AttributeResult.Success() :
                AttributeResult.Fail("The date must be between {0} and {1}.", minimum, maximum);
            _validateDateTime = dt => dt >= minimum.ToDateTime(TimeOnly.MinValue) && dt <= maximum.ToDateTime(TimeOnly.MinValue) ?
                AttributeResult.Success() :
                AttributeResult.Fail("The date must be between {0} and {1}.", minimum, maximum);
            _validateDateTimeOffset = dto => dto.UtcDateTime >= minimum.ToDateTime(TimeOnly.MinValue) &&
                                             dto.UtcDateTime <= maximum.ToDateTime(TimeOnly.MinValue) ?
                AttributeResult.Success() :
                AttributeResult.Fail("The date must be between {0} and {1}.", minimum, maximum);
        }
#endif


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "DateRangeValidationError";

        /// <summary>
        /// Validates a DateTime value for range.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTime value) => _validateDateTime(value);

#if NET6_0_OR_GREATER
        /// <summary>
        /// Validates a DateOnly value for range.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateOnly value) => _validateDateOnly(value);
#endif

        /// <summary>
        /// Validates a DateTimeOffset value for range.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTimeOffset value) => _validateDateTimeOffset(value);
    }
}
