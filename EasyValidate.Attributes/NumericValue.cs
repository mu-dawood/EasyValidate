using System;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Represents a numeric value in a type-agnostic way, supporting all common numeric types.
    /// </summary>
    internal readonly struct NumericValue
    {
        public double AsDouble { get; }
        public decimal AsDecimal { get; }
        public long AsInt64 { get; }
        public ulong AsUInt64 { get; }
        public float AsFloat { get; }

        public NumericValue(double value)
        {
            AsDouble = value;
            AsDecimal = (decimal)value;
            AsInt64 = (long)value;
            AsUInt64 = (ulong)Math.Max(0, value);
            AsFloat = (float)value;
        }
        public NumericValue(decimal value)
        {
            AsDouble = (double)value;
            AsDecimal = value;
            AsInt64 = (long)value;
            AsUInt64 = (ulong)Math.Max(0, (double)value);
            AsFloat = (float)value;
        }
        public NumericValue(long value)
        {
            AsDouble = value;
            AsDecimal = value;
            AsInt64 = value;
            AsUInt64 = (ulong)Math.Max(0, value);
            AsFloat = value;
        }
        public NumericValue(ulong value)
        {
            AsDouble = value;
            AsDecimal = value;
            AsInt64 = (long)Math.Min(long.MaxValue, value);
            AsUInt64 = value;
            AsFloat = value;
        }
        public NumericValue(int value) : this((long)value) { }
        public NumericValue(uint value) : this((ulong)value) { }
        public NumericValue(short value) : this((long)value) { }
        public NumericValue(ushort value) : this((ulong)value) { }
        public NumericValue(byte value) : this((ulong)value) { }
        public NumericValue(sbyte value) : this((long)value) { }
        public NumericValue(float value) : this((double)value) { }

        public static implicit operator NumericValue(int value) => new(value);
        public static implicit operator NumericValue(long value) => new(value);
        public static implicit operator NumericValue(float value) => new(value);
        public static implicit operator NumericValue(double value) => new(value);
        public static implicit operator NumericValue(decimal value) => new(value);
        public static implicit operator NumericValue(short value) => new(value);
        public static implicit operator NumericValue(ushort value) => new(value);
        public static implicit operator NumericValue(uint value) => new(value);
        public static implicit operator NumericValue(ulong value) => new(value);
        public static implicit operator NumericValue(byte value) => new(value);
        public static implicit operator NumericValue(sbyte value) => new(value);
    }
}
