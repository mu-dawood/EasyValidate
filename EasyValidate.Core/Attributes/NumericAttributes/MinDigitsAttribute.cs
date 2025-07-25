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
        public bool IsValid(long value) => CountDigits(value) >= _minDigits;
        public bool IsValid(ulong value) => CountDigits(value) >= _minDigits;
        public bool IsValid(int value) => CountDigits(value) >= _minDigits;
        public bool IsValid(uint value) => CountDigits(value) >= _minDigits;
        public bool IsValid(short value) => CountDigits(value) >= _minDigits;
        public bool IsValid(ushort value) => CountDigits(value) >= _minDigits;
        public bool IsValid(byte value) => CountDigits(value) >= _minDigits;
        public bool IsValid(sbyte value) => CountDigits(value) >= _minDigits;

        // IValidationAttribute implementations
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, byte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, sbyte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, short value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, ushort value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, int value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, uint value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, long value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, ulong value)
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
