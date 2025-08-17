using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a numeric value is divisible by a specified number.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [DivisibleBy(5)]
    ///     public int Quantity { get; set; } // Valid: 5, 10, 15, 20, Invalid: 3, 7, 12
    /// }
    /// </code>
    /// </example>
    public sealed class DivisibleByAttribute : NumericValidationAttributeBase,
        IValidationAttribute<byte>,
        IValidationAttribute<sbyte>,
        IValidationAttribute<short>,
        IValidationAttribute<ushort>,
        IValidationAttribute<int>,
        IValidationAttribute<uint>,
        IValidationAttribute<long>,
        IValidationAttribute<ulong>,
        IValidationAttribute<float>,
        IValidationAttribute<double>,
        IValidationAttribute<decimal>
    {
        private readonly NumericValue _divisor;

        /// <inheritdoc/>
        public DivisibleByAttribute(int divisor) { _divisor = divisor; }
        /// <inheritdoc/>
        public DivisibleByAttribute(long divisor) { _divisor = divisor; }
        /// <inheritdoc/>
        public DivisibleByAttribute(double divisor) { _divisor = divisor; }
        /// <inheritdoc/>
        public DivisibleByAttribute(decimal divisor) { _divisor = divisor; }
        /// <inheritdoc/>
        public DivisibleByAttribute(float divisor) { _divisor = divisor; }
        /// <inheritdoc/>
        public DivisibleByAttribute(short divisor) { _divisor = divisor; }
        /// <inheritdoc/>
        public DivisibleByAttribute(ushort divisor) { _divisor = divisor; }
        /// <inheritdoc/>
        public DivisibleByAttribute(uint divisor) { _divisor = divisor; }
        /// <inheritdoc/>
        public DivisibleByAttribute(ulong divisor) { _divisor = divisor; }
        /// <inheritdoc/>
        public DivisibleByAttribute(byte divisor) { _divisor = divisor; }
        /// <inheritdoc/>
        public DivisibleByAttribute(sbyte divisor) { _divisor = divisor; }

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "DivisibleByValidationError";
        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must be divisible by {1}.";

        // Integer validation
        /// <inheritdoc/>
        public bool IsValid(long value) => _divisor.AsInt64 != 0 && value % _divisor.AsInt64 == 0;
        /// <inheritdoc/>
        public bool IsValid(ulong value) => _divisor.AsUInt64 != 0 && value % _divisor.AsUInt64 == 0;
        /// <inheritdoc/>
        public bool IsValid(int value) => _divisor.AsInt64 != 0 && value % _divisor.AsInt64 == 0;
        /// <inheritdoc/>
        public bool IsValid(uint value) => _divisor.AsUInt64 != 0 && value % _divisor.AsUInt64 == 0;
        /// <inheritdoc/>
        public bool IsValid(short value) => _divisor.AsInt64 != 0 && value % _divisor.AsInt64 == 0;
        /// <inheritdoc/>
        public bool IsValid(ushort value) => _divisor.AsUInt64 != 0 && value % _divisor.AsUInt64 == 0;
        /// <inheritdoc/>
        public bool IsValid(byte value) => _divisor.AsUInt64 != 0 && value % _divisor.AsUInt64 == 0;
        /// <inheritdoc/>
        public bool IsValid(sbyte value) => _divisor.AsInt64 != 0 && value % _divisor.AsInt64 == 0;

        // Floating point validation (with tolerance)
        /// <inheritdoc/>
        public bool IsValid(double value) => _divisor.AsDouble != 0 && Math.Abs(value % _divisor.AsDouble) < 1e-10;
        /// <inheritdoc/>
        public bool IsValid(float value) => _divisor.AsFloat != 0 && Math.Abs(value % _divisor.AsFloat) < 1e-6f;
        /// <inheritdoc/>
        public bool IsValid(decimal value) => _divisor.AsDecimal != 0 && value % _divisor.AsDecimal == 0;

        // IValidationAttribute implementations
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, byte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _divisor.AsUInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, sbyte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _divisor.AsInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, short value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _divisor.AsInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, ushort value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _divisor.AsUInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, int value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _divisor.AsInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, uint value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _divisor.AsUInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, long value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _divisor.AsInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, ulong value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _divisor.AsUInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, float value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _divisor.AsFloat);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, double value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _divisor.AsDouble);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, decimal value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _divisor.AsDecimal);
        }
    }
}
