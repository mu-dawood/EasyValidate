using System;
using EasyValidate.Abstraction.Rules;
using EasyValidate.Abstraction.Attributes;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class GreaterThanAttribute : ValidationAttributeBase
    {
        public double Value { get; }
        public GreaterThanAttribute(double value) => Value = value;

        public override IValidationAttributeHandler Handler => new GreaterThanHandler();

        public override bool IsCompatibleType(Type clrType)
        {
            var code = Type.GetTypeCode(clrType);
            return code == TypeCode.Byte || code == TypeCode.SByte || code == TypeCode.Int16 || code == TypeCode.UInt16
                || code == TypeCode.Int32 || code == TypeCode.UInt32 || code == TypeCode.Int64 || code == TypeCode.UInt64
                || code == TypeCode.Single || code == TypeCode.Double || code == TypeCode.Decimal;
        }
    }
}
