using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that an integer value has at most a specified number of digits.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [MaxDigits(3)]
    ///     public int Percentage { get; set; } // Valid: 99, 100, Invalid: 1000
    /// }
    /// </code>
    /// </example>
    public class MaxDigitsAttribute : NumericValidationAttributeBase,
        IValidationAttribute<byte>,
        IValidationAttribute<sbyte>,
        IValidationAttribute<short>,
        IValidationAttribute<ushort>,
        IValidationAttribute<int>,
        IValidationAttribute<uint>,
        IValidationAttribute<long>,
        IValidationAttribute<ulong>
    {
        private readonly int _maxDigits;
        public MaxDigitsAttribute(int maxDigits) { _maxDigits = maxDigits; }

        public override string ErrorCode { get; set; } = "MaxDigitsValidationError";
        public string ErrorMessage { get; set; } = "The {0} field must not exceed {1} digits.";

        // Integer validation
        public bool IsValid(long value) => CountDigits(value) <= _maxDigits;
        public bool IsValid(ulong value) => CountDigits(value) <= _maxDigits;
        public bool IsValid(int value) => CountDigits(value) <= _maxDigits;
        public bool IsValid(uint value) => CountDigits(value) <= _maxDigits;
        public bool IsValid(short value) => CountDigits(value) <= _maxDigits;
        public bool IsValid(ushort value) => CountDigits(value) <= _maxDigits;
        public bool IsValid(byte value) => CountDigits(value) <= _maxDigits;
        public bool IsValid(sbyte value) => CountDigits(value) <= _maxDigits;

        // IValidationAttribute implementations
        public AttributeResult Validate(string propertyName, byte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _maxDigits);
        }
        public AttributeResult Validate(string propertyName, sbyte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _maxDigits);
        }
        public AttributeResult Validate(string propertyName, short value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _maxDigits);
        }
        public AttributeResult Validate(string propertyName, ushort value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _maxDigits);
        }
        public AttributeResult Validate(string propertyName, int value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _maxDigits);
        }
        public AttributeResult Validate(string propertyName, uint value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _maxDigits);
        }
        public AttributeResult Validate(string propertyName, long value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _maxDigits);
        }
        public AttributeResult Validate(string propertyName, ulong value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _maxDigits);
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
