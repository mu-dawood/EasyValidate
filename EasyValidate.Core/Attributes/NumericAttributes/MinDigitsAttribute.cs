using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that an integer value has at least a specified number of digits.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [MinDigits(4)]
    ///     public int Pin { get; set; } // Valid: 1234, 12345, Invalid: 123
    /// }
    /// </code>
    /// </example>
    public class MinDigitsAttribute : NumericValidationAttributeBase,
        IValidationAttribute<byte, byte>,
        IValidationAttribute<sbyte, sbyte>,
        IValidationAttribute<short, short>,
        IValidationAttribute<ushort, ushort>,
        IValidationAttribute<int, int>,
        IValidationAttribute<uint, uint>,
        IValidationAttribute<long, long>,
        IValidationAttribute<ulong, ulong>
    {
        private readonly int _minDigits;

        /// <summary>
        /// Initializes a new instance of the <see cref="MinDigitsAttribute"/> class.
        /// </summary>
        /// <param name="minDigits">The minimum number of digits required.</param>
        public MinDigitsAttribute(int minDigits)
        {
            _minDigits = minDigits;
        }

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "MinDigitsValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must have at least {1} digits.";

        // Integer validation
        public bool IsValid(long value) => CountDigits(value) >= _minDigits;
        public bool IsValid(ulong value) => CountDigits(value) >= _minDigits;
        public bool IsValid(int value) => CountDigits(value) >= _minDigits;
        public bool IsValid(uint value) => CountDigits(value) >= _minDigits;
        public bool IsValid(short value) => CountDigits(value) >= _minDigits;
        public bool IsValid(ushort value) => CountDigits(value) >= _minDigits;
        public bool IsValid(byte value) => CountDigits(value) >= _minDigits;
        public bool IsValid(sbyte value) => CountDigits(value) >= _minDigits;

        // IValidationAttribute implementations
        public AttributeResult<byte> Validate(object obj, string propertyName, byte value) =>
            new(IsValid(value), value, propertyName, _minDigits);
        public AttributeResult<sbyte> Validate(object obj, string propertyName, sbyte value) =>
            new(IsValid(value), value, propertyName, _minDigits);
        public AttributeResult<short> Validate(object obj, string propertyName, short value) =>
            new(IsValid(value), value, propertyName, _minDigits);
        public AttributeResult<ushort> Validate(object obj, string propertyName, ushort value) =>
            new(IsValid(value), value, propertyName, _minDigits);
        public AttributeResult<int> Validate(object obj, string propertyName, int value) =>
            new(IsValid(value), value, propertyName, _minDigits);
        public AttributeResult<uint> Validate(object obj, string propertyName, uint value) =>
            new(IsValid(value), value, propertyName, _minDigits);
        public AttributeResult<long> Validate(object obj, string propertyName, long value) =>
            new(IsValid(value), value, propertyName, _minDigits);
        public AttributeResult<ulong> Validate(object obj, string propertyName, ulong value) =>
            new(IsValid(value), value, propertyName, _minDigits);

        /// <summary>
        /// Counts the number of digits in an integer value (ignores sign).
        /// </summary>
        private static int CountDigits(long value)
        {
            value = Math.Abs(value);
            if (value < 10) return 1;
            int digits = 0;
            while (value != 0) { digits++; value /= 10; }
            return digits;
        }
        private static int CountDigits(ulong value)
        {
            if (value < 10) return 1;
            int digits = 0;
            while (value != 0) { digits++; value /= 10; }
            return digits;
        }
    }
}
