using System;
using EasyValidate.Abstraction.Rules;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class CreditCardAttribute : ValidationAttributeBase
    {
        public override IValidationAttributeHandler Handler => new CreditCardHandler();

        public override bool IsCompatibleType(Type clrType) => clrType == typeof(string);
    }
}
