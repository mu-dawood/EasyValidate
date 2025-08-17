using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public class NumericAttribute : Attribute, IValidationAttribute<string, double>
    {


        /// <summary>
        /// Gets or sets the name of the validation chain this attribute belongs to.
        /// </summary>
        public string Chain { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of a method that determines if this validation should be executed. The method must be parameterless and return bool. If null or empty, validation always executes.
        /// </summary>
        public string? ConditionalMethod { get; set; }
        /// <summary>
        /// Gets or sets the error code for this validation attribute.
        /// </summary>
        public string ErrorCode { get; set; } = "NumericValidationError";


        /// <inheritdoc/>
        public AttributeResult<double> Validate(string propertyName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return AttributeResult.Fail<double>("The {0} field must contain only numeric characters.", [propertyName]);
            }
            bool isValid = IsNumeric(value, out double output);
            return isValid
                ? AttributeResult.Success(output)
                : AttributeResult.Fail<double>("The {0} field must contain only numeric characters.", [propertyName]);
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