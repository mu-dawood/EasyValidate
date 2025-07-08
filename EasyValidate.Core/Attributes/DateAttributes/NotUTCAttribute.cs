using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a date is not in UTC format.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [NotUTC]
    ///     public DateTime LocalTime { get; set; } // Valid: DateTime.Now, Invalid: DateTime.UtcNow
    ///     
    ///     [NotUTC]
    ///     public DateTime UserTimestamp { get; set; } // Valid: new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Local), Invalid: new DateTime(2024, 1, 1, 12, 0, 0, DateTimeKind.Utc)
    /// }
    /// </code>
    /// </example>
    public class NotUTCAttribute : DateValidationAttributeBase
    {
        public static readonly Lazy<NotUTCAttribute> Instance = new(() => new NotUTCAttribute());


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NotUTCValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must not be in UTC format.";

        /// Arguments propertyName

        /// <inheritdoc/>
        protected override AttributeResult ValidateUtc(object obj, string propertyName, DateTime value)
        {
            bool isValid = value.Kind != DateTimeKind.Utc;
            return isValid
              ? AttributeResult.Success()
              : AttributeResult.Fail(ErrorMessage, propertyName);
        }
    }
}
