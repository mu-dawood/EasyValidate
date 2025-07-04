using System.Collections;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a collection has at least one element.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [HasElements]
    ///     public List<int> Numbers { get; set; } // Valid: [1, 2, 3], Invalid: [], null
    ///     
    ///     [HasElements]
    ///     public string[] Names { get; set; } // Valid: ["John"], Invalid: [], null
    /// }
    /// </code>
    /// </example>
    public class HasElementsAttribute : CollectionValidationAttributeBase
    {
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsInvalid.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "HasElementsValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must contain at least one element.";

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, IEnumerable value, out IEnumerable output)
        {
            bool isValid = value.GetEnumerator().MoveNext();
            output = value;
            return isValid
                ? AttributeResult.Success()
                : AttributeResult.Fail(ErrorMessage, propertyName);
        }
    }
}
