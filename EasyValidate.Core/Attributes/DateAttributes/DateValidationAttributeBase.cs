using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Base attribute for date validation attributes. Provides a contract for validating DateTime properties.
    /// </summary>
    /// <docs-display-name>Date & Time Validation Attributes</docs-display-name>
    /// <docs-icon>Calendar</docs-icon>
    /// <docs-description>Date and time validation attributes for temporal data validation. Includes date ranges, age verification, business day checks, and time period validation.</docs-description>
    /// <remarks>
    /// Initializes a new instance of the DateValidationAttributeBase class.
    /// </remarks>
    /// <param name="followUpValidations">Array of validation attribute names that should only be validated after this validation succeeds.</param>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public abstract class DateValidationAttributeBase : Attribute,
    IValidationAttribute<DateTime, DateTime>,
    IValidationAttribute<DateTimeOffset, DateTimeOffset>
#if NET6_0_OR_GREATER
        , IValidationAttribute<DateOnly, DateOnly>
#endif
    {
        protected DateTime Now = DateTime.UtcNow;

        /// <inheritdoc/>
        public virtual string Chain { get; set; } = string.Empty;

        /// <inheritdoc/>
        public virtual string? ConditionalMethod { get; set; }
        /// <inheritdoc/>
        public virtual ExecutionStrategy Strategy { get; set; } = ExecutionStrategy.ValidateAndStop;

        /// <inheritdoc/>
        public abstract string ErrorCode { get; set; }

        /// <inheritdoc/>
        public abstract string ErrorMessage { get; set; }

        /// <inheritdoc/>
        protected abstract AttributeResult<DateTime> ValidateUtc(object obj, string propertyName, DateTime value);

        /// <inheritdoc/>
        public AttributeResult<DateTime> Validate(object obj, string propertyName, DateTime value)
        {
            var result = ValidateUtc(obj, propertyName, value.ToUniversalTime());
            return new AttributeResult<DateTime>(result.IsValid, value, propertyName);
        }

        public AttributeResult<DateTimeOffset> Validate(object obj, string propertyName, DateTimeOffset value)
        {
            var result = ValidateUtc(obj, propertyName, value.UtcDateTime);
            return new AttributeResult<DateTimeOffset>(result.IsValid, value, propertyName);
        }
#if NET6_0_OR_GREATER
        /// <inheritdoc/>
        public AttributeResult<DateOnly> Validate(object obj, string propertyName, DateOnly value)
        {
            var result = Validate(obj, propertyName, value.ToDateTime(TimeOnly.FromTimeSpan(Now.TimeOfDay), DateTimeKind.Utc));
            return new AttributeResult<DateOnly>(result.IsValid, value, propertyName);
        }
#endif
    }
}
