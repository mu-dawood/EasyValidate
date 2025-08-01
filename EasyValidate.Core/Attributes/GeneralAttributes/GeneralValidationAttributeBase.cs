using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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


        public virtual string Chain { get; set; } = string.Empty;

        public virtual string? ConditionalMethod { get; set; }

        public abstract string ErrorCode { get; set; }

        // 
        public abstract AttributeResult Validate(string propertyName, object? value);
    }
}
