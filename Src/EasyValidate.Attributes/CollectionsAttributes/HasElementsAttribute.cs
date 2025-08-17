using System;
using System.Collections;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a collection has at least one element.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [HasElements]
    ///     public List&lt;int&gt; Numbers { get; set; } // Valid: [1, 2, 3], Invalid: [], null
    ///     
    ///     [HasElements]
    ///     public string[] Names { get; set; } // Valid: ["John"], Invalid: [], null
    /// }
    /// </code>
    /// </example>
    public class HasElementsAttribute : CollectionValidationAttributeBase
    {




        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "HasElementsValidationError";


        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must contain at least one element.";


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, IEnumerable value)
        {

            foreach (var _ in value)
            {
                return AttributeResult.Success(); // Found at least one element
            }

            return AttributeResult.Fail(ErrorMessage, propertyName);
        }

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
            bool isValid = !string.IsNullOrWhiteSpace(value) && value.Length > 0;
            return isValid
                ? AttributeResult.Success()
                : AttributeResult.Fail(ErrorMessage, propertyName);
        }

    }
}
