using System;
using System.Collections;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Base attribute for collection validation attributes. Provides common validation logic for IEnumerable properties.
    /// </summary>
    /// <docs-display-name>Collection Validation Attributes</docs-display-name>
    /// <docs-icon>List</docs-icon>
    /// <docs-description>Validation attributes for arrays, lists, and collections. Validate collection size, element presence, uniqueness, and content requirements for all enumerable data structures.</docs-description>
    /// <remarks>
    /// Initializes a new instance of the CollectionValidationAttributeBase class.
    /// </remarks>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public abstract class CollectionValidationAttributeBase : Attribute, IValidationAttribute<IEnumerable>, IValidationAttribute<string>
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
        public abstract AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, IEnumerable value);

        public abstract AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, string value);
    }
}

