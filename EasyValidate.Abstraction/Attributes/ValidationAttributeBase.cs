using System;
using EasyValidate.Abstraction.Rules;

namespace EasyValidate.Abstraction.Attributes
{
    /// <summary>
    /// Base class for all validation attributes.
    /// Provides a handler and optional type-checking logic.
    /// </summary>
    public abstract class ValidationAttributeBase : Attribute
    {
        /// <summary>
        /// Returns the rule handler instance for this attribute.
        /// </summary>
        public abstract IValidationAttributeHandler Handler { get; }

        /// <summary>
        /// Checks at compile time (in generator) whether this attribute is valid on a given CLR type.
        /// Override in derived attributes to enforce type compatibility.
        /// </summary>
        public virtual bool IsCompatibleType(Type clrType) => true;
    }
}
