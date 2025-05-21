using System;
using EasyValidate.Abstraction.Rules;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MinimumLengthAttribute : ValidationAttributeBase
    {
        public int Min { get; }

        public MinimumLengthAttribute(int min) => Min = min;

        public override IValidationAttributeHandler Handler => new MinimumLengthHandler();

        public override bool IsCompatibleType(Type clrType) => clrType == typeof(string);
    }
}
