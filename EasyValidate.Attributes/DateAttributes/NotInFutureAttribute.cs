using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date is today or in the past (not in the future).
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [NotInFuture]
    ///     public DateTime BirthDate { get; set; } // Valid: 2000-01-01, DateTime.Now, Invalid: DateTime.Now.AddDays(1)
    ///     
    ///     [NotInFuture]
    ///     public DateTime PurchaseDate { get; set; } // Valid: 2024-01-01, DateTime.Today, Invalid: 2025-12-31
    /// }
    /// </code>
    /// </example>
    public class NotInFutureAttribute : DateValidationAttributeBase
    {



        private string _errorCode = "NotInFutureValidationError";
        /// <inheritdoc/>
        public override string ErrorCode
        {
            get => _errorCode;
            set => _errorCode = value;
        }

        /// <summary>
        /// Validates a DateTime value for not in future.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            if (value <= DateTime.UtcNow)
                return AttributeResult.Success();
            return AttributeResult.Fail("The {0} field must not be in the future.", propertyName);
        }

#if NET6_0_OR_GREATER
        /// <summary>
        /// Validates a DateOnly value for not in future.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateOnly value)
        {
            if (value.ToDateTime(TimeOnly.MinValue) <= DateTime.UtcNow.Date)
                return AttributeResult.Success();
            return AttributeResult.Fail("The {0} field must not be in the future.", propertyName);
        }
#endif

        /// <summary>
        /// Validates a DateTimeOffset value for not in future.
        /// </summary>
        public override AttributeResult Validate(string propertyName, DateTimeOffset value)
        {
            if (value <= DateTimeOffset.UtcNow)
                return AttributeResult.Success();
            return AttributeResult.Fail("The {0} field must not be in the future.", propertyName);
        }
    }
}
