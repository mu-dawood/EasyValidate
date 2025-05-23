using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string ends with the specified suffix.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EndsWithAttribute(string suffix) : StringValidationAttributeBase
    {
        /// <summary>
        /// The required suffix.
        /// </summary>
        public string Suffix { get; } = suffix;

        public override string ErrorCode => "EndsWithValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (value == null || !value.EndsWith(Suffix))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must end with {1}.",
                    MessageArgs = new object?[] { propertyName, Suffix }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
