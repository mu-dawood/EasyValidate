using System;
using System.Collections;
using System.Linq;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that the specified value appears at most once (zero or one time) in the collection.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [SingleOrNone("primary")]
    ///     public List&lt;string&gt; Categories { get; set; } // Valid: ["secondary", "primary"], ["secondary"], Invalid: ["primary", "secondary", "primary"]
    ///     
    ///     [SingleOrNone(1)]
    ///     public int[] Flags { get; set; } // Valid: [0, 1, 2], [0, 2], Invalid: [1, 0, 1]
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// Initializes a new instance of the <see cref="SingleOrNoneAttribute"/> class.
    /// </remarks>
    /// <param name="value">The value that must appear at most once in the collection.</param>

    public class SingleOrNoneAttribute(object value) : CollectionValidationAttributeBase
    {
        /// <summary>
        /// The value that must appear at most once in the collection.
        /// </summary>
        public object Value { get; } = value;

        /// <summary>
        /// </summary>


        public override string ErrorCode { get; set; } = "SingleOrNoneValidationError";


        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must contain the value {1} at most once.";

        /// Arguments propertyName, Value


        public override AttributeResult Validate(string propertyName, IEnumerable value)
        {
            bool found = false;
            foreach (var item in value)
            {
                if (Equals(item, Value))
                {
                    if (found)
                    {
                        return AttributeResult.Fail(ErrorMessage, propertyName, Value); // Found more than once
                    }
                    found = true;
                }
            }
            // Valid if not found (empty or value not present) or found exactly once
            return AttributeResult.Success();
        }

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
            // For string, we can only check if the value is present or not
            bool isValid = !string.IsNullOrWhiteSpace(value) && value.Count(c => c.ToString() == Value.ToString()) <= 1;
            return isValid
                ? AttributeResult.Success()
                : AttributeResult.Fail(ErrorMessage, propertyName, Value);
        }
    }
}
