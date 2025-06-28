using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a date falls in one of the specified years.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Year(2023, 2024, 2025)]
    ///     public DateTime EventDate { get; set; } // Valid: any date in 2023, 2024, or 2025, Invalid: dates in other years
    ///     
    ///     [Year(2020)]
    ///     public DateTime HistoricalDate { get; set; } // Valid: any date in 2020, Invalid: dates in other years
    /// }
    /// </code>
    /// </example>
    public class YearAttribute(params int[] years) : DateValidationAttributeBase
    {
        /// <summary>
        /// The array of allowed years.
        /// </summary>
        public int[] Years { get; } = years;

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "YearValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must contain a valid year.";

        /// <inheritdoc/>
        protected override AttributeResult<DateTime> ValidateUtc(object obj, string propertyName, DateTime value)
        {
            bool isValid = Array.Exists(Years, year => year == value.Year);
            return new AttributeResult<DateTime>(isValid, value, propertyName, string.Join(", ", Years));
        }
    }
}
