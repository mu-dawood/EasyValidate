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
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "FirstLetterUpperValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must start with an uppercase letter.";

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                bool isValid = true;
                return new AttributeResult<string>(isValid, value ?? string.Empty, propertyName);
            }
                
            bool valid = char.IsUpper(value![0]);
            return new AttributeResult<string>(valid, value, propertyName);
        }
    }
}
