using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string does not exceed a specified maximum length.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MaxLengthAttribute(int maxLength) : StringValidationAttributeBase
    {
        /// <summary>
        /// The maximum allowed length.
        /// </summary>
        public int MaxLength { get; } = maxLength;

        public override string ErrorCode => "MaxLengthValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (value == null || value.Length > MaxLength)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must not exceed {1} characters.",
                    MessageArgs = new object?[] { propertyName, MaxLength }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
