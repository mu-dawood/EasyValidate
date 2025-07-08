using System;
using System.Linq;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string is all lowercase.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Lowercase]
    ///     public string Username { get; set; } // Valid: "john_doe", "user123", Invalid: "John_Doe", "USER123"
    ///     
    ///     [Lowercase]
    ///     public string EmailPrefix { get; set; } // Valid: "john.doe", "admin", Invalid: "John.Doe", "ADMIN"
    /// }
    /// </code>
    /// </example>
    public class LowercaseAttribute : StringValidationAttributeBase
    {
        public static readonly Lazy<LowercaseAttribute> Instance = new(() => new LowercaseAttribute());
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "LowercaseValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, string value)
        {
            bool isValid = value.ToLowerInvariant().Equals(value, StringComparison.InvariantCulture);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be lowercase.", propertyName);
        }
    }
}
