using System;
using EasyValidate.Abstraction.Rules;
using EasyValidate.Abstraction.Attributes;

namespace EasyValidate.Abstraction.Attributes
{
    public class NotNullAttribute : ValidationAttributeBase
    {
        public override IValidationAttributeHandler Handler => new NotNullHandler();
        // applicable to any type
        public override bool IsCompatibleType(Type clrType) => true;
    }
}
