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
        public override string ErrorMessage { get; set; } = "The field {0} must contain only unique values.";

        /// Arguments propertyName

        /// <inheritdoc/>
        public override AttributeResult<IEnumerable> Validate(object obj, string propertyName, IEnumerable value)
        {
            var seen = new HashSet<object>();
            foreach (var item in value.Cast<object>())
            {
                if (!seen.Add(item))
                {
                    return new AttributeResult<IEnumerable>(false, value, propertyName); // Duplicate found
                }
            }
            return new AttributeResult<IEnumerable>(true, value, propertyName);
        }
    }
}
