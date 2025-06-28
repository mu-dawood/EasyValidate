using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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
        IValidationAttribute<byte, byte>,
        IValidationAttribute<sbyte, sbyte>,
        IValidationAttribute<short, short>,
        IValidationAttribute<ushort, ushort>,
        IValidationAttribute<int, int>,
        IValidationAttribute<uint, uint>,
        IValidationAttribute<long, long>,
        IValidationAttribute<ulong, ulong>,
        IValidationAttribute<float, float>,
        IValidationAttribute<double, double>,
        IValidationAttribute<decimal, decimal>
    {
        private readonly NumericValue _factor;

        public MultipleOfAttribute(int value)    { _factor = value; }
        public MultipleOfAttribute(long value)   { _factor = value; }
        public MultipleOfAttribute(double value) { _factor = value; }
        public MultipleOfAttribute(decimal value){ _factor = value; }
        public MultipleOfAttribute(float value)  { _factor = value; }
        public MultipleOfAttribute(short value)  { _factor = value; }
        public MultipleOfAttribute(ushort value) { _factor = value; }
        public MultipleOfAttribute(uint value)   { _factor = value; }
        public MultipleOfAttribute(ulong value)  { _factor = value; }
        public MultipleOfAttribute(byte value)   { _factor = value; }
        public MultipleOfAttribute(sbyte value)  { _factor = value; }

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "MultipleOfValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be a multiple of {1}.";

        // Integer validation
        public bool IsValid(long value) => _factor.AsInt64 != 0 && value % _factor.AsInt64 == 0;
        public bool IsValid(ulong value) => _factor.AsUInt64 != 0 && value % _factor.AsUInt64 == 0;
        public bool IsValid(int value) => _factor.AsInt64 != 0 && value % _factor.AsInt64 == 0;
        public bool IsValid(uint value) => _factor.AsUInt64 != 0 && value % _factor.AsUInt64 == 0;
        public bool IsValid(short value) => _factor.AsInt64 != 0 && value % _factor.AsInt64 == 0;
        public bool IsValid(ushort value) => _factor.AsUInt64 != 0 && value % _factor.AsUInt64 == 0;
        public bool IsValid(byte value) => _factor.AsUInt64 != 0 && value % _factor.AsUInt64 == 0;
        public bool IsValid(sbyte value) => _factor.AsInt64 != 0 && value % _factor.AsInt64 == 0;

        // Floating point/decimal validation
        public bool IsValid(double value) => Math.Abs(_factor.AsDouble) > 1e-10 && Math.Abs(value % _factor.AsDouble) < 1e-10;
        public bool IsValid(float value) => Math.Abs(_factor.AsFloat) > 1e-7f && Math.Abs(value % _factor.AsFloat) < 1e-7f;
        public bool IsValid(decimal value) => _factor.AsDecimal != 0 && value % _factor.AsDecimal == 0;

        // IValidationAttribute implementations
        public AttributeResult<byte> Validate(object obj, string propertyName, byte value) =>
            new(IsValid(value), value, propertyName, _factor.AsUInt64);
        public AttributeResult<sbyte> Validate(object obj, string propertyName, sbyte value) =>
            new(IsValid(value), value, propertyName, _factor.AsInt64);
        public AttributeResult<short> Validate(object obj, string propertyName, short value) =>
            new(IsValid(value), value, propertyName, _factor.AsInt64);
        public AttributeResult<ushort> Validate(object obj, string propertyName, ushort value) =>
            new(IsValid(value), value, propertyName, _factor.AsUInt64);
        public AttributeResult<int> Validate(object obj, string propertyName, int value) =>
            new(IsValid(value), value, propertyName, _factor.AsInt64);
        public AttributeResult<uint> Validate(object obj, string propertyName, uint value) =>
            new(IsValid(value), value, propertyName, _factor.AsUInt64);
        public AttributeResult<long> Validate(object obj, string propertyName, long value) =>
            new(IsValid(value), value, propertyName, _factor.AsInt64);
        public AttributeResult<ulong> Validate(object obj, string propertyName, ulong value) =>
            new(IsValid(value), value, propertyName, _factor.AsUInt64);
        public AttributeResult<float> Validate(object obj, string propertyName, float value) =>
            new(IsValid(value), value, propertyName, _factor.AsFloat);
        public AttributeResult<double> Validate(object obj, string propertyName, double value) =>
            new(IsValid(value), value, propertyName, _factor.AsDouble);
        public AttributeResult<decimal> Validate(object obj, string propertyName, decimal value) =>
            new(IsValid(value), value, propertyName, _factor.AsDecimal);
    }
}
