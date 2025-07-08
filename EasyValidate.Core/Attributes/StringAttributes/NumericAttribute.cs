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
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class NumericAttribute : Attribute, IValidationAttribute<string, double>
    {
        public static readonly Lazy<NumericAttribute> Instance = new(() => new NumericAttribute());

        /// <summary>
        /// Gets or sets the name of the validation chain this attribute belongs to.
        /// </summary>
        public string Chain { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of a method that determines if this validation should be executed. The method must be parameterless and return bool. If null or empty, validation always executes.
        /// </summary>
        public string? ConditionalMethod { get; set; }

        /// <summary>
        /// Gets or sets the execution strategy for this validation attribute. Determines how this validation interacts with the validation chain.
        /// </summary>
        public ExecutionStrategy Strategy { get; set; } = ExecutionStrategy.ValidateAndStop;

        /// <summary>
        /// Gets or sets the error code for this validation attribute.
        /// </summary>
        public string ErrorCode { get; set; } = "NumericValidationError";

        /// <inheritdoc/>
        public AttributeResult Validate(object obj, string propertyName, string value, out double output)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                output = default;
                return AttributeResult.Fail($"The {propertyName} field must contain only numeric characters.");
            }
            bool isValid = IsNumeric(value, out output);
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