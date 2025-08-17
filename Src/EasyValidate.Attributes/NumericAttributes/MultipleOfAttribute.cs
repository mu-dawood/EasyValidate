using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a numeric value is a multiple of a specified number.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [MultipleOf(5)]
    ///     public int Quantity { get; set; } // Valid: 0, 5, 10, 15, 20, Invalid: 1, 3, 7, 12
    ///     
    ///     [MultipleOf(2.5)]
    ///     public double Measurement { get; set; } // Valid: 0.0, 2.5, 5.0, 7.5, Invalid: 1.0, 3.0, 6.0
    /// }
    /// </code>
    /// </example>
    public class MultipleOfAttribute : NumericValidationAttributeBase,
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
        private readonly NumericValue _factor;

        /// <inheritdoc/>
        public MultipleOfAttribute(int value) { _factor = value; }
        /// <inheritdoc/>
        public MultipleOfAttribute(long value) { _factor = value; }
        /// <inheritdoc/>
        public MultipleOfAttribute(double value) { _factor = value; }
        /// <inheritdoc/>
        public MultipleOfAttribute(decimal value) { _factor = value; }
        /// <inheritdoc/>
        public MultipleOfAttribute(float value) { _factor = value; }
        /// <inheritdoc/>
        public MultipleOfAttribute(short value) { _factor = value; }
        /// <inheritdoc/>
        public MultipleOfAttribute(ushort value) { _factor = value; }
        /// <inheritdoc/>
        public MultipleOfAttribute(uint value) { _factor = value; }
        /// <inheritdoc/>
        public MultipleOfAttribute(ulong value) { _factor = value; }
        /// <inheritdoc/>
        public MultipleOfAttribute(byte value) { _factor = value; }
        /// <inheritdoc/>
        public MultipleOfAttribute(sbyte value) { _factor = value; }


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "MultipleOfValidationError";


        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must be a multiple of {1}.";

        // Integer validation
        /// <inheritdoc/>
        public bool IsValid(long value) => _factor.AsInt64 != 0 && value % _factor.AsInt64 == 0;
        /// <inheritdoc/>
        public bool IsValid(ulong value) => _factor.AsUInt64 != 0 && value % _factor.AsUInt64 == 0;
        /// <inheritdoc/>
        public bool IsValid(int value) => _factor.AsInt64 != 0 && value % _factor.AsInt64 == 0;
        /// <inheritdoc/>
        public bool IsValid(uint value) => _factor.AsUInt64 != 0 && value % _factor.AsUInt64 == 0;
        /// <inheritdoc/>
        public bool IsValid(short value) => _factor.AsInt64 != 0 && value % _factor.AsInt64 == 0;
        /// <inheritdoc/>
        public bool IsValid(ushort value) => _factor.AsUInt64 != 0 && value % _factor.AsUInt64 == 0;
        /// <inheritdoc/>
        public bool IsValid(byte value) => _factor.AsUInt64 != 0 && value % _factor.AsUInt64 == 0;
        /// <inheritdoc/>
        public bool IsValid(sbyte value) => _factor.AsInt64 != 0 && value % _factor.AsInt64 == 0;

        // Floating point/decimal validation
        /// <inheritdoc/>
        public bool IsValid(double value) => Math.Abs(_factor.AsDouble) > 1e-10 && Math.Abs(value % _factor.AsDouble) < 1e-10;
        /// <inheritdoc/>
        public bool IsValid(float value) => Math.Abs(_factor.AsFloat) > 1e-7f && Math.Abs(value % _factor.AsFloat) < 1e-7f;
        /// <inheritdoc/>
        public bool IsValid(decimal value) => _factor.AsDecimal != 0 && value % _factor.AsDecimal == 0;

        // IValidationAttribute implementations
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, byte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _factor.AsUInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, sbyte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _factor.AsInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, short value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _factor.AsInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, ushort value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _factor.AsUInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, int value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _factor.AsInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, uint value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _factor.AsUInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, long value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _factor.AsInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, ulong value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _factor.AsUInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, float value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _factor.AsFloat);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, double value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _factor.AsDouble);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, decimal value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _factor.AsDecimal);
        }
    }
}
