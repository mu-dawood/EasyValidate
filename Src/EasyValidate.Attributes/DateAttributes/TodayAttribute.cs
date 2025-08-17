using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date is today (ignoring time).
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Today]
    ///     public DateTime CheckInDate { get; set; } // Valid: DateTime.Today, Invalid: yesterday, tomorrow
    ///     
    ///     [Today]
    ///     public DateTime TransactionDate { get; set; } // Valid: any time today, Invalid: past or future dates
    /// }
    /// </code>
    /// </example>
    public class TodayAttribute : DateValidationAttributeBase
    {


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "TodayValidationError";

        private static bool IsToday(DateTime value, DateTime utcNow)
        {
            return value.Date == utcNow.Date;
        }


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            return IsToday(value, DateTime.UtcNow)
                ? AttributeResult.Success()
                : AttributeResult.Fail("The {0} field must be today's date.", propertyName);
        }


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, DateTimeOffset value)
        {
            return IsToday(value.UtcDateTime, DateTime.UtcNow)
                ? AttributeResult.Success()
                : AttributeResult.Fail("The {0} field must be today's date.", propertyName);
        }
#if NET6_0_OR_GREATER
        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, DateOnly value)
        {
            return value == DateOnly.FromDateTime(DateTime.UtcNow)
                ? AttributeResult.Success()
                : AttributeResult.Fail("The {0} field must be today's date.", propertyName);
        }
#endif
    }
}
