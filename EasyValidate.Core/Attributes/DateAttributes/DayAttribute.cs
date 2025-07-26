using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a date falls on one of the specified days of the month.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Day(1, 15, 31)]
    ///     public DateTime PaymentDate { get; set; } // Valid: 2024-01-01, 2024-01-15, 2024-01-31, Invalid: 2024-01-10
    ///     
    ///     [Day(1)]
    ///     public DateTime MonthlyReport { get; set; } // Valid: 2024-01-01, Invalid: 2024-01-02
    /// }
    /// </code>
    /// </example>
    public class DayAttribute(params int[] days) : DateValidationAttributeBase
    {
        /// <summary>
        /// The array of allowed days of the month (1-31).
        /// </summary>
        public int[] Days { get; } = days ?? [];

        /// <inheritdoc/>
        private string _errorCode = "DayValidationError";
        public override string ErrorCode
        {
            get => _errorCode;
            set => _errorCode = value;
        }

        /// <summary>
        /// Validates a DateTime value for allowed days.
        /// </summary>
        public override AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, DateTime value) => ValidateDay(propertyName, value.Day);

#if NET6_0_OR_GREATER
        /// <summary>
        /// Validates a DateOnly value for allowed days.
        /// </summary>
        public override AttributeResult Validate(IServiceProvider serviceProvider,string propertyName, DateOnly value) => ValidateDay(propertyName, value.Day);
#endif

        /// <summary>
        /// Validates a DateTimeOffset value for allowed days.
        /// </summary>
        public override AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, DateTimeOffset value) => ValidateDay(propertyName, value.Day);

        private AttributeResult ValidateDay(string propertyName, int day)
        {
            if (Array.Exists(Days, d => d == day))
                return AttributeResult.Success();
            return AttributeResult.Fail("The field {0} must be one of the following days: {1}.", propertyName, string.Join(", ", Days));
        }
    }
}
