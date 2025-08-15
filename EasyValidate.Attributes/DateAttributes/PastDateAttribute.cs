using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date is in the past.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [PastDate]
    ///     public DateTime BirthDate { get; set; } // Valid: yesterday, last year, Invalid: today, tomorrow
    ///     
    ///     [PastDate]
    ///     public DateTime CompletedDate { get; set; } // Valid: any past date, Invalid: current or future date
    /// }
    /// </code>
    /// </example>
    public class PastDateAttribute : DateValidationAttributeBase
    {



        private string _errorCode = "PastDateValidationError";
        /// <inheritdoc/>
        public override string ErrorCode
        {
            get => _errorCode;
            set => _errorCode = value;
        }

        /// <summary>
        /// Validates a DateTime value for past date.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            if (value < DateTime.UtcNow)
                return AttributeResult.Success();
            return AttributeResult.Fail("The {0} field must be a past date.", propertyName);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Validates a DateOnly value for past date.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateOnly value)
        {
            if (value < DateOnly.FromDateTime(DateTime.UtcNow))
                return AttributeResult.Success();
            return AttributeResult.Fail("The {0} field must be a past date.", propertyName);
        }
#endif

        /// <summary>
        /// Validates a DateTimeOffset value for past date.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTimeOffset value)
        {
            if (value < DateTimeOffset.UtcNow)
                return AttributeResult.Success();
            return AttributeResult.Fail("The {0} field must be a past date.", propertyName);
        }
    }
}
