using System;
using EasyValidate.Abstraction.Rules;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RequiredAttribute : ValidationAttributeBase
    {
        public override IValidationAttributeHandler Handler => new RequiredHandler();

        public override bool IsCompatibleType(Type clrType) => true;
    }
}
