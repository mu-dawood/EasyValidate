using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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

        private AttributeResult IsValidYear(int year, string propertyName)
        {
            if (Array.Exists(Years, y => y == year))
                return AttributeResult.Success();
            return AttributeResult.Fail("The field {0} must fall within the specified years: {1}.", propertyName, string.Join(", ", Years));
        }


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            return IsValidYear(value.Year, propertyName);
        }


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, DateTimeOffset value)
        {
            return IsValidYear(value.Year, propertyName);
        }
#if NET6_0_OR_GREATER
        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, DateOnly value)
        {
            return IsValidYear(value.Year, propertyName);
        }
#endif
    }
}
