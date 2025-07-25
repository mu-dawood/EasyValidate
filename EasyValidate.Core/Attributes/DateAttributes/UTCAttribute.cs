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
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public class UTCAttribute : Attribute, IValidationAttribute<DateTime>
    {
        public static readonly Lazy<UTCAttribute> Instance = new(() => new UTCAttribute());

        private string _errorCode = "UTCValidationError";
        public string ErrorCode
        {
            get => _errorCode;
            set => _errorCode = value;
        }

        public string Chain { get; set; } = string.Empty;
        public string? ConditionalMethod { get; set; }
        public ExecutionStrategy Strategy { get; set; } = ExecutionStrategy.ValidateAndStop;

        private static bool IsUtc(DateTime value) => value.Kind == DateTimeKind.Utc;

        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, DateTime value)
        {
            return IsUtc(value)
                ? AttributeResult.Success()
                : AttributeResult.Fail("The field {0} must be in UTC format.", propertyName);
        }
    }
}
