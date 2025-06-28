using System.Collections;
using System.Linq;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that the collection does not exceed a maximum number of elements.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [MaxLength(5)]
    ///     public List<string> Tags { get; set; } // Valid: 0-5 items, Invalid: 6+ items
    ///     
    ///     [MaxLength(10)]
    ///     public string[] Categories { get; set; } // Valid: 0-10 items, Invalid: 11+ items
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// Initializes a new instance of the <see cref="MaxLengthAttribute"/> class.
    /// </remarks>
    /// <param name="maximum">The maximum allowed length of the collection.</param>

    public class MaxLengthAttribute(uint maximum) : CollectionValidationAttributeBase
    {
        /// <summary>
        /// The maximum allowed length of the collection.
        /// </summary>
        public uint Maximum { get; } = maximum;

        /// <summary>
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "MaxLengthValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The field {0} must not exceed a length of {1}.";

        /// <inheritdoc/>
        public override AttributeResult<IEnumerable> Validate(object obj, string propertyName, IEnumerable value)
        {
            bool isValid = value.Cast<object>().Count() <= Maximum;
            return new AttributeResult<IEnumerable>(isValid, value, propertyName, Maximum);
        }
    }
}
