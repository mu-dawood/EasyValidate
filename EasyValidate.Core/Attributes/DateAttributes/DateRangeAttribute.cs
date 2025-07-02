using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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
        public DateRangeAttribute(string minimum, string maximum)
        {
            if (string.IsNullOrWhiteSpace(minimum))
                throw new ArgumentException("Minimum date cannot be null or empty.", nameof(minimum));
            if (string.IsNullOrWhiteSpace(maximum))
                throw new ArgumentException("Maximum date cannot be null or empty.", nameof(maximum));

            if (!DateTime.TryParse(minimum, out _))
                throw new ArgumentException("Invalid minimum date format.", nameof(minimum));
            if (!DateTime.TryParse(maximum, out _))
                throw new ArgumentException("Invalid maximum date format.", nameof(maximum));
        }
        public DateRangeAttribute(DateTime minimum, DateTime maximum)
        {
            if (minimum > maximum)
                throw new ArgumentException("Minimum date cannot be greater than maximum date.");

            Minimum = minimum;
            Maximum = maximum;
        }

#if NET6_0_OR_GREATER
        public DateRangeAttribute(DateOnly minimum, DateOnly maximum)
        {
            if (minimum > maximum)
                throw new ArgumentException("Minimum date cannot be greater than maximum date.");

            Minimum = minimum.ToDateTime(TimeOnly.FromTimeSpan(Now.TimeOfDay));
            Maximum = maximum.ToDateTime(TimeOnly.FromTimeSpan(Now.TimeOfDay));
        }
#endif
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsInvalid.
        /// </summary>

        /// <summary>
        /// The minimum allowed date (inclusive).
        /// </summary>
        public DateTime Minimum { get; private set; }
        /// <summary>
        /// The maximum allowed date (inclusive).
        /// </summary>
        public DateTime Maximum { get; private set; }

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "DateRangeValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must be between {1} and {2}.";

        /// Arguments propertyName, Minimum, Maximum

        /// <inheritdoc/>
        protected override AttributeResult ValidateUtc(object obj, string propertyName, DateTime value)
        {
            bool isValid = value >= Minimum && value <= Maximum;
            return isValid
               ? AttributeResult.Success()
               : AttributeResult.Fail(ErrorMessage, propertyName, Minimum, Maximum);
        }
    }
}
