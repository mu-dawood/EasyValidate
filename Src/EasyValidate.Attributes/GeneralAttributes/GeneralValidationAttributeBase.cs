using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Base class for all general validation attributes.
    /// </summary>
    /// <docs-display-name>General Validation Attributes</docs-display-name>
    /// <docs-icon>Settings</docs-icon>
    /// <docs-description>General-purpose validation attributes that work across multiple data types. Includes required field validation, conditional validation, custom validation, and cross-property validation rules.</docs-description>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public abstract class GeneralValidationAttributeBase : Attribute, IValidationAttribute<object?>
    {


        /// <inheritdoc/>
        public virtual string Chain { get; set; } = string.Empty;

        /// <inheritdoc/>
        public virtual string? ConditionalMethod { get; set; }

        /// <inheritdoc/>
        public abstract string ErrorCode { get; set; }

        // 
        /// <inheritdoc/>
        public abstract AttributeResult Validate(string propertyName, object? value);
    }
}
