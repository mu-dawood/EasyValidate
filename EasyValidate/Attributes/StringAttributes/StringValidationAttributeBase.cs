using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Base attribute for string validation attributes. Provides a contract for validating string properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public abstract class StringValidationAttributeBase : ValidationAttributeBase
    {
        /// <summary>
        /// Validates the string property value (nullable).
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="value">The nullable string value to validate.</param>
        /// <returns>An <see cref="AttributeResult"/> indicating the result of validation.</returns>
        public abstract AttributeResult Validate(string propertyName, string? value);
    }
}
