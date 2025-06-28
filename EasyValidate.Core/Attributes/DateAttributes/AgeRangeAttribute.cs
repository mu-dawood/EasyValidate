using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a date represents an age within a specified range.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [AgeRange(18, 65)]
    ///     public DateTime BirthDate { get; set; } // Valid: birth date resulting in age 18-65, Invalid: younger than 18 or older than 65
    ///     
    ///     [AgeRange(21, 100)]
    ///     public DateTime DateOfBirth { get; set; } // Valid: birth date resulting in age 21-100, Invalid: under 21
    /// }
    /// </code>
    /// </example>
    public class AgeRangeAttribute(int minimumAge, int maximumAge) : DateValidationAttributeBase
    {
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsInvalid.
        /// </summary>

        /// <summary>
        /// The minimum age in years.
        /// </summary>
        public int MinimumAge { get; } = minimumAge;

        /// <summary>
        /// The maximum age in years.
        /// </summary>
        public int MaximumAge { get; } = maximumAge;

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "AgeRangeValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The field {0} must represent an age between {1} and {2} years.";

        /// Arguments propertyName, MinimumAge, MaximumAge

        /// <inheritdoc/>
        protected override AttributeResult<DateTime> ValidateUtc(object obj, string propertyName, DateTime value)
        {
            var age = Now.Year - value.Year;
            if (value.Date > Now.AddYears(-age).Date) age--;

            bool isValid = age >= MinimumAge && age <= MaximumAge;
            return new AttributeResult<DateTime>(isValid, value, propertyName);
        }
    }
}
