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
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public abstract class StringValidationAttributeBase : Attribute, IValidationAttribute<string>
    {
        /// <summary>
        /// Gets or sets the name of the validation chain this attribute belongs to.
        /// </summary>
        public virtual string Chain { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of a method that determines if this validation should be executed. The method must be parameterless and return bool. If null or empty, validation always executes.
        /// </summary>
        public virtual string? ConditionalMethod { get; set; }

        /// <summary>
        /// Gets or sets the execution strategy for this validation attribute. Determines how this validation interacts with the validation chain.
        /// </summary>
        public virtual ExecutionStrategy Strategy { get; set; } = ExecutionStrategy.ValidateAndStop;

        /// <summary>
        /// Gets or sets the error code for this validation attribute.
        /// </summary>
        public abstract string ErrorCode { get; set; }

        /// <summary>
        /// Validates the specified string value.
        /// </summary>
        /// <param name="obj">The main object being validated.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="value">The string value to validate.</param>
        /// <returns>An <see cref="AttributeResult"/> indicating success or failure.</returns>
        public abstract AttributeResult Validate(object obj, string propertyName, string value);
    }

}
