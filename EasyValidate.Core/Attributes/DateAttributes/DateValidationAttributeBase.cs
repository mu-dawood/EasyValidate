using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Base attribute for date validation attributes. Provides a contract for validating DateTime properties.
    /// </summary>
    /// <docs-display-name>Date &amp; Time Validation Attributes</docs-display-name>
    /// <docs-icon>Calendar</docs-icon>
    /// <docs-description>Date and time validation attributes for temporal data validation. Includes date ranges, age verification, business day checks, and time period validation.</docs-description>
    /// <remarks>
    /// Initializes a new instance of the DateValidationAttributeBase class.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public abstract class DateValidationAttributeBase : Attribute,
    IValidationAttribute<DateTime>,
    IValidationAttribute<DateTimeOffset>
#if NET6_0_OR_GREATER
        , IValidationAttribute<DateOnly>
#endif
    {

        /// <inheritdoc/>
        public virtual string Chain { get; set; } = string.Empty;

        /// <inheritdoc/>
        public virtual string? ConditionalMethod { get; set; }
        /// <inheritdoc/>
        public virtual ExecutionStrategy Strategy { get; set; } = ExecutionStrategy.ValidateAndStop;

        /// <inheritdoc/>
        public abstract string ErrorCode { get; set; }

        // /// <inheritdoc/>
        // protected abstract AttributeResult ValidateUtc(object obj, string propertyName, DateTime value);

        /// <inheritdoc/>
        public abstract AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, DateTime value);

        public abstract AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, DateTimeOffset value);
#if NET6_0_OR_GREATER
        /// <inheritdoc/>
        public abstract AttributeResult Validate(IServiceProvider serviceProvider,string propertyName, DateOnly value);
#endif
    }
}