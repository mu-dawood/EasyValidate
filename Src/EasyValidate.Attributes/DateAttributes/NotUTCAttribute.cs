using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public class NotUTCAttribute : Attribute, IValidationAttribute<DateTime>
    {


        private string _errorCode = "NotUTCValidationError";
        /// <inheritdoc/>
        public string ErrorCode
        {
            get => _errorCode;
            set => _errorCode = value;
        }

        /// <inheritdoc/>
        public string Chain { get; set; } = string.Empty;
        /// <inheritdoc/>
        public string? ConditionalMethod { get; set; }

        private static bool IsNotUtc(DateTime value) => value.Kind != DateTimeKind.Utc;

        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, DateTime value)
        {
            return IsNotUtc(value)
                ? AttributeResult.Success()
                : AttributeResult.Fail("The field {0} must not be in UTC format.", propertyName);
        }
    }
}
