using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Represents the quarters of a year.
    /// </summary>
    public enum Quarter
    {
        /// <inheritdoc/>
        Q1 = 1,
        /// <inheritdoc/>
        Q2 = 2,
        /// <inheritdoc/>
        Q3 = 3,
        /// <inheritdoc/>
        Q4 = 4
    }

    /// <summary>
    /// Validates that a date falls within a specified quarter.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Quarter(Quarter.Q1, Quarter.Q3)]
    ///     public DateTime ReportDate { get; set; } // Valid: 2024-02-15 (Q1), 2024-08-10 (Q3), Invalid: 2024-05-01 (Q2)
    ///     
    ///     [Quarter(Quarter.Q4)]
    ///     public DateTime YearEndDate { get; set; } // Valid: 2024-12-31, Invalid: 2024-06-15
    /// }
    /// </code>
    /// </example>
    public class QuarterAttribute(params Quarter[] quarters) : DateValidationAttributeBase
    {
        /// <inheritdoc/>
        public Quarter[] Quarters { get; } = quarters;


        private string _errorCode = "QuarterValidationError";
        /// <inheritdoc/>
        public override string ErrorCode
        {
            get => _errorCode;
            set => _errorCode = value;
        }

        private AttributeResult ValidateQuarter(string propertyName, int month)
        {
            int quarter = (month - 1) / 3 + 1;
            if (Array.IndexOf(Quarters, (Quarter)quarter) != -1)
                return AttributeResult.Success();
            return AttributeResult.Fail("The field {0} must fall within the specified quarters.", propertyName);
        }

        /// <summary>
        /// Validates a DateTime value for allowed quarters.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            return ValidateQuarter(propertyName, value.Month);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Validates a DateOnly value for allowed quarters.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateOnly value)
        {
            return ValidateQuarter(propertyName, value.Month);
        }
#endif

        /// <summary>
        /// Validates a DateTimeOffset value for allowed quarters.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTimeOffset value)
        {
            return ValidateQuarter(propertyName, value.Month);
        }
    }
}
