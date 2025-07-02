using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that the first letter of a string is uppercase.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [FirstLetterUpper]
    ///     public string FirstName { get; set; } // Valid: "John", "Mary", "Alex", Invalid: "john", "mary", "123abc"
    ///     
    ///     [FirstLetterUpper]
    ///     public string Title { get; set; } // Valid: "Manager", "Director", Invalid: "manager", "director"
    /// }
    /// </code>
    /// </example>
    public class FirstLetterUpperAttribute : StringValidationAttributeBase
    {
        private static readonly Lazy<FirstLetterUpperAttribute> _instance = new(() => new FirstLetterUpperAttribute());
        public static FirstLetterUpperAttribute Instance => _instance.Value;
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "FirstLetterUpperValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, string value, out string output)
        {
            if (string.IsNullOrEmpty(value))
            {
                output = value ?? string.Empty;
                return AttributeResult.Success();
            }
            bool valid = char.IsUpper(value[0]);
            output = value;
            return valid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must start with an uppercase letter.", propertyName);
        }
    }
}
