using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validation attribute to ensure a property or field is not equal to a specified value.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [NotEqualTo("admin")]
    ///     public string Username { get; set; } // Valid: "user123", "guest", Invalid: "admin"
    ///     
    ///     [NotEqualTo(0)]
    ///     public int Score { get; set; } // Valid: 1, 100, -5, Invalid: 0
    /// }
    /// </code>
    /// </example>
    /// <remarks>
    /// Initializes a new instance of the <see cref="NotEqualToAttribute"/> class.
    /// </remarks>
    /// <param name="comparisonValue">The value to compare against.</param>
    public class NotEqualToAttribute(object? comparisonValue) : GeneralValidationAttributeBase
    {

        /// <summary>
        /// Gets the value to compare against.
        /// </summary>
        public object? ComparisonValue { get; } = comparisonValue;

        /// <summary>
        /// Gets or sets the nullable behavior for this attribute.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NotEqualToValidationError";

        /// Arguments propertyName, ComparisonValue

        /// <inheritdoc/>
        public override AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, object? value)
        {
            bool isValid = !Equals(value, ComparisonValue);
            return isValid
                ? AttributeResult.Success()
                : AttributeResult.Fail("The field {0} must not be equal to {1}.", propertyName, ComparisonValue);
        }
    }
}