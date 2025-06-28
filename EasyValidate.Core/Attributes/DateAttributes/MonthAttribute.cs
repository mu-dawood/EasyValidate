using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a date falls in one of the specified months.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Month(6, 7, 8)] // Summer months
    ///     public DateTime VacationDate { get; set; } // Valid: June, July, August dates, Invalid: dates in other months
    ///     
    ///     [Month(12, 1, 2)] // Winter months
    ///     public DateTime WinterEvent { get; set; } // Valid: December, January, February dates, Invalid: dates in other months
    /// }
    /// </code>
    /// </example>
    public class MonthAttribute(params int[] months) : DateValidationAttributeBase
    {
        /// <summary>
        /// The array of allowed months (1-12).
        /// </summary>
        public int[] Months { get; } = months;

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "MonthValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must contain a valid month.";

        /// <inheritdoc/>
        protected override AttributeResult<DateTime> ValidateUtc(object obj, string propertyName, DateTime value)
        {
            bool isValid = Array.Exists(Months, month => month == value.Month);
            return new AttributeResult<DateTime>(isValid, value, propertyName, string.Join(", ", Months));
        }
    }
}
