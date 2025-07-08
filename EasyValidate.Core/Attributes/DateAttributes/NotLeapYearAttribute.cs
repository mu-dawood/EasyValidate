using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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
        public static readonly Lazy<NotLeapYearAttribute> Instance = new(() => new NotLeapYearAttribute());


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NotLeapYearValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must not be in a leap year.";

        /// Arguments propertyName

        /// <inheritdoc/>
        protected override AttributeResult ValidateUtc(object obj, string propertyName, DateTime value)
        {
            bool isValid = !DateTime.IsLeapYear(value.Year);
            return isValid
              ? AttributeResult.Success()
              : AttributeResult.Fail(ErrorMessage, propertyName);
        }
    }
}
