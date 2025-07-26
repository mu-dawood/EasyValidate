using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Base class for string validation attributes. Provides a contract for validating string properties such as emails, URLs, patterns, length, and content.
    /// </summary>
    /// <docs-display-name>String Validation Attributes</docs-display-name>
    /// <docs-icon>Type</docs-icon>
    /// <docs-description>Comprehensive text and string validation attributes for emails, URLs, patterns, length validation, content filtering, and format checking. Essential for validating user input and text data.</docs-description>
    /// <remarks>
    /// Derive from this class to implement custom string validation logic. Used for validating user input and text data.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public abstract class StringValidationAttributeBase : Attribute, IValidationAttribute<string>
    {
        /// <summary>
        /// Gets or sets the name of the validation chain this attribute belongs to. Used to group related validations for advanced scenarios.
        /// </summary>
        public virtual string Chain { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of a method that determines if this validation should be executed. The method must be parameterless and return <c>bool</c>. If null or empty, validation always executes.
        /// </summary>
        public virtual string? ConditionalMethod { get; set; }

        /// <summary>
        /// Gets or sets the execution strategy for this validation attribute. Determines how this validation interacts with the validation chain (e.g., stop on failure, continue, or run conditionally).
        /// </summary>
        public virtual ExecutionStrategy Strategy { get; set; } = ExecutionStrategy.ValidateAndStop;

        /// <summary>
        /// Gets or sets the error code for this validation attribute. Used to identify the type of validation error.
        /// </summary>
        public abstract string ErrorCode { get; set; }

        /// <summary>
        /// Validates the specified string value using the provided service provider for dependency resolution.
        /// </summary>
        /// <param name="serviceProvider">The <see cref="IServiceProvider"/> used to resolve dependencies or services for validation.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="value">The string value to validate.</param>
        /// <returns>An <see cref="AttributeResult"/> indicating success or failure.</returns>
        public abstract AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, string value);
    }

}
