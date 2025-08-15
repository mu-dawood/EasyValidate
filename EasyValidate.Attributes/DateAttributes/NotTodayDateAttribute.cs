using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date is not today's date.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [NotTodayDate]
    ///     public DateTime ScheduledDate { get; set; } // Valid: DateTime.Today.AddDays(1), 2020-01-01, Invalid: DateTime.Today
    ///     
    ///     [NotTodayDate]
    ///     public DateTime HistoricalDate { get; set; } // Valid: 2024-01-01, DateTime.Today.AddDays(-1), Invalid: DateTime.Today
    /// }
    /// </code>
    /// </example>
    public class NotTodayDateAttribute : DateValidationAttributeBase
    {



        private string _errorCode = "NotTodayDateValidationError";
        /// <inheritdoc/>
        public override string ErrorCode
        {
            get => _errorCode;
            set => _errorCode = value;
        }

        private static AttributeResult ValidateNotToday(string propertyName, DateTime date)
        {
            if (date.Date != DateTime.UtcNow.Date)
                return AttributeResult.Success();
            return AttributeResult.Fail("The field {0} must not be today's date.", propertyName);
        }

        /// <summary>
        /// Validates a DateTime value for not today.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            return ValidateNotToday(propertyName, value);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Validates a DateOnly value for not today.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateOnly value)
        {
            return ValidateNotToday(propertyName, value.ToDateTime(TimeOnly.MinValue));
        }
#endif

        /// <summary>
        /// Validates a DateTimeOffset value for not today.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTimeOffset value)
        {
            return ValidateNotToday(propertyName, value.UtcDateTime);
        }
    }
}
