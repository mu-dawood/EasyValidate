using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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
    public class EqualToAttribute(object comparisonValue) : GeneralValidationAttributeBase
    {
        /// <summary>
        /// Gets the value to compare against.
        /// </summary>
        public object ComparisonValue { get; } = comparisonValue;
        /// <inheritdoc/>
        public override ExecutionStrategy Strategy { get; set; } = ExecutionStrategy.ValidateAndStop;

        /// <summary>
        /// Gets or sets the nullable behavior for this attribute.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "EqualToValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The field {0} must be equal to {1}.";

        /// Arguments propertyName, ComparisonValue

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, object? value)
        {
            // Compare the values for equality
            bool isValid = Equals(value, ComparisonValue);
            return isValid
                ? AttributeResult.Success()
                : AttributeResult.Fail(ErrorMessage, propertyName, ComparisonValue);
        }
    }

}