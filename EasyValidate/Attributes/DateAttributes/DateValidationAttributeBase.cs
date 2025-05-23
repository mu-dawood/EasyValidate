using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Base attribute for date validation attributes. Provides a contract for validating DateTime properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public abstract class DateValidationAttributeBase : ValidationAttributeBase
    {
        /// <summary>
        /// Validates the date property value (nullable).
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="value">The nullable date value to validate.</param>
        /// <returns>An <see cref="AttributeResult"/> indicating the result of validation.</returns>
        public virtual AttributeResult Validate(string propertyName, DateTime? value)
        {
            if (value == null)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} cannot be null.",
                    MessageArgs = new object?[] { propertyName }
                };
            }
            return Validate(propertyName, value.Value);
        }

        /// <summary>
        /// Validates the date property value (non-nullable).
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="value">The date value to validate.</param>
        /// <returns>An <see cref="AttributeResult"/> indicating the result of validation.</returns>
        public abstract AttributeResult Validate(string propertyName, DateTime value);
    }
}
