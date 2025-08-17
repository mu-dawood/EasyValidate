using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date does not fall in a leap year.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [NotLeapYear]
    ///     public DateTime StandardYear { get; set; } // Valid: 2023-06-15, 2025-01-01, Invalid: 2024-02-29 (2024 is leap year)
    ///     
    ///     [NotLeapYear]
    ///     public DateTime EventDate { get; set; } // Valid: 2021-12-31, Invalid: 2020-01-01 (2020 is leap year)
    /// }
    /// </code>
    /// </example>
    public class NotLeapYearAttribute : DateValidationAttributeBase
    {



        private string _errorCode = "NotLeapYearValidationError";
        /// <inheritdoc/>
        public override string ErrorCode
        {
            get => _errorCode;
            set => _errorCode = value;
        }

        private static AttributeResult ValidateNotLeapYear(string propertyName, int year)
        {
            if (!DateTime.IsLeapYear(year))
                return AttributeResult.Success();
            return AttributeResult.Fail("The field {0} must not be in a leap year.", propertyName);
        }

        /// <summary>
        /// Validates a DateTime value for not leap year.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            return ValidateNotLeapYear(propertyName, value.Year);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Validates a DateOnly value for not leap year.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateOnly value)
        {
            return ValidateNotLeapYear(propertyName, value.Year);
        }
#endif

        /// <summary>
        /// Validates a DateTimeOffset value for not leap year.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTimeOffset value)
        {
            return ValidateNotLeapYear(propertyName, value.Year);
        }
    }
}
