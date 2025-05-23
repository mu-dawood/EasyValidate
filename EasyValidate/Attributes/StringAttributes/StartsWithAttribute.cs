using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string starts with the specified prefix.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class StartsWithAttribute(string prefix) : StringValidationAttributeBase
    {
        /// <summary>
        /// The required prefix.
        /// </summary>
        public string Prefix { get; } = prefix;

        public override string ErrorCode => "StartsWithValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (value == null || !value.StartsWith(Prefix))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must start with {1}.",
                    MessageArgs = [propertyName, Prefix]
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
