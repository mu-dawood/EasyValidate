using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Base attribute for string validation attributes. Provides a contract for validating string properties.
    /// </summary>
    /// <docs-display-name>String Validation Attributes</docs-display-name>
    /// <docs-icon>Type</docs-icon>
    /// <docs-description>Comprehensive text and string validation attributes for emails, URLs, patterns, length validation, content filtering, and format checking. Essential for validating user input and text data.</docs-description>
    /// <remarks>
    /// Initializes a new instance of the StringValidationAttributeBase class.
    /// </remarks>
    /// <param name="followUpValidations">Array of validation attribute names that should only be validated after this validation succeeds.</param>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public abstract class StringValidationAttributeBase<TOut> : Attribute, IValidationAttribute<string, TOut>
    {

        /// <inheritdoc/>
        public virtual string Chain { get; set; } = string.Empty;
        /// <inheritdoc/>
        public virtual string? ConditionalMethod { get; set; }

        /// <inheritdoc/>
        public virtual ExecutionStrategy Strategy { get; set; } = ExecutionStrategy.ValidateAndStop;

        /// <inheritdoc/>
        public abstract string ErrorCode { get; set; }

        /// <inheritdoc/>
        public abstract AttributeResult Validate(object obj, string propertyName, string value, out TOut output);
    }

    public abstract class StringValidationAttributeBase : StringValidationAttributeBase<string>
    {

    }
}
