using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsInvalid.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "PastDateValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must be a past date.";

        /// Arguments propertyName

        /// <inheritdoc/>
        protected override AttributeResult ValidateUtc(object obj, string propertyName, DateTime value)
        {
            bool isValid = value < Now;
            return isValid
              ? AttributeResult.Success()
              : AttributeResult.Fail(ErrorMessage, propertyName);
        }
    }
}
