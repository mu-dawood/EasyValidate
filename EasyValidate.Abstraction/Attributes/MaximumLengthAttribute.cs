using System;
using EasyValidate.Abstraction.Rules;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MaximumLengthAttribute : ValidationAttributeBase
    {
        public int Max { get; }

        public MaximumLengthAttribute(int max) => Max = max;

        public override IValidationAttributeHandler Handler => new MaximumLengthHandler();

        public override bool IsCompatibleType(Type clrType) => clrType == typeof(string);
    }
}
