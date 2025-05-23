using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string has exactly the specified length.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class ExactLengthAttribute(int length) : StringValidationAttributeBase
    {
        /// <summary>
        /// The required length.
        /// </summary>
        public int Length { get; } = length;

        public override string ErrorCode => "ExactLengthValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (value == null || value.Length != Length)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be exactly {1} characters long.",
                    MessageArgs = new object?[] { propertyName, Length }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
