using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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


        private string _errorCode = "LeapYearValidationError";
        /// <inheritdoc/>
        public override string ErrorCode
        {
            get => _errorCode;
            set => _errorCode = value;
        }

        private static AttributeResult ValidateLeapYear(string propertyName, int year)
        {
            if (DateTime.IsLeapYear(year))
                return AttributeResult.Success();
            return AttributeResult.Fail("The field {0} must be in a leap year.", propertyName);
        }

        /// <summary>
        /// Validates a DateTime value for leap year.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            return ValidateLeapYear(propertyName, value.Year);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Validates a DateOnly value for leap year.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateOnly value)
        {
            return ValidateLeapYear(propertyName, value.Year);
        }
#endif

        /// <summary>
        /// Validates a DateTimeOffset value for leap year.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTimeOffset value)
        {
            return ValidateLeapYear(propertyName, value.Year);
        }
    }
}
