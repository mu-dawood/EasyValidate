using System;
using System.Globalization;

using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string can be parsed as a number (int, double, decimal, etc).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NumericAttribute : StringValidationAttributeBase
    {
        /// <inheritdoc/>
        public override string ErrorCode => "NumericValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || !IsNumeric(value!))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be a valid number.",
                    MessageArgs = [propertyName]
                };
            }
            return new AttributeResult { IsValid = true };
        }

        /// <summary>
        /// Checks if the string can be parsed as a number (int, double, decimal, etc).
        /// </summary>
        private static bool IsNumeric(string s)
        {
            return double.TryParse(s, System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out _);
        }
    }
}