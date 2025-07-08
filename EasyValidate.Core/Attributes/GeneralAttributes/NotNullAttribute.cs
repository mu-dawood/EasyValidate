using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validation attribute to ensure a property or field is not null.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [NotNull]
    ///     public string Name { get; set; } // Valid: "John", "", Invalid: null
    ///     
    ///     [NotNull]
    ///     public List&lt;int&gt; Numbers { get; set; } // Valid: new List&lt;int&gt;(), Invalid: null
    /// }
    /// </code>
    /// </example>
    public sealed class NotNullAttribute : GeneralValidationAttributeBase
    {
        public static readonly Lazy<NotNullAttribute> Instance = new(() => new NotNullAttribute());
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NotNullValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} cannot be null.";


        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, object? value)
        {
            if (value != null)
                return AttributeResult.Success();
            return AttributeResult.Fail(ErrorMessage, propertyName);
        }
    }

}