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
        IValidationAttribute<byte, byte>,
        IValidationAttribute<sbyte, sbyte>,
        IValidationAttribute<short, short>,
        IValidationAttribute<ushort, ushort>,
        IValidationAttribute<int, int>,
        IValidationAttribute<uint, uint>,
        IValidationAttribute<long, long>,
        IValidationAttribute<ulong, ulong>
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
        public AttributeResult Validate(object obj, string propertyName, byte value, out byte output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _maxDigits);
        }
        public AttributeResult Validate(object obj, string propertyName, sbyte value, out sbyte output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _maxDigits);
        }
        public AttributeResult Validate(object obj, string propertyName, short value, out short output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _maxDigits);
        }
        public AttributeResult Validate(object obj, string propertyName, ushort value, out ushort output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _maxDigits);
        }
        public AttributeResult Validate(object obj, string propertyName, int value, out int output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _maxDigits);
        }
        public AttributeResult Validate(object obj, string propertyName, uint value, out uint output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _maxDigits);
        }
        public AttributeResult Validate(object obj, string propertyName, long value, out long output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _maxDigits);
        }
        public AttributeResult Validate(object obj, string propertyName, ulong value, out ulong output)
        {
            output = value;
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
