using System;

namespace EasyValidate.Abstraction.Attributes
{
    /// <summary>
    /// Base class for all validation attributes.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public abstract class ValidationAttributeBase : Attribute
    {
        /// <summary>
        /// Gets the error code associated with the validation attribute.
        /// </summary>
        public abstract string ErrorCode { get; }

        // This class is intentionally left empty as a base for validation attributes.
    }
}
