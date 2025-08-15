using System;
using System.Collections;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that the collection does not exceed a maximum number of elements.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [MaxLength(5)]
    ///     public List&lt;string&gt; Tags { get; set; } // Valid: 0-5 items, Invalid: 6+ items
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


        public override string ErrorCode { get; set; } = "MaxLengthValidationError";


        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must not exceed a length of {1}.";


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, IEnumerable value)
        {
            if (value is ICollection collection)
            {
                if (collection.Count <= Maximum)
                    return AttributeResult.Success();

                return AttributeResult.Fail(ErrorMessage, propertyName, Maximum);
            }

            int count = 0;
            foreach (var _ in value)
            {
                count++;
                if (count > Maximum)
                {
                    return AttributeResult.Fail(ErrorMessage, propertyName, Maximum);
                }
            }

            return AttributeResult.Success();
        }

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
            return value.Length <= Maximum
                ? AttributeResult.Success()
                : AttributeResult.Fail(ErrorMessage, propertyName, Maximum);
        }
    }
}
