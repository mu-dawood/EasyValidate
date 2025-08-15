using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Base attribute for numeric validation attributes. Provides a contract for validating numeric properties.
    /// </summary>
    /// <docs-display-name>Numeric Validation Attributes</docs-display-name>
    /// <docs-icon>Hash</docs-icon>
    /// <docs-description>Comprehensive numeric validation attributes for integers, decimals, and floating-point numbers. Includes range validation, mathematical operations, divisibility checks, and numeric constraints.</docs-description>
    /// <remarks>
    /// Initializes a new instance of the NumericValidationAttributeBase class.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public abstract class NumericValidationAttributeBase : Attribute
    {

        /// <inheritdoc/>
        public virtual string? ConditionalMethod { get; set; }

        /// <inheritdoc/>
        public abstract string ErrorCode { get; set; }


        /// <inheritdoc/>
        public virtual string Chain { get; set; } = string.Empty;
    }
}
