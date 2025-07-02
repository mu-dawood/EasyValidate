using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Represents the quarters of a year.
    /// </summary>
    public enum Quarter
    {
        Q1 = 1,
        Q2 = 2,
        Q3 = 3,
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
        public Quarter[] Quarters { get; } = quarters;

        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsInvalid.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "QuarterValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must fall within the specified quarters.";

        /// Arguments propertyName

        /// <inheritdoc/>
        protected override AttributeResult ValidateUtc(object obj, string propertyName, DateTime value)
        {
            int quarter = (value.Month - 1) / 3 + 1;
            bool isValid = Array.IndexOf(Quarters, (Quarter)quarter) != -1;
            return isValid
              ? AttributeResult.Success()
              : AttributeResult.Fail(ErrorMessage, propertyName);
        }
    }
}
