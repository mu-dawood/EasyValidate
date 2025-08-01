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


        public override string ErrorCode { get; set; } = "ValidEnumValidationError";


        public override AttributeResult Validate(string propertyName, string value)
        {
            bool isValid = !string.IsNullOrWhiteSpace(value) && IsValidEnum(value!);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be a valid enum value.", propertyName);
        }

        private bool IsValidEnum(string value)
        {
            return Enum.IsDefined(EnumType, value);
        }
    }

    public class ValidEnumAttribute<T> : ValidEnumAttribute
        where T : Enum
    {
        public ValidEnumAttribute() : base(typeof(T))
        {
        }
    }
}
