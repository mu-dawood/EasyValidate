using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string is a valid GUID (Globally Unique Identifier).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class GuidAttribute : StringValidationAttributeBase
    {
        public override string ErrorCode => "GuidValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || !System.Guid.TryParse(value, out _))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be a valid GUID.",
                    MessageArgs = [propertyName]
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
