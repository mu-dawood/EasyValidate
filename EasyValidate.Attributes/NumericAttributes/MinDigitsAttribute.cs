using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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
        IValidationAttribute<byte>,
        IValidationAttribute<sbyte>,
        IValidationAttribute<short>,
        IValidationAttribute<ushort>,
        IValidationAttribute<int>,
        IValidationAttribute<uint>,
        IValidationAttribute<long>,
        IValidationAttribute<ulong>
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
        public string ErrorMessage { get; set; } = "The {0} field must have at least {1} digits.";

        // Integer validation
        /// <inheritdoc/>
        public bool IsValid(long value) => CountDigits(value) >= _minDigits;
        /// <inheritdoc/>
        public bool IsValid(ulong value) => CountDigits(value) >= _minDigits;
        /// <inheritdoc/>
        public bool IsValid(int value) => CountDigits(value) >= _minDigits;
        /// <inheritdoc/>
        public bool IsValid(uint value) => CountDigits(value) >= _minDigits;
        /// <inheritdoc/>
        public bool IsValid(short value) => CountDigits(value) >= _minDigits;
        /// <inheritdoc/>
        public bool IsValid(ushort value) => CountDigits(value) >= _minDigits;
        /// <inheritdoc/>
        public bool IsValid(byte value) => CountDigits(value) >= _minDigits;
        /// <inheritdoc/>
        public bool IsValid(sbyte value) => CountDigits(value) >= _minDigits;

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
