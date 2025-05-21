using System;
using EasyValidate.Abstraction.Rules;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class LessThanOrEqualToAttribute : ValidationAttributeBase
    {
        public double Value { get; }

        public LessThanOrEqualToAttribute(double value) => Value = value;

        public override IValidationAttributeHandler Handler => new LessThanOrEqualToHandler();

        public override bool IsCompatibleType(Type clrType)
        {
            return clrType == typeof(byte) || clrType == typeof(sbyte) ||
                   clrType == typeof(short) || clrType == typeof(ushort) ||
                   clrType == typeof(int) || clrType == typeof(uint) ||
                   clrType == typeof(long) || clrType == typeof(ulong) ||
                   clrType == typeof(float) || clrType == typeof(double) ||
                   clrType == typeof(decimal);
        }
    }
}
