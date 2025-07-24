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
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public class NotUTCAttribute : Attribute, IValidationAttribute<DateTime>
    {
        public static readonly Lazy<NotUTCAttribute> Instance = new(() => new NotUTCAttribute());

        private string _errorCode = "NotUTCValidationError";
        public string ErrorCode
        {
            get => _errorCode;
            set => _errorCode = value;
        }

        public string Chain { get; set; } = string.Empty;
        public string? ConditionalMethod { get; set; }
        public ExecutionStrategy Strategy { get; set; } = ExecutionStrategy.ValidateAndStop;

        private static bool IsNotUtc(DateTime value) => value.Kind != DateTimeKind.Utc;

        public AttributeResult Validate(object obj, string propertyName, DateTime value)
        {
            return IsNotUtc(value)
                ? AttributeResult.Success()
                : AttributeResult.Fail("The field {0} must not be in UTC format.", propertyName);
        }
    }
}
