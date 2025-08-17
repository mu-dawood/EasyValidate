using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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
        IValidationAttribute<byte>,
        IValidationAttribute<sbyte>,
        IValidationAttribute<short>,
        IValidationAttribute<ushort>,
        IValidationAttribute<int>,
        IValidationAttribute<uint>,
        IValidationAttribute<long>,
        IValidationAttribute<ulong>
    {

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "FibonacciValidationError";


        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must be a Fibonacci number.";

        // Integer validation
        /// <inheritdoc/>
        public static bool IsValid(long value) => value >= 0 && IsFibonacciLong(value);
        /// <inheritdoc/>
        public static bool IsValid(ulong value) => IsFibonacciULong(value);
        /// <inheritdoc/>
        public static bool IsValid(int value) => value >= 0 && IsFibonacciLong(value);
        /// <inheritdoc/>
        public static bool IsValid(uint value) => IsFibonacciULong(value);
        /// <inheritdoc/>
        public static bool IsValid(short value) => value >= 0 && IsFibonacciLong(value);
        /// <inheritdoc/>
        public static bool IsValid(ushort value) => IsFibonacciULong(value);
        /// <inheritdoc/>
        public static bool IsValid(byte value) => IsFibonacciULong(value);
        /// <inheritdoc/>
        public static bool IsValid(sbyte value) => value >= 0 && IsFibonacciLong(value);

        // IValidationAttribute implementations
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, byte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, sbyte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, short value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, ushort value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, int value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, uint value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, long value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, ulong value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }

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
