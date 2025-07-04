using System;
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
        private static readonly Lazy<NumericAttribute> _instance = new(() => new NumericAttribute());
        public static NumericAttribute Instance => _instance.Value;

        public override string ErrorCode { get; set; } = "NumericValidationError";



        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, string value, out double output)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                output = default;
                return AttributeResult.Fail($"The {propertyName} field must contain only numeric characters.");
            }
            bool isValid = IsNumeric(value, out double number);
            output = number;
            return isValid
                ? AttributeResult.Success()
                : AttributeResult.Fail($"The {propertyName} field must contain only numeric characters.");
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