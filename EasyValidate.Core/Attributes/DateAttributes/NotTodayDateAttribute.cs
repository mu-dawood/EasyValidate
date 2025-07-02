using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NotTodayDateValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must not be today's date.";

        /// <inheritdoc/>
        protected override AttributeResult ValidateUtc(object obj, string propertyName, DateTime value)
        {
            bool isValid = value.Date != Now.Date;
            return isValid
              ? AttributeResult.Success()
              : AttributeResult.Fail(ErrorMessage, propertyName);
        }
    }
}
