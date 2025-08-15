using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a numeric value is a prime number (for integer values only).
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Prime]
    ///     public int SpecialNumber { get; set; } // Valid: 2, 3, 5, 7, 11, 13, Invalid: 1, 4, 6, 8, 9, 10
    ///     
    ///     [Prime]
    ///     public int KeyValue { get; set; } // Valid: 17, 19, 23, Invalid: 15, 18, 20, 21
    /// }
    /// </code>
    /// </example>
    public class PrimeAttribute : NumericValidationAttributeBase,
        IValidationAttribute<sbyte>,
        IValidationAttribute<short>,
        IValidationAttribute<int>,
        IValidationAttribute<long>,
        IValidationAttribute<byte>,
        IValidationAttribute<ushort>,
        IValidationAttribute<uint>,
        IValidationAttribute<ulong>
    {

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "PrimeValidationError";


        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must be a prime number.";

        // Integer validation
        /// <inheritdoc/>
        public static bool IsValid(long value) => value >= 2 && IsPrime(value);
        /// <inheritdoc/>
        public static bool IsValid(int value) => value >= 2 && IsPrime(value);
        /// <inheritdoc/>
        public static bool IsValid(short value) => value >= 2 && IsPrime(value);
        /// <inheritdoc/>
        public static bool IsValid(sbyte value) => value >= 2 && IsPrime(value);
        /// <inheritdoc/>
        public static bool IsValid(ulong value) => value >= 2 && IsPrime((long)value);
        /// <inheritdoc/>
        public static bool IsValid(uint value) => value >= 2 && IsPrime(value);
        /// <inheritdoc/>
        public static bool IsValid(ushort value) => value >= 2 && IsPrime(value);
        /// <inheritdoc/>
        public static bool IsValid(byte value) => value >= 2 && IsPrime(value);

        // IValidationAttribute implementations
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
        public AttributeResult Validate(string propertyName, int value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, long value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, byte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, ushort value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, uint value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, ulong value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }

        /// <summary>
        /// Checks if a long integer is prime.
        /// </summary>
        private static bool IsPrime(long n)
        {
            if (n < 2) return false;
            if (n == 2) return true;
            if (n % 2 == 0) return false;
            var limit = (long)Math.Sqrt(n);
            for (long i = 3; i <= limit; i += 2)
            {
                if (n % i == 0) return false;
            }
            return true;
        }
        private static bool IsPrime(int n) => IsPrime((long)n);
        private static bool IsPrime(short n) => IsPrime((long)n);
        private static bool IsPrime(sbyte n) => IsPrime((long)n);
        private static bool IsPrime(uint n) => IsPrime((long)n);
        private static bool IsPrime(ushort n) => IsPrime((long)n);
        private static bool IsPrime(byte n) => IsPrime((long)n);
    }
}
