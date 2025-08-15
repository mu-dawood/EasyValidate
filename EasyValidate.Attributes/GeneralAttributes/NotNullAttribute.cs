using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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
        /// <summary>
        /// Singleton instance for reuse to avoid creating multiple identical instances.
        /// </summary>



        public override string ErrorCode { get; set; } = "NotNullValidationError";


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, object? value)
        {
            return value is not null
                ? AttributeResult.Success()
                : AttributeResult.Fail("The field {0} cannot be null.", propertyName);
        }
    }

}