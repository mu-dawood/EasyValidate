using System;
using System.Collections;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that the collection has at least a minimum number of elements.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [MinLength(1)]
    ///     public List&lt;string&gt; RequiredItems { get; set; } // Valid: 1+ items, Invalid: 0 items
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


        public override string ErrorCode { get; set; } = "MinLengthValidationError";


        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must have a minimum length of {1}.";


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, IEnumerable value)
        {
            if (value is ICollection collection)
            {
                return collection.Count >= Minimum
                    ? AttributeResult.Success()
                    : AttributeResult.Fail(ErrorMessage, propertyName, Minimum);
            }

            int count = 0;
            foreach (var _ in value)
            {
                count++;
                if (count >= Minimum)
                {
                    return AttributeResult.Success();
                }
            }

            return AttributeResult.Fail(ErrorMessage, propertyName, Minimum);
        }

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
            bool isValid = value.Length >= Minimum;
            return isValid
                ? AttributeResult.Success()
                : AttributeResult.Fail(ErrorMessage, propertyName, Minimum);
        }
    }
}
