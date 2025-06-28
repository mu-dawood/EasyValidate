using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a date represents a maximum age.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [MaxAge(65)]
    ///     public DateTime EmployeeBirthDate { get; set; } // Valid: 1970-01-01 (if person is 65 or younger), Invalid: 1950-01-01 (if person is over 65)
    ///     
    ///     [MaxAge(30)]
    ///     public DateTime ParticipantBirthDate { get; set; } // Valid: 2000-06-15 (if person is 30 or younger), Invalid: 1985-06-15 (if person is over 30)
    /// }
    /// </code>
    /// </example>
    public class MaxAgeAttribute(int maximumAge) : DateValidationAttributeBase
    {
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsValid.
        /// </summary>

        /// <summary>
        /// The maximum age in years.
        /// </summary>
        public int MaximumAge { get; } = maximumAge;

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "MaxAgeValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The field {0} must represent an age of no more than {1} years.";

        /// Arguments propertyName, MaximumAge

        /// <inheritdoc/>
        protected override AttributeResult<DateTime> ValidateUtc(object obj, string propertyName, DateTime value)
        {
            var age = Now.Year - value.Year;
            if (value.Date > Now.AddYears(-age).Date) age--;

            bool isValid = age <= MaximumAge;
            return new AttributeResult<DateTime>(isValid, value, propertyName);
        }
    }
}
