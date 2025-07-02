using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsValid.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NotInPastValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must not be in the past.";

        /// Arguments propertyName

        /// <inheritdoc/>
        protected override AttributeResult ValidateUtc(object obj, string propertyName, DateTime value)
        {
            bool isValid = value >= Now;
            return isValid
               ? AttributeResult.Success()
               : AttributeResult.Fail(ErrorMessage, propertyName);
        }
    }
}
