using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string matches a valid enum value.
    /// </summary>
    /// <example>
    /// <code>
    /// public enum Status { Active, Inactive, Pending }
    /// 
    /// public partial class Model
    /// {
    ///     [ValidEnum(typeof(Status))]
    ///     public string UserStatus { get; set; } // Valid: "Active", "Inactive", Invalid: "Unknown"
    ///     
    ///     [ValidEnum(typeof(DayOfWeek))]
    ///     public string WorkDay { get; set; } // Valid: "Monday", "Friday", Invalid: "Someday"
    /// }
    /// </code>
    /// </example>
    public class ValidEnumAttribute(Type enumType) : StringValidationAttributeBase
    {
        public Type EnumType { get; } = enumType;

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "ValidEnumValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be a valid value of the {1} enum.";

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {
            bool isValid = Enum.IsDefined(EnumType, value);
            return new AttributeResult<string>(isValid, value , propertyName, EnumType.Name);
        }
    }
}
