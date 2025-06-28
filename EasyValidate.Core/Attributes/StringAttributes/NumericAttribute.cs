using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string can be parsed as a number (int, double, decimal, etc).
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Numeric]
    ///     public string Amount { get; set; } // Valid: "123", "45.67", "-89.0", Invalid: "abc", "12.3.4", "hello123"
    ///     
    ///     [Numeric]
    ///     public string Percentage { get; set; } // Valid: "100", "50.5", "0", Invalid: "50%", "fifty"
    /// }
    /// </code>
    /// </example>
    public class NumericAttribute : StringValidationAttributeBase<double>
    {
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NumericValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must contain only numeric characters.";

        /// Arguments propertyName

        /// <inheritdoc/>
        public override AttributeResult<double> Validate(object obj, string propertyName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new AttributeResult<double>(false, default, propertyName);
            }
            bool isValid = IsNumeric(value, out double number);
            return new AttributeResult<double>(isValid, number, propertyName);
        }

        /// <summary>
        /// Checks if the string can be parsed as a number (int, double, decimal, etc).
        /// </summary>
        private static bool IsNumeric(string s, out double number)
        {
            return double.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out number);
        }
    }
}