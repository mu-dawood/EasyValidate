using System;
using System.Linq;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string contains only ASCII characters (0x00-0x7F).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class AsciiAttribute : StringValidationAttributeBase
    {
        public override string ErrorCode => "AsciiValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (string.IsNullOrEmpty(value) || value!.Any(c => c > 127))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must contain only ASCII characters.",
                    MessageArgs = new object?[] { propertyName }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
