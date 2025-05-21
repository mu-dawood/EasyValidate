using System;
using EasyValidate.Abstraction.Rules;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class EqualToAttribute : ValidationAttributeBase
    {
        public object Value { get; }

        public EqualToAttribute(object value) => Value = value;

        public override IValidationAttributeHandler Handler => new EqualToHandler();

        public override bool IsCompatibleType(Type clrType) => true; // Compatible with all types
    }
}
