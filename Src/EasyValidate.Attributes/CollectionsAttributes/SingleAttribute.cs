using System;
using System.Collections;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that the specified value appears exactly once in the collection.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Single("admin")]
    ///     public List&lt;string&gt; Roles { get; set; } // Valid: ["user", "admin", "guest"], Invalid: ["admin", "admin"], ["user", "guest"]
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
        public string ErrorMessage { get; set; } = "The field {0} must contain the value {1} exactly once.";

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
                        return AttributeResult.Fail(ErrorMessage, propertyName, Value);
                    }
                    found = true;
                }
            }
            return found
                ? AttributeResult.Success()
                : AttributeResult.Fail(ErrorMessage, propertyName, Value);
        }

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
            bool found = false;
            foreach (var item in value.Split(','))
            {
                if (item.Trim() == Value.ToString())
                {
                    if (found)
                    {
                        return AttributeResult.Fail(ErrorMessage, propertyName, Value);
                    }
                    found = true;
                }
            }
            return found
                ? AttributeResult.Success()
                : AttributeResult.Fail(ErrorMessage, propertyName, Value);
        }
    }
}
