using System;
using EasyValidate.Abstraction.Rules;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class LengthAttribute : ValidationAttributeBase
    {
        public int Min { get; }
        public int Max { get; }

        public LengthAttribute(int min, int max)
        {
            Min = min;
            Max = max;
        }

        public override IValidationAttributeHandler Handler => new LengthHandler();

        public override bool IsCompatibleType(Type clrType) => clrType == typeof(string);
    }
}
