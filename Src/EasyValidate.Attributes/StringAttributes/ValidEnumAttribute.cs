using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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
        /// <summary>
        /// Gets the <see cref="Type"/> of the enum to be validated.
        /// </summary>
        public Type EnumType { get; } = enumType;


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "ValidEnumValidationError";


        /// <inheritdoc/>
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

    /// <summary>
    /// Validates that a string matches a valid enum value of type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The enum type to validate against.</typeparam>
    public class ValidEnumAttribute<T> : ValidEnumAttribute where T : Enum
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidEnumAttribute{T}"/> class for the specified enum type.
        /// </summary>
        public ValidEnumAttribute() : base(typeof(T))
        {
        }
    }
}
