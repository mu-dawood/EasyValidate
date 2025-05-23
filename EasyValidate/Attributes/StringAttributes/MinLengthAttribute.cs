using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string is at least a specified minimum length.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MinLengthAttribute(int minLength) : StringValidationAttributeBase
    {
        /// <summary>
        /// The minimum required length.
        /// </summary>
        public int MinLength { get; } = minLength;

        public override string ErrorCode => "MinLengthValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (value == null || value.Length < MinLength)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be at least {1} characters long.",
                    MessageArgs = [propertyName, MinLength]
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
