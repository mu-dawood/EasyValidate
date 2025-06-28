using System.Collections;
using System.Linq;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that the specified value appears exactly once in the collection.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Single("admin")]
    ///     public List<string> Roles { get; set; } // Valid: ["user", "admin", "guest"], Invalid: ["admin", "admin"], ["user", "guest"]
    ///     
    ///     [Single(1)]
    ///     public int[] Numbers { get; set; } // Valid: [1, 2, 3], Invalid: [1, 1, 2], [2, 3, 4]
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SingleAttribute"/> class.
    /// </remarks>
    /// <param name="value">The value that must appear exactly once in the collection.</param>

    public class SingleAttribute(object value) : CollectionValidationAttributeBase
    {
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsInvalid.
        /// </summary>

        /// <summary>
        /// The value that must appear exactly once in the collection.
        /// </summary>
        public object Value { get; } = value;

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "SingleValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The field {0} must contain the value {1} exactly once.";

        /// Arguments propertyName, Value

        /// <inheritdoc/>
        public override AttributeResult<IEnumerable> Validate(object obj, string propertyName, IEnumerable value)
        {
            bool found = false;
            foreach (var item in value.Cast<object>())
            {
                if (Equals(item, Value))
                {
                    if (found)
                    {
                        return new AttributeResult<IEnumerable>(false, value, propertyName, Value); // Found more than once
                    }
                    found = true;
                }
            }
            return new AttributeResult<IEnumerable>(found, value, propertyName, Value); // Must be found exactly once
        }
    }
}
