using System;
using System.Collections.Generic;

namespace EasyValidate.Abstraction.Attributes
{
    /// <summary>
    /// Specifies that a data field value is required.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RequiredAttribute : ValidationAttributeBase
    {
        /// <summary>
        /// Validates that the value is not null or empty (for strings) or not null (for other types).
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="value">The value to validate.</param>
        /// <returns>An AttributeResult indicating whether the value is valid.</returns>
        public AttributeResult Validate(string propertyName, object value)
        {
            if (value == null)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} is required.",
                    MessageArgs = new object[] { propertyName }
                };
            }

            if (value is string stringValue && string.IsNullOrWhiteSpace(stringValue))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} is required.",
                    MessageArgs = new object[] { propertyName }
                };
            }

            return new AttributeResult { IsValid = true };
        }

        /// <summary>
        /// Gets the error code for the RequiredAttribute.
        /// </summary>
        public override string ErrorCode => "REQUIRED";
    }
}