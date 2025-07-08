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
        public static readonly Lazy<FutureDateAttribute> Instance = new(() => new FutureDateAttribute());

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "FutureDateValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must be a future date.";

        /// <inheritdoc/>
        protected override AttributeResult ValidateUtc(object obj, string propertyName, DateTime value)
        {
            bool isValid = value > Now;
            return isValid
               ? AttributeResult.Success()
               : AttributeResult.Fail(ErrorMessage, propertyName);
        }
    }
}
