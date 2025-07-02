using System;
using System.Collections;
using System.Linq;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a collection does not contain the specified value.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [NotContainElement("banned")]
    ///     public List<string> AllowedWords { get; set; } // Valid: ["hello", "world"], Invalid: ["hello", "banned", "world"]
    ///     
    ///     [NotContainElement(0)]
    ///     public int[] NonZeroNumbers { get; set; } // Valid: [1, 2, 3], Invalid: [1, 0, 3]
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// Initializes a new instance of the <see cref="NotContainElementAttribute"/> class.
    /// </remarks>
    /// <param name="forbiddenValue">The value that must not be present in the collection.</param>

    public class NotContainElementAttribute(object forbiddenValue) : CollectionValidationAttributeBase
    {
        /// <summary>
        /// The value that must not be present in the collection.
        /// </summary>
        public object ForbiddenValue { get; } = forbiddenValue;

        /// <summary>
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "DoesNotContainValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must not contain the value {1}.";

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, IEnumerable value, out IEnumerable output)
        {
            bool isValid = !value.Cast<object>().Contains(ForbiddenValue);
            output = value;
            return isValid
                ? AttributeResult.Success()
                : AttributeResult.Fail(ErrorMessage, propertyName, ForbiddenValue);
        }
    }
}
