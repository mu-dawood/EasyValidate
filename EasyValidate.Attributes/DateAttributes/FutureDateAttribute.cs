using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date is in the future.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [FutureDate]
    ///     public DateTime EventDate { get; set; } // Valid: tomorrow, next week, Invalid: yesterday, today
    ///     
    ///     [FutureDate]
    ///     public DateTime ExpiryDate { get; set; } // Valid: any future date, Invalid: past or current date
    /// }
    /// </code>
    /// </example>
    public class FutureDateAttribute : DateValidationAttributeBase
    {



        private string _errorCode = "FutureDateValidationError";
        /// <inheritdoc/>
        public override string ErrorCode
        {
            get => _errorCode;
            set => _errorCode = value;
        }

        /// <summary>
        /// Validates a DateTime value for future date.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            bool isFuture = value.Kind switch
            {
                DateTimeKind.Utc => value > DateTime.UtcNow,
                DateTimeKind.Local => value > DateTime.Now,
                DateTimeKind.Unspecified => value > DateTime.UtcNow,
                _ => value > DateTime.UtcNow
            };
            if (isFuture)
                return AttributeResult.Success();
            return AttributeResult.Fail("The {0} field must be a future date.", propertyName);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Validates a DateOnly value for future date.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateOnly value)
        {
            if (value > DateOnly.FromDateTime(DateTime.UtcNow))
                return AttributeResult.Success();
            return AttributeResult.Fail("The {0} field must be a future date.", propertyName);
        }
#endif

        /// <summary>
        /// Validates a DateTimeOffset value for future date.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTimeOffset value)
        {
            if (value > DateTimeOffset.UtcNow)
                return AttributeResult.Success();
            return AttributeResult.Fail("The {0} field must be a future date.", propertyName);
        }
    }
}
