using System.Collections;
using System.Collections.Generic;
using System.Linq;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that all values in the collection are unique.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [UniqueElements]
    ///     public List<int> Numbers { get; set; } // Valid: [1, 2, 3, 4], Invalid: [1, 2, 2, 3]
    ///     
    ///     [UniqueElements]
    ///     public string[] Tags { get; set; } // Valid: ["red", "blue", "green"], Invalid: ["red", "blue", "red"]
    /// }
    /// </code>
    /// </example>
    public class UniqueElementsAttribute : CollectionValidationAttributeBase
    {
        /// <summary>
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "UniqueValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must contain only unique values.";

        /// Arguments propertyName

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, IEnumerable value, out IEnumerable output)
        {
            var seen = new HashSet<object>();
            output = value; // Set output to the original value
            foreach (var item in value.Cast<object>())
            {
                if (!seen.Add(item))
                {
                    return AttributeResult.Fail(ErrorMessage, propertyName); // Duplicate found
                }
            }
            return AttributeResult.Success(); // All elements are unique
        }
    }
}
