using System;
using EasyValidate.Abstraction.Rules;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotEqualToAttribute : ValidationAttributeBase
    {
        public object Value { get; }

        public NotEqualToAttribute(object value) => Value = value;

        public override IValidationAttributeHandler Handler => new NotEqualToHandler();

        public override bool IsCompatibleType(Type clrType) => true; // Compatible with all types
    }
}
