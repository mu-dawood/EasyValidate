using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validation attribute to ensure a property or field is not set to its default value.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [NotDefault]
    ///     public Guid Id { get; set; } // Valid: new Guid("..."), Invalid: Guid.Empty
    ///     
    ///     [NotDefault]
    ///     public int Count { get; set; } // Valid: 1, 5, -3, Invalid: 0
    /// }
    /// </code>
    /// </example>
    public class NotDefaultAttribute : GeneralValidationAttributeBase
    {

        /// <summary>
        /// Gets or sets the nullable behavior for this attribute.
        /// </summary>


        public override string ErrorCode { get; set; } = "NotDefaultValidationError";


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, object? value)
        {

            bool isValid = value != null && !value.Equals(GetDefaultValue(value.GetType()));
            return isValid
                ? AttributeResult.Success()
                : AttributeResult.Fail("The field {0} must not have the default value.", propertyName);
        }

        /// <summary>
        /// Gets the default value for a given type.
        /// </summary>
        /// <param name="type">The type to get the default value for.</param>
        /// <returns>The default value of the specified type, or null if the type is a reference type.</returns>
        private static object? GetDefaultValue(Type type)
        {
            return type.IsValueType ? Activator.CreateInstance(type) : null;
        }

    }
}
