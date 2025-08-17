using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date is today or in the future (not in the past).
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [NotInPast]
    ///     public DateTime EventDate { get; set; } // Valid: DateTime.Today, 2025-12-31, Invalid: 2020-01-01
    ///     
    ///     [NotInPast]
    ///     public DateTime ExpiryDate { get; set; } // Valid: DateTime.Now.AddDays(30), Invalid: DateTime.Now.AddDays(-1)
    /// }
    /// </code>
    /// </example>
    public class NotInPastAttribute : DateValidationAttributeBase
    {



        private string _errorCode = "NotInPastValidationError";
        /// <inheritdoc/>
        public override string ErrorCode
        {
            get => _errorCode;
            set => _errorCode = value;
        }

        /// <summary>
        /// Validates a DateTime value for not in past.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            if (value >= DateTime.UtcNow)
                return AttributeResult.Success();
            return AttributeResult.Fail("The {0} field must not be in the past.", propertyName);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Validates a DateOnly value for not in past.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateOnly value)
        {
            if (value.ToDateTime(TimeOnly.MinValue) >= DateTime.UtcNow.Date)
                return AttributeResult.Success();
            return AttributeResult.Fail("The {0} field must not be in the past.", propertyName);
        }
#endif

        /// <summary>
        /// Validates a DateTimeOffset value for not in past.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTimeOffset value)
        {
            if (value >= DateTimeOffset.UtcNow)
                return AttributeResult.Success();
            return AttributeResult.Fail("The {0} field must not be in the past.", propertyName);
        }
    }
}
