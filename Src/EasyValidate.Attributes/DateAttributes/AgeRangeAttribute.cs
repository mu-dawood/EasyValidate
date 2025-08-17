using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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


        /// <summary>
        /// Validates a DateTime value for age range.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            var now = DateTime.UtcNow;
            var age = now.Year - value.Year;
            // Adjust age if birthday hasn't occurred yet this year
            // now.AddYears(-age) gives the last birthday date in the current year
            // If the birth date is after the last birthday, subtract one from age
            if (value.Date > now.AddYears(-age).Date)
                age--;
            bool isValid = age >= MinimumAge && age <= MaximumAge;
            return isValid
                ? AttributeResult.Success()
                : AttributeResult.Fail("The field {0} must represent an age between {1} and {2} years.", propertyName, MinimumAge, MaximumAge);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Validates a DateOnly value for age range.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateOnly value)
        {
            var now = DateOnly.FromDateTime(DateTime.UtcNow);
            var age = now.Year - value.Year;
            // Adjust age if birthday hasn't occurred yet this year
            // now.AddYears(-age) gives the last birthday date in the current year
            // If the birth date is after the last birthday, subtract one from age
            if (value > now.AddYears(-age))
                age--;
            bool isValid = age >= MinimumAge && age <= MaximumAge;
            return isValid
                ? AttributeResult.Success()
                : AttributeResult.Fail("The field {0} must represent an age between {1} and {2} years.", propertyName, MinimumAge, MaximumAge);
        }
#endif

        /// <summary>
        /// Validates a DateTimeOffset value for age range.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTimeOffset value)
        {
            var now = DateTimeOffset.UtcNow;
            var age = now.Year - value.Year;
            // Adjust age if birthday hasn't occurred yet this year
            // now.AddYears(-age) gives the last birthday date in the current year
            // If the birth date is after the last birthday, subtract one from age
            if (value.Date > now.AddYears(-age).Date)
                age--;
            bool isValid = age >= MinimumAge && age <= MaximumAge;
            return isValid
                ? AttributeResult.Success()
                : AttributeResult.Fail("The field {0} must represent an age between {1} and {2} years.", propertyName, MinimumAge, MaximumAge);
        }
    }
}
