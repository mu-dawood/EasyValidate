using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a date represents a minimum age.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [MinAge(18)]
    ///     public DateTime BirthDate { get; set; } // Valid: 2000-01-01 (if person is 18+), Invalid: 2010-01-01 (if person is under 18)
    ///     
    ///     [MinAge(21)]
    ///     public DateTime DateOfBirth { get; set; } // Valid: 1995-06-15 (if person is 21+), Invalid: 2005-06-15 (if person is under 21)
    /// }
    /// </code>
    /// </example>
    public class MinAgeAttribute(int minimumAge) : DateValidationAttributeBase
    {
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsInvalid.
        /// </summary>

        /// <summary>
        /// The minimum age in years.
        /// </summary>
        public int MinimumAge { get; } = minimumAge;

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "MinAgeValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The field {0} must represent an age of at least {1} years.";

        /// Arguments propertyName, MinimumAge

        /// <inheritdoc/>
        protected override AttributeResult<DateTime> ValidateUtc(object obj, string propertyName, DateTime value)
        {
            var age = Now.Year - value.Year;
            if (value.Date > Now.AddYears(-age).Date) age--;

            bool isValid = age >= MinimumAge;
            return new AttributeResult<DateTime>(isValid, value, propertyName);
        }
    }
}
