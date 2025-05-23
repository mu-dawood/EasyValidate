using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string is all lowercase.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class LowercaseAttribute : StringValidationAttributeBase
    {
        public override string ErrorCode => "LowercaseValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var lower = value!;
                if (lower != lower.ToLowerInvariant())
                {
                    return new AttributeResult
                    {
                        IsValid = false,
                        Message = "The field {0} must be all lowercase.",
                        MessageArgs = [propertyName]
                    };
                }
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
