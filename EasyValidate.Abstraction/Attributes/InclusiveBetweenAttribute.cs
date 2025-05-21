using System;
using EasyValidate.Abstraction.Rules;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class InclusiveBetweenAttribute : ValidationAttributeBase
    {
        public double Min { get; }
        public double Max { get; }

        public InclusiveBetweenAttribute(double min, double max)
        {
            Min = min;
            Max = max;
        }

        public override IValidationAttributeHandler Handler => new InclusiveBetweenHandler();

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
