using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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
        IValidationAttribute<sbyte>,
        IValidationAttribute<short>,
        IValidationAttribute<int>,
        IValidationAttribute<long>,
        IValidationAttribute<byte>,
        IValidationAttribute<ushort>,
        IValidationAttribute<uint>,
        IValidationAttribute<ulong>
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
        /// <inheritdoc/>
        public bool IsValid(long value) => value >= 1 && IsPowerOfBase(value, _baseValue);
        /// <inheritdoc/>
        public bool IsValid(int value) => value >= 1 && IsPowerOfBase(value, _baseValue);
        /// <inheritdoc/>
        public bool IsValid(short value) => value >= 1 && IsPowerOfBase(value, _baseValue);
        /// <inheritdoc/>
        public bool IsValid(sbyte value) => value >= 1 && IsPowerOfBase(value, _baseValue);
        /// <inheritdoc/>
        public bool IsValid(ulong value) => value >= 1 && IsPowerOfBase(value, _baseValue);
        /// <inheritdoc/>
        public bool IsValid(uint value) => value >= 1 && IsPowerOfBase(value, _baseValue);
        /// <inheritdoc/>
        public bool IsValid(ushort value) => value >= 1 && IsPowerOfBase(value, _baseValue);
        /// <inheritdoc/>
        public bool IsValid(byte value) => value >= 1 && IsPowerOfBase(value, _baseValue);

        // IValidationAttribute implementations
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, sbyte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _baseValue);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, short value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _baseValue);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, int value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _baseValue);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, long value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _baseValue);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, byte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _baseValue);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, ushort value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _baseValue);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, uint value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _baseValue);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, ulong value)
        {
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
