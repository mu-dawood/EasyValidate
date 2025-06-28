using System;
using System.Linq;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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
        public string[] DisallowedValues { get; } = disallowedValues;

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NotOneOfValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must not be one of the following values: {1}.";

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {   
            bool valid = !DisallowedValues.Contains(value);
            return new AttributeResult<string>(valid, value, propertyName, string.Join(", ", DisallowedValues));
        }
    }
}
