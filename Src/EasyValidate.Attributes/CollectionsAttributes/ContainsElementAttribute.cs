using System;
using System.Collections;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a collection contains the specified value.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [ContainsElement("admin")]
    ///     public List&lt;string&gt; Roles { get; set; } // Valid: ["user", "admin", "guest"], Invalid: ["user", "guest"]
    ///     
    ///     [ContainsElement(1)]
    ///     public int[] Numbers { get; set; } // Valid: [1, 2, 3], Invalid: [2, 3, 4]
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// Initializes a new instance of the <see cref="ContainsElementAttribute"/> class.
    /// </remarks>
    /// <param name="expectedValue">The value that must be present in the collection.</param>
    public class ContainsElementAttribute(object expectedValue) : CollectionValidationAttributeBase
    {
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsInvalid.
        /// </summary>

        /// <summary>
        /// The value that must be present in the collection.
        /// </summary>
        public object ExpectedValue { get; } = expectedValue;


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "ContainsValidationError";


        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must contain the value {1}.";




        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, IEnumerable value)
        {
            foreach (var item in value)
            {
                if (Equals(item, ExpectedValue))
                {
                    return AttributeResult.Success();
                }
            }
            return AttributeResult.Fail(ErrorMessage, propertyName, ExpectedValue);
        }

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
#if NET5_0_OR_GREATER
            bool isValid = value.Contains(ExpectedValue?.ToString() ?? "", StringComparison.OrdinalIgnoreCase);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, ExpectedValue?.ToString()?? "");
#else
            bool isValid = value.IndexOf(ExpectedValue.ToString(), StringComparison.OrdinalIgnoreCase) >= 0;
            return isValid ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, ExpectedValue);
#endif
        }

    }
}
