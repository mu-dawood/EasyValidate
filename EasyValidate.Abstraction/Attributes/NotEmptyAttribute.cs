using System;
using EasyValidate.Abstraction.Rules;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotEmptyAttribute : ValidationAttributeBase
    {
        public override IValidationAttributeHandler Handler => new NotEmptyHandler();
        public override bool IsCompatibleType(Type clrType) => clrType == typeof(string);
    }
}
