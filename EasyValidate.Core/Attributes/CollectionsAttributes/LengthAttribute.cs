using System.Collections;
using System.Linq;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that the collection has exactly the specified number of elements.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Length(3)]
    ///     public List<string> Options { get; set; } // Valid: exactly 3 items, Invalid: 2, 4, or any other count
    ///     
    ///     [Length(5)]
    ///     public int[] Numbers { get; set; } // Valid: exactly 5 items, Invalid: 4, 6, or any other count
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// Initializes a new instance of the <see cref="LengthAttribute"/> class.
    /// </remarks>
    /// <param name="length">The required length of the collection.</param>
    public class LengthAttribute(uint length) : CollectionValidationAttributeBase
    {
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsInvalid.
        /// </summary>

        /// <summary>
        /// The required length of the collection.
        /// </summary>
        public uint Length { get; } = length;

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "LengthValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must have exactly {1} elements.";

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, IEnumerable value, out IEnumerable output)
        {
            bool isValid = value.Cast<object>().Count() == Length;
            output = value;
            return isValid ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, Length);
        }
    }
}
