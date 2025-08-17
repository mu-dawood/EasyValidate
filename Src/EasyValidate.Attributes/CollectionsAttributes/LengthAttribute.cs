using System;
using System.Collections;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that the collection has exactly the specified number of elements.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Length(3)]
    ///     public List&lt;string&gt; Options { get; set; } // Valid: exactly 3 items, Invalid: 2, 4, or any other count
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
        public override AttributeResult Validate(string propertyName, IEnumerable value)
        {
            if (value is ICollection collection)
            {
                return collection.Count == Length
                    ? AttributeResult.Success()
                    : AttributeResult.Fail(ErrorMessage, propertyName, Length);
            }

            int count = 0;
            foreach (var _ in value)
            {
                count++;
                if (count > Length)
                    break;
            }

            return count == Length
                ? AttributeResult.Success()
                : AttributeResult.Fail(ErrorMessage, propertyName, Length);
        }

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
            return value.Length == Length
                ? AttributeResult.Success()
                : AttributeResult.Fail(ErrorMessage, propertyName, Length);
        }
    }
}
