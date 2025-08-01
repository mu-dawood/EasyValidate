using System;
using System.Linq;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string is one of the specified values.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [OneOf("Active", "Inactive", "Pending")]
    ///     public string Status { get; set; } // Valid: "Active", "Inactive", "Pending", Invalid: "Disabled", "Unknown"
    ///     
    ///     [OneOf("Small", "Medium", "Large")]
    ///     public string Size { get; set; } // Valid: "Small", "Medium", "Large", Invalid: "XL", "Tiny"
    /// }
    /// </code>
    /// </example>
    public class OneOfAttribute(params string[] allowedValues) : StringValidationAttributeBase
    {
        public string[] AllowedValues { get; } = allowedValues;


        public override string ErrorCode { get; set; } = "OneOfValidationError";


        public override AttributeResult Validate(string propertyName, string value)
        {
            bool isValid = AllowedValues.Contains(value);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be one of the following values: {1}.", propertyName, string.Join(", ", AllowedValues));
        }
    }
}
