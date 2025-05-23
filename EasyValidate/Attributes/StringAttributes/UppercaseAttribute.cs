using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string is all uppercase.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class UppercaseAttribute : StringValidationAttributeBase
    {
        public override string ErrorCode => "UppercaseValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (!string.IsNullOrWhiteSpace(value))
            {
                var upper = value!;
                if (upper != upper.ToUpperInvariant())
                {
                    return new AttributeResult
                    {
                        IsValid = false,
                        Message = "The field {0} must be all uppercase.",
                        MessageArgs = [propertyName]
                    };
                }
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
