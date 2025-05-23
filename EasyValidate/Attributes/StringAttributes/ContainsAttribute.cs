using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string contains a specific substring.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ContainsAttribute(string substring) : StringValidationAttributeBase
    {
        /// <summary>
        /// The substring that must be present.
        /// </summary>
        public string Substring { get; } = substring;

        public override string ErrorCode => "StringContainsValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (string.IsNullOrEmpty(value) || !value!.Contains(Substring))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must contain '{1}'.",
                    MessageArgs = new object?[] { propertyName, Substring }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
