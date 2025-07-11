using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a date is in UTC format.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [UTC]
    ///     public DateTime CreatedAt { get; set; } // Valid: DateTime.UtcNow, Invalid: DateTime.Now
    ///     
    ///     [UTC]
    ///     public DateTime LogTimestamp { get; set; } // Valid: new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc), Invalid: new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Local)
    /// }
    /// </code>
    /// </example>
    public class UTCAttribute : DateValidationAttributeBase
    {
        public static readonly Lazy<UTCAttribute> Instance = new(() => new UTCAttribute());


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "UTCValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must be in UTC format.";

        /// Arguments propertyName

        /// <inheritdoc/>
        protected override AttributeResult ValidateUtc(object obj, string propertyName, DateTime value)
        {
            bool isValid = value.Kind == DateTimeKind.Utc;
            return isValid
               ? AttributeResult.Success()
               : AttributeResult.Fail(ErrorMessage, propertyName);
        }
    }
}
