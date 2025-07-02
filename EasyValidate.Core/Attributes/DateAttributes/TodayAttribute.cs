using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must be today's date.";

        /// <inheritdoc/>
        protected override AttributeResult ValidateUtc(object obj, string propertyName, DateTime value)
        {
            bool isValid = value.Date == Now.Date;
            return isValid
              ? AttributeResult.Success()
              : AttributeResult.Fail(ErrorMessage, propertyName);
        }
    }
}
