using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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
        public static readonly Lazy<NotInFutureAttribute> Instance = new(() => new NotInFutureAttribute());


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NotInFutureValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must not be in the future.";

        /// Arguments propertyName

        /// <inheritdoc/>
        protected override AttributeResult ValidateUtc(object obj, string propertyName, DateTime value)
        {
            bool isValid = value <= DateTime.Now;
            return isValid
               ? AttributeResult.Success()
               : AttributeResult.Fail(ErrorMessage, propertyName);
        }
    }
}
