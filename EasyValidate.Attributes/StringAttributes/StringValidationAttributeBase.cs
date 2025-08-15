using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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
        /// <inheritdoc/>
        public virtual string Chain { get; set; } = string.Empty;
        /// <inheritdoc/>
        public virtual string? ConditionalMethod { get; set; }
        /// <inheritdoc/>
        public abstract string ErrorCode { get; set; }

        /// <inheritdoc/>
        public abstract AttributeResult Validate(string propertyName, string value);
    }

}
