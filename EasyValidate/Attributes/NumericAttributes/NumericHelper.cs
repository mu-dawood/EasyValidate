using System;

namespace EasyValidate.Attributes
{
    internal static class NumericHelper
    {
        public static bool IsNumericType<T>(T value)
        {
            var type = typeof(T);
            var underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
            {
                type = underlyingType;
            }
            var code = Type.GetTypeCode(type);
            return code == TypeCode.Byte   || code == TypeCode.SByte  ||
                   code == TypeCode.Int16  || code == TypeCode.UInt16 ||
                   code == TypeCode.Int32  || code == TypeCode.UInt32 ||
                   code == TypeCode.Int64  || code == TypeCode.UInt64 ||
                   code == TypeCode.Single || code == TypeCode.Double ||
                   code == TypeCode.Decimal;
        }
    }
}
