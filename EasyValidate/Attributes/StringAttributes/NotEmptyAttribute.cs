using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string is not null, empty, or whitespace.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotEmptyAttribute : StringValidationAttributeBase
    {
        public override string ErrorCode => "NotEmptyValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} cannot be empty.",
                    MessageArgs = new object?[] { propertyName }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}