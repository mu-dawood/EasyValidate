using System.Collections;
using System.Linq;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that the collection has at least a minimum number of elements.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [MinLength(1)]
    ///     public List<string> RequiredItems { get; set; } // Valid: 1+ items, Invalid: 0 items
    ///     
    ///     [MinLength(3)]
    ///     public string[] Options { get; set; } // Valid: 3+ items, Invalid: 0-2 items
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// Initializes a new instance of the <see cref="MinLengthAttribute"/> class.
    /// </remarks>
    /// <param name="minimum">The minimum required length of the collection.</param>

    public class MinLengthAttribute(uint minimum) : CollectionValidationAttributeBase
    {
        /// <summary>
        /// The minimum required length of the collection.
        /// </summary>
        public uint Minimum { get; } = minimum;

        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsInvalid.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "MinLengthValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The field {0} must have a minimum length of {1}.";

        /// <inheritdoc/>
        public override AttributeResult<IEnumerable> Validate(object obj, string propertyName, IEnumerable value)
        {
            bool isValid = value.Cast<object>().Count() >= Minimum;
            return new AttributeResult<IEnumerable>(isValid, value, propertyName, Minimum);
        }
    }
}
