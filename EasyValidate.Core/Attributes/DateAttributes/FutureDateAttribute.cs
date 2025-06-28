using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsInvalid.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "FutureDateValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be a future date.";

        /// <inheritdoc/>
        protected override AttributeResult<DateTime> ValidateUtc(object obj, string propertyName, DateTime value)
        {
            bool isValid = value > Now;
            return new AttributeResult<DateTime>(isValid, value, propertyName);
        }
    }
}
