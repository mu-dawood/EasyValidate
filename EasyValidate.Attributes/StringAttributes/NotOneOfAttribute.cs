using System;
using System.Linq;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string is not one of the specified values.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [NotOneOf("admin", "root", "system")]
    ///     public string Username { get; set; } // Valid: "user123", "john_doe", Invalid: "admin", "root", "system"
    ///     
    ///     [NotOneOf("DELETE", "DROP", "TRUNCATE")]
    ///     public string SqlQuery { get; set; } // Valid: "SELECT", "INSERT", "UPDATE", Invalid: "DELETE", "DROP"
    /// }
    /// </code>
    /// </example>
    public class NotOneOfAttribute(params string[] disallowedValues) : StringValidationAttributeBase
    {
        /// <inheritdoc/>
        public string[] DisallowedValues { get; } = disallowedValues;


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NotOneOfValidationError";


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
            bool valid = !DisallowedValues.Contains(value);
            return valid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must not be one of the following values: {1}.", propertyName, string.Join(", ", DisallowedValues));
        }
    }
}
