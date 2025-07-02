using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a date falls on one of the specified days of the month.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Day(1, 15, 31)]
    ///     public DateTime PaymentDate { get; set; } // Valid: 2024-01-01, 2024-01-15, 2024-01-31, Invalid: 2024-01-10
    ///     
    ///     [Day(1)]
    ///     public DateTime MonthlyReport { get; set; } // Valid: 2024-01-01, Invalid: 2024-01-02
    /// }
    /// </code>
    /// </example>
    public class DayAttribute(params int[] days) : DateValidationAttributeBase
    {
        /// <summary>
        /// The array of allowed days of the month (1-31).
        /// </summary>
        public int[] Days { get; } = days;

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "DayValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must be one of the following days: {1}.";

        /// <inheritdoc/>
        protected override AttributeResult ValidateUtc(object obj, string propertyName, DateTime value)
        {
            bool isValid = Array.Exists(Days, day => day == value.Day);
            return isValid
               ? AttributeResult.Success()
               : AttributeResult.Fail(ErrorMessage, propertyName, string.Join(", ", Days));
        }
    }
}
