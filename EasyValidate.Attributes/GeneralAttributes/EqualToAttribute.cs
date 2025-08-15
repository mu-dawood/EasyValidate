using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validation attribute to ensure a property or field is equal to a specified value.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [EqualTo("admin")]
    ///     public string UserType { get; set; } // Valid: "admin", Invalid: "user", "guest"
    ///     
    ///     [EqualTo(18)]
    ///     public int MinimumAge { get; set; } // Valid: 18, Invalid: 17, 19
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// Initializes a new instance of the <see cref="EqualToAttribute"/> class.
    /// </remarks>
    /// <param name="comparisonValue">The value to compare against.</param>
    public class EqualToAttribute(object? comparisonValue) : GeneralValidationAttributeBase
    {
        /// <inheritdoc/>
        public object? ComparisonValue { get; } = comparisonValue;

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "EqualToValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, object? value)
        {
            // Compare the values for equality
            bool isValid = Equals(value, ComparisonValue);
            return isValid
                ? AttributeResult.Success()
                : ComparisonValue is null ? AttributeResult.Fail("The field {0} must be null.", propertyName) : AttributeResult.Fail("The field {0} must be equal to {1}.", propertyName, ComparisonValue);
        }
    }

}