using System;
using System.Collections;
using System.Collections.Generic;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that all values in the collection are unique.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [UniqueElements]
    ///     public List&lt;int&gt; Numbers { get; set; } // Valid: [1, 2, 3, 4], Invalid: [1, 2, 2, 3]
    ///     
    ///     [UniqueElements]
    ///     public string[] Tags { get; set; } // Valid: ["red", "blue", "green"], Invalid: ["red", "blue", "red"]
    /// }
    /// </code>
    /// </example>
    public class UniqueElementsAttribute : CollectionValidationAttributeBase
    {
        public static readonly Lazy<UniqueElementsAttribute> Instance = new(() => new UniqueElementsAttribute());


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "UniqueValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must contain only unique values.";

        /// Arguments propertyName

        /// <inheritdoc/>
        public override AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, IEnumerable value)
        {
            var seen = new HashSet<object>();
            foreach (var item in value)
            {
                if (!seen.Add(item))
                {
                    return AttributeResult.Fail(ErrorMessage, propertyName); // Duplicate found
                }
            }
            return AttributeResult.Success(); // All elements are unique
        }
        /// <inheritdoc/>
        public override AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, string value)
        {
            // For string, we can only check if the characters are unique
            var seen = new HashSet<char>();
            foreach (var item in value)
            {
                if (!seen.Add(item))
                {
                    return AttributeResult.Fail(ErrorMessage, propertyName); // Duplicate character found
                }
            }
            return AttributeResult.Success(); // All characters are unique 
        }
    }
}
