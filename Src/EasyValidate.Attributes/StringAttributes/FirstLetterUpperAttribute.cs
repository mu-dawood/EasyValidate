using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "FirstLetterUpperValidationError";


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
            bool valid = !string.IsNullOrWhiteSpace(value) && char.IsUpper(value[0]);
            return valid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must start with an uppercase letter.", propertyName);
        }
    }
}
