using System.Collections;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a collection does not contain any null elements.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [NoNullElements]
    ///     public List<string> Names { get; set; } // Valid: ["Alice", "Bob"], Invalid: ["Alice", null, "Bob"]
    ///     
    ///     [NoNullElements]
    ///     public string[] Tags { get; set; } // Valid: ["tag1", "tag2"], Invalid: ["tag1", null]
    /// }
    /// </code>
    /// </example>
    public class NoNullElementsAttribute : CollectionValidationAttributeBase
    {
        /// <summary>
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NoNullElementsValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The field {0} must not contain null elements.";

        /// Arguments propertyName

        /// <inheritdoc/>
        public override AttributeResult<IEnumerable> Validate(object obj, string propertyName, IEnumerable value)
        {
            foreach (var item in value)
            {
                if (item == null)
                {
                    return new AttributeResult<IEnumerable>(false, value, propertyName);
                }
            }
            return new AttributeResult<IEnumerable>(true, value, propertyName);
        }
    }
}
