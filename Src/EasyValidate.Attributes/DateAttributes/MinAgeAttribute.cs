using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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


        private string _errorCode = "MinAgeValidationError";
        /// <inheritdoc/>
        public override string ErrorCode
        {
            get => _errorCode;
            set => _errorCode = value;
        }

        /// <summary>
        /// Validates a DateTime value for minimum age.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            var now = DateTime.UtcNow;
            var age = now.Year - value.Year;
            if (value.Date > now.AddYears(-age).Date) age--;
            if (age >= MinimumAge)
                return AttributeResult.Success();
            return AttributeResult.Fail("The field {0} must represent an age of at least {1} years.", propertyName, MinimumAge);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Validates a DateOnly value for minimum age.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateOnly value)
        {
            var now = DateTime.UtcNow;
            var birthDate = value.ToDateTime(TimeOnly.MinValue);
            var age = now.Year - birthDate.Year;
            if (birthDate.Date > now.AddYears(-age).Date) age--;
            if (age >= MinimumAge)
                return AttributeResult.Success();
            return AttributeResult.Fail("The field {0} must represent an age of at least {1} years.", propertyName, MinimumAge);
        }
#endif

        /// <summary>
        /// Validates a DateTimeOffset value for minimum age.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTimeOffset value)
        {
            var now = DateTimeOffset.UtcNow;
            var age = now.Year - value.Year;
            if (value.Date > now.AddYears(-age).Date) age--;
            if (age >= MinimumAge)
                return AttributeResult.Success();
            return AttributeResult.Fail("The field {0} must represent an age of at least {1} years.", propertyName, MinimumAge);
        }

    }
}
