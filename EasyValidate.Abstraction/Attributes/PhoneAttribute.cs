using System;
using EasyValidate.Abstraction.Rules;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PhoneAttribute : ValidationAttributeBase
    {
        public override IValidationAttributeHandler Handler => new PhoneHandler();

        public override bool IsCompatibleType(Type clrType) => clrType == typeof(string);
    }
}
