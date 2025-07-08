using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a date falls in a leap year.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [LeapYear]
    ///     public DateTime SpecialDate { get; set; } // Valid: 2024-02-29, 2020-06-15, Invalid: 2023-01-01, 2021-12-31
    ///     
    ///     [LeapYear]
    ///     public DateTime EventDate { get; set; } // Valid: any date in 2024, 2020, 2016, Invalid: dates in 2023, 2022, 2021
    /// }
    /// </code>
    /// </example>
    public class LeapYearAttribute : DateValidationAttributeBase
    {
        public static readonly Lazy<LeapYearAttribute> Instance = new(() => new LeapYearAttribute());


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "LeapYearValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must be in a leap year.";

        /// Arguments propertyName

        /// <inheritdoc/>
        protected override AttributeResult ValidateUtc(object obj, string propertyName, DateTime value)
        {
            bool isValid = DateTime.IsLeapYear(value.Year);
            return isValid
               ? AttributeResult.Success()
               : AttributeResult.Fail(ErrorMessage, propertyName);
        }
    }
}
