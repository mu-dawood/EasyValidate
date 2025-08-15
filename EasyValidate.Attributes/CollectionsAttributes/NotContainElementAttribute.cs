using System;
using System.Collections;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a collection does not contain the specified value.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [NotContainElement("banned")]
    ///     public List&lt;string&gt; AllowedWords { get; set; } // Valid: ["hello", "world"], Invalid: ["hello", "banned", "world"]
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


        public override string ErrorCode { get; set; } = "DoesNotContainValidationError";


        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must not contain the value {1}.";


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, IEnumerable value)
        {
            foreach (var item in value)
            {
                if (Equals(item, ForbiddenValue))
                {
                    return AttributeResult.Fail(ErrorMessage, propertyName, ForbiddenValue);
                }
            }

            return AttributeResult.Success();
        }

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
#if NET5_0_OR_GREATER
            bool isValid = string.IsNullOrEmpty(value) || !value.Contains(ForbiddenValue?.ToString() ?? "", StringComparison.OrdinalIgnoreCase);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, ForbiddenValue?.ToString() ?? "");
#else
            bool isValid = string.IsNullOrEmpty(value) || value!.IndexOf(ForbiddenValue.ToString(), StringComparison.OrdinalIgnoreCase) == 0;
            return isValid ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, ForbiddenValue);
#endif
        }
    }
}
