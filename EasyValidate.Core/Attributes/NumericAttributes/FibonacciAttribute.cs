using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a numeric value is a Fibonacci number.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Fibonacci]
    ///     public int SpecialNumber { get; set; } // Valid: 0, 1, 1, 2, 3, 5, 8, 13, 21, Invalid: 4, 6, 7, 9, 10
    ///     
    ///     [Fibonacci]
    ///     public long Sequence { get; set; } // Valid: 34, 55, 89, 144, Invalid: 35, 56, 90
    /// }
    /// </code>
    /// </example>
    public class FibonacciAttribute : NumericValidationAttributeBase,
        IValidationAttribute<byte, byte>,
        IValidationAttribute<sbyte, sbyte>,
        IValidationAttribute<short, short>,
        IValidationAttribute<ushort, ushort>,
        IValidationAttribute<int, int>,
        IValidationAttribute<uint, uint>,
        IValidationAttribute<long, long>,
        IValidationAttribute<ulong, ulong>
    {
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "FibonacciValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be a Fibonacci number.";

        // Integer validation
        public static bool IsValid(long value) => value >= 0 && IsFibonacciLong(value);
        public static bool IsValid(ulong value) => IsFibonacciULong(value);
        public static bool IsValid(int value) => value >= 0 && IsFibonacciLong(value);
        public static bool IsValid(uint value) => IsFibonacciULong(value);
        public static bool IsValid(short value) => value >= 0 && IsFibonacciLong(value);
        public static bool IsValid(ushort value) => IsFibonacciULong(value);
        public static bool IsValid(byte value) => IsFibonacciULong(value);
        public static bool IsValid(sbyte value) => value >= 0 && IsFibonacciLong(value);

        // IValidationAttribute implementations
        public AttributeResult<byte> Validate(object obj, string propertyName, byte value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<sbyte> Validate(object obj, string propertyName, sbyte value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<short> Validate(object obj, string propertyName, short value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<ushort> Validate(object obj, string propertyName, ushort value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<int> Validate(object obj, string propertyName, int value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<uint> Validate(object obj, string propertyName, uint value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<long> Validate(object obj, string propertyName, long value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<ulong> Validate(object obj, string propertyName, ulong value) =>
            new(IsValid(value), value, propertyName);

        // Fibonacci helpers
        private static bool IsFibonacciLong(long n)
        {
            if (n < 0) return false;
            long x1 = 5 * n * n + 4;
            long x2 = 5 * n * n - 4;
            return IsPerfectSquareLong(x1) || IsPerfectSquareLong(x2);
        }
        private static bool IsFibonacciULong(ulong n)
        {
            ulong x1 = 5 * n * n + 4;
            ulong x2 = 5 * n * n - 4;
            return IsPerfectSquareULong(x1) || IsPerfectSquareULong(x2);
        }
        private static bool IsPerfectSquareLong(long x)
        {
            long s = (long)Math.Sqrt(x);
            return s * s == x;
        }
        private static bool IsPerfectSquareULong(ulong x)
        {
            ulong s = (ulong)Math.Sqrt(x);
            return s * s == x;
        }
    }
}
