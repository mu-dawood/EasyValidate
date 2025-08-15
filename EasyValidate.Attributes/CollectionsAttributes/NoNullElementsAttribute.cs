using System;
using System.Collections;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a collection does not contain any null elements.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [NoNullElements]
    ///     public List&lt;string&gt; Names { get; set; } // Valid: ["Alice", "Bob"], Invalid: ["Alice", null, "Bob"]
    ///     
    ///     [NoNullElements]
    ///     public string[] Tags { get; set; } // Valid: ["tag1", "tag2"], Invalid: ["tag1", null]
    /// }
    /// </code>
    /// </example>
    public class NoNullElementsAttribute : CollectionValidationAttributeBase
    {




        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NoNullElementsValidationError";


        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must not contain null elements.";

        /// Arguments propertyName


        public override AttributeResult Validate(string propertyName, IEnumerable value)
        {
            foreach (var item in value)
            {
                if (item == null)
                {
                    return AttributeResult.Fail(ErrorMessage, propertyName);
                }
            }
            return AttributeResult.Success();
        }

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {

            if (string.IsNullOrWhiteSpace(value))
            {
                return AttributeResult.Success(); // Empty string is valid
            }

            foreach (var item in value.Split(','))
            {
                if (item == null || item.Trim().Length == 0)
                {
                    return AttributeResult.Fail(ErrorMessage, propertyName);
                }
            }
            return AttributeResult.Success(); // All characters are non-null
        }
    }
}
