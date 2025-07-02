using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a numeric value is a power of a specified base.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [PowerOf(2)]
    ///     public int BufferSize { get; set; } // Valid: 1, 2, 4, 8, 16, 32, 64, Invalid: 3, 5, 6, 7, 9
    ///     
    ///     [PowerOf(10)]
    ///     public long Multiplier { get; set; } // Valid: 1, 10, 100, 1000, Invalid: 2, 20, 200
    /// }
    /// </code>
    /// </example>
    public class PowerOfAttribute : NumericValidationAttributeBase,
        IValidationAttribute<sbyte, sbyte>,
        IValidationAttribute<short, short>,
        IValidationAttribute<int, int>,
        IValidationAttribute<long, long>,
        IValidationAttribute<byte, byte>,
        IValidationAttribute<ushort, ushort>,
        IValidationAttribute<uint, uint>,
        IValidationAttribute<ulong, ulong>
    {
        private readonly int _baseValue;

        /// <summary>
        /// Initializes a new instance of the <see cref="PowerOfAttribute"/> class.
        /// </summary>
        /// <param name="baseValue">The base value for the power check (must be > 1).</param>
        public PowerOfAttribute(int baseValue)
        {
            if (baseValue <= 1)
            {
                throw new ArgumentException("Base must be greater than 1.", nameof(baseValue));
            }
            _baseValue = baseValue;
        }

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "PowerOfValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must be a power of {1}.";

        // Integer validation
        public bool IsValid(long value) => value >= 1 && IsPowerOfBase(value, _baseValue);
        public bool IsValid(int value) => value >= 1 && IsPowerOfBase(value, _baseValue);
        public bool IsValid(short value) => value >= 1 && IsPowerOfBase(value, _baseValue);
        public bool IsValid(sbyte value) => value >= 1 && IsPowerOfBase(value, _baseValue);
        public bool IsValid(ulong value) => value >= 1 && IsPowerOfBase(value, _baseValue);
        public bool IsValid(uint value) => value >= 1 && IsPowerOfBase(value, _baseValue);
        public bool IsValid(ushort value) => value >= 1 && IsPowerOfBase(value, _baseValue);
        public bool IsValid(byte value) => value >= 1 && IsPowerOfBase(value, _baseValue);

        // IValidationAttribute implementations
        public AttributeResult Validate(object obj, string propertyName, sbyte value, out sbyte output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _baseValue);
        }
        public AttributeResult Validate(object obj, string propertyName, short value, out short output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _baseValue);
        }
        public AttributeResult Validate(object obj, string propertyName, int value, out int output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _baseValue);
        }
        public AttributeResult Validate(object obj, string propertyName, long value, out long output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _baseValue);
        }
        public AttributeResult Validate(object obj, string propertyName, byte value, out byte output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _baseValue);
        }
        public AttributeResult Validate(object obj, string propertyName, ushort value, out ushort output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _baseValue);
        }
        public AttributeResult Validate(object obj, string propertyName, uint value, out uint output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _baseValue);
        }
        public AttributeResult Validate(object obj, string propertyName, ulong value, out ulong output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _baseValue);
        }

        /// <summary>
        /// Checks if an integer value is a power of the given base.
        /// </summary>
        private static bool IsPowerOfBase(long value, int baseValue)
        {
            if (value < 1 || baseValue <= 1)
                return false;
            long current = 1;
            while (current < value)
            {
                current *= baseValue;
            }
            return current == value;
        }
        private static bool IsPowerOfBase(int value, int baseValue) => IsPowerOfBase((long)value, baseValue);
        private static bool IsPowerOfBase(short value, int baseValue) => IsPowerOfBase((long)value, baseValue);
        private static bool IsPowerOfBase(sbyte value, int baseValue) => IsPowerOfBase((long)value, baseValue);
        private static bool IsPowerOfBase(ulong value, int baseValue)
        {
            if (value < 1UL || baseValue <= 1)
                return false;
            ulong current = 1;
            while (current < value)
            {
                current *= (ulong)baseValue;
            }
            return current == value;
        }
        private static bool IsPowerOfBase(uint value, int baseValue) => IsPowerOfBase((ulong)value, baseValue);
        private static bool IsPowerOfBase(ushort value, int baseValue) => IsPowerOfBase((ulong)value, baseValue);
        private static bool IsPowerOfBase(byte value, int baseValue) => IsPowerOfBase((ulong)value, baseValue);
    }
}
