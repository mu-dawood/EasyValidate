using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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


        private string _errorCode = "MaxAgeValidationError";
        /// <inheritdoc/>
        public override string ErrorCode
        {
            get => _errorCode;
            set => _errorCode = value;
        }

        /// <summary>
        /// Validates a DateTime value for maximum age.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            var now = DateTime.UtcNow;
            var age = now.Year - value.Year;
            if (value.Date > now.AddYears(-age).Date) age--;
            if (age <= MaximumAge)
                return AttributeResult.Success();
            return AttributeResult.Fail("The field {0} must represent an age of no more than {1} years.", propertyName, MaximumAge);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Validates a DateOnly value for maximum age.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateOnly value)
        {
            var now = DateTime.UtcNow;
            var birthDate = value.ToDateTime(TimeOnly.MinValue);
            var age = now.Year - birthDate.Year;
            if (birthDate.Date > now.AddYears(-age).Date) age--;
            if (age <= MaximumAge)
                return AttributeResult.Success();
            return AttributeResult.Fail("The field {0} must represent an age of no more than {1} years.", propertyName, MaximumAge);
        }
#endif

        /// <summary>
        /// Validates a DateTimeOffset value for maximum age.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTimeOffset value)
        {
            var now = DateTimeOffset.UtcNow;
            var age = now.Year - value.Year;
            if (value.Date > now.AddYears(-age).Date) age--;
            if (age <= MaximumAge)
                return AttributeResult.Success();
            return AttributeResult.Fail("The field {0} must represent an age of no more than {1} years.", propertyName, MaximumAge);
        }
    }
}
