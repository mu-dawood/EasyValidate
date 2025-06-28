using EasyValidate.Core.Abstraction;
using System;

namespace EasyValidate.Core.Attributes
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
        private readonly NumericValue _divisor;

        public DivisibleByAttribute(int divisor) { _divisor = divisor; }
        public DivisibleByAttribute(long divisor) { _divisor = divisor; }
        public DivisibleByAttribute(double divisor) { _divisor = divisor; }
        public DivisibleByAttribute(decimal divisor) { _divisor = divisor; }
        public DivisibleByAttribute(float divisor) { _divisor = divisor; }
        public DivisibleByAttribute(short divisor) { _divisor = divisor; }
        public DivisibleByAttribute(ushort divisor) { _divisor = divisor; }
        public DivisibleByAttribute(uint divisor) { _divisor = divisor; }
        public DivisibleByAttribute(ulong divisor) { _divisor = divisor; }
        public DivisibleByAttribute(byte divisor) { _divisor = divisor; }
        public DivisibleByAttribute(sbyte divisor) { _divisor = divisor; }

        public override string ErrorCode { get; set; } = "DivisibleByValidationError";
        public override string ErrorMessage { get; set; } = "The {0} field must be divisible by {1}.";

        // Integer validation
        public bool IsValid(long value) => _divisor.AsInt64 != 0 && value % _divisor.AsInt64 == 0;
        public bool IsValid(ulong value) => _divisor.AsUInt64 != 0 && value % _divisor.AsUInt64 == 0;
        public bool IsValid(int value) => _divisor.AsInt64 != 0 && value % _divisor.AsInt64 == 0;
        public bool IsValid(uint value) => _divisor.AsUInt64 != 0 && value % _divisor.AsUInt64 == 0;
        public bool IsValid(short value) => _divisor.AsInt64 != 0 && value % _divisor.AsInt64 == 0;
        public bool IsValid(ushort value) => _divisor.AsUInt64 != 0 && value % _divisor.AsUInt64 == 0;
        public bool IsValid(byte value) => _divisor.AsUInt64 != 0 && value % _divisor.AsUInt64 == 0;
        public bool IsValid(sbyte value) => _divisor.AsInt64 != 0 && value % _divisor.AsInt64 == 0;

        // Floating point validation (with tolerance)
        public bool IsValid(double value) => _divisor.AsDouble != 0 && Math.Abs(value % _divisor.AsDouble) < 1e-10;
        public bool IsValid(float value) => _divisor.AsFloat != 0 && Math.Abs(value % _divisor.AsFloat) < 1e-6f;
        public bool IsValid(decimal value) => _divisor.AsDecimal != 0 && value % _divisor.AsDecimal == 0;

        // IValidationAttribute implementations
        public AttributeResult<byte> Validate(object obj, string propertyName, byte value) =>
            new(IsValid(value), value, propertyName, _divisor.AsUInt64);
        public AttributeResult<sbyte> Validate(object obj, string propertyName, sbyte value) =>
            new(IsValid(value), value, propertyName, _divisor.AsInt64);
        public AttributeResult<short> Validate(object obj, string propertyName, short value) =>
            new(IsValid(value), value, propertyName, _divisor.AsInt64);
        public AttributeResult<ushort> Validate(object obj, string propertyName, ushort value) =>
            new(IsValid(value), value, propertyName, _divisor.AsUInt64);
        public AttributeResult<int> Validate(object obj, string propertyName, int value) =>
            new(IsValid(value), value, propertyName, _divisor.AsInt64);
        public AttributeResult<uint> Validate(object obj, string propertyName, uint value) =>
            new(IsValid(value), value, propertyName, _divisor.AsUInt64);
        public AttributeResult<long> Validate(object obj, string propertyName, long value) =>
            new(IsValid(value), value, propertyName, _divisor.AsInt64);
        public AttributeResult<ulong> Validate(object obj, string propertyName, ulong value) =>
            new(IsValid(value), value, propertyName, _divisor.AsUInt64);
        public AttributeResult<float> Validate(object obj, string propertyName, float value) =>
            new(IsValid(value), value, propertyName, _divisor.AsFloat);
        public AttributeResult<double> Validate(object obj, string propertyName, double value) =>
            new(IsValid(value), value, propertyName, _divisor.AsDouble);
        public AttributeResult<decimal> Validate(object obj, string propertyName, decimal value) =>
            new(IsValid(value), value, propertyName, _divisor.AsDecimal);
    }
}
