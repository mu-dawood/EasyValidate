using System;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Represents a numeric value in a type-agnostic way, supporting all common numeric types.
    /// </summary>
    internal readonly struct NumericValue
    {
        private readonly Func<double> _asDoubleFunc;
        public double AsDouble => _asDoubleFunc?.Invoke() ?? 0.0;
        private readonly Func<decimal> _asDecimalFunc;
        public decimal AsDecimal => _asDecimalFunc?.Invoke() ?? 0.0m;
        private readonly Func<long> _asInt64Func;
        public long AsInt64 => _asInt64Func?.Invoke() ?? 0L;
        private readonly Func<ulong> _asUInt64Func;
        public ulong AsUInt64 => _asUInt64Func?.Invoke() ?? 0UL;
        private readonly Func<float> _asFloatFunc;
        public float AsFloat => _asFloatFunc?.Invoke() ?? 0.0f;

        public NumericValue(double value)
        {
            _asDecimalFunc = () => (decimal)value;
            _asDoubleFunc = () => value;
            _asInt64Func = () => (long)value;
            _asUInt64Func = () => (ulong)Math.Max(0, (int)value);
            _asFloatFunc = () => (float)value;
        }
        public NumericValue(decimal value)
        {
            _asDecimalFunc = () => value;
            _asDoubleFunc = () => (double)value;
            _asInt64Func = () => (long)value;
            _asUInt64Func = () => (ulong)Math.Max(0, (int)value);
            _asFloatFunc = () => (float)value;
        }
        public NumericValue(long value)
        {
            _asDecimalFunc = () => value;
            _asDoubleFunc = () => value;
            _asInt64Func = () => value;
            _asUInt64Func = () => (ulong)Math.Max(0, (int)value);
            _asFloatFunc = () => value;
        }
        public NumericValue(ulong value)
        {
            _asDecimalFunc = () => value;
            _asDoubleFunc = () => value;
            _asInt64Func = () => (long)value;
            _asUInt64Func = () => value;
            _asFloatFunc = () => value;
        }
        public NumericValue(int value)
        {
            _asDecimalFunc = () => value;
            _asDoubleFunc = () => value;
            _asInt64Func = () => value;
            _asUInt64Func = () => (ulong)Math.Max(0, value);
            _asFloatFunc = () => value;
        }
        public NumericValue(uint value)
        {
            _asDecimalFunc = () => value;
            _asDoubleFunc = () => value;
            _asInt64Func = () => value;
            _asUInt64Func = () => value;
            _asFloatFunc = () => value;
        }
        public NumericValue(short value)
        {
            _asDecimalFunc = () => value;
            _asDoubleFunc = () => value;
            _asInt64Func = () => value;
            _asUInt64Func = () => (ulong)Math.Max(0, (int)value);
            _asFloatFunc = () => value;
        }
        public NumericValue(ushort value)
        {
            _asDecimalFunc = () => value;
            _asDoubleFunc = () => value;
            _asInt64Func = () => value;
            _asUInt64Func = () => value;
            _asFloatFunc = () => value;
        }
        public NumericValue(byte value)
        {
            _asDecimalFunc = () => value;
            _asDoubleFunc = () => value;
            _asInt64Func = () => value;
            _asUInt64Func = () => value;
            _asFloatFunc = () => value;
        }
        public NumericValue(sbyte value)
        {
            _asDecimalFunc = () => value;
            _asDoubleFunc = () => value;
            _asInt64Func = () => value;
            _asUInt64Func = () => (ulong)Math.Max(0, (int)value);
            _asFloatFunc = () => value;
        }
        public NumericValue(float value)
        {
            _asDecimalFunc = () => (decimal)value;
            _asDoubleFunc = () => value;
            _asInt64Func = () => (long)value;
            _asUInt64Func = () => (ulong)Math.Max(0, value);
            _asFloatFunc = () => value;
        }

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
