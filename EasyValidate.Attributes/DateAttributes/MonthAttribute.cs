using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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


        private string _errorCode = "MonthValidationError";
        /// <inheritdoc/>
        public override string ErrorCode
        {
            get => _errorCode;
            set => _errorCode = value;
        }

        private AttributeResult ValidateMonth(string propertyName, int month)
        {
            if (Array.Exists(Months, m => m == month))
                return AttributeResult.Success();
            return AttributeResult.Fail("The {0} field must contain a valid month: {1}.", propertyName, string.Join(", ", Months));
        }

        /// <summary>
        /// Validates a DateTime value for allowed months.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            return ValidateMonth(propertyName, value.Month);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Validates a DateOnly value for allowed months.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateOnly value)
        {
            return ValidateMonth(propertyName, value.Month);
        }
#endif

        /// <summary>
        /// Validates a DateTimeOffset value for allowed months.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTimeOffset value)
        {
            return ValidateMonth(propertyName, value.Month);
        }
    }
}
