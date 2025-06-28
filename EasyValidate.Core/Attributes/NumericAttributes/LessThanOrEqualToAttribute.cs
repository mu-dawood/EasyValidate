using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a numeric value is less than or equal to a specified comparison value.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [LessThanOrEqualTo(100)]
    ///     public int Percentage { get; set; } // Valid: 50, 100, Invalid: 101
    ///     
    ///     [LessThanOrEqualTo(999.99)]
    ///     public double MaxPrice { get; set; } // Valid: 500.00, 999.99, Invalid: 1000.00
    /// }
    /// </code>
    /// </example>
    public class LessThanOrEqualToAttribute : NumericValidationAttributeBase,
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
        private readonly NumericValue _comparisonValue;

        public LessThanOrEqualToAttribute(int value)    { _comparisonValue = value; }
        public LessThanOrEqualToAttribute(long value)   { _comparisonValue = value; }
        public LessThanOrEqualToAttribute(double value) { _comparisonValue = value; }
        public LessThanOrEqualToAttribute(decimal value){ _comparisonValue = value; }
        public LessThanOrEqualToAttribute(float value)  { _comparisonValue = value; }
        public LessThanOrEqualToAttribute(short value)  { _comparisonValue = value; }
        public LessThanOrEqualToAttribute(ushort value) { _comparisonValue = value; }
        public LessThanOrEqualToAttribute(uint value)   { _comparisonValue = value; }
        public LessThanOrEqualToAttribute(ulong value)  { _comparisonValue = value; }
        public LessThanOrEqualToAttribute(byte value)   { _comparisonValue = value; }
        public LessThanOrEqualToAttribute(sbyte value)  { _comparisonValue = value; }

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "LessThanOrEqualToValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be less than or equal to {1}.";

        // Integer validation
        public bool IsValid(long value) => value <= _comparisonValue.AsInt64;
        public bool IsValid(ulong value) => value <= _comparisonValue.AsUInt64;
        public bool IsValid(int value) => value <= _comparisonValue.AsInt64;
        public bool IsValid(uint value) => value <= _comparisonValue.AsUInt64;
        public bool IsValid(short value) => value <= _comparisonValue.AsInt64;
        public bool IsValid(ushort value) => value <= _comparisonValue.AsUInt64;
        public bool IsValid(byte value) => value <= _comparisonValue.AsUInt64;
        public bool IsValid(sbyte value) => value <= _comparisonValue.AsInt64;

        // Floating point validation
        public bool IsValid(double value) => value <= _comparisonValue.AsDouble;
        public bool IsValid(float value) => value <= _comparisonValue.AsFloat;
        public bool IsValid(decimal value) => value <= _comparisonValue.AsDecimal;

        // IValidationAttribute implementations
        public AttributeResult<byte> Validate(object obj, string propertyName, byte value) =>
            new(IsValid(value), value, propertyName, _comparisonValue.AsUInt64);
        public AttributeResult<sbyte> Validate(object obj, string propertyName, sbyte value) =>
            new(IsValid(value), value, propertyName, _comparisonValue.AsInt64);
        public AttributeResult<short> Validate(object obj, string propertyName, short value) =>
            new(IsValid(value), value, propertyName, _comparisonValue.AsInt64);
        public AttributeResult<ushort> Validate(object obj, string propertyName, ushort value) =>
            new(IsValid(value), value, propertyName, _comparisonValue.AsUInt64);
        public AttributeResult<int> Validate(object obj, string propertyName, int value) =>
            new(IsValid(value), value, propertyName, _comparisonValue.AsInt64);
        public AttributeResult<uint> Validate(object obj, string propertyName, uint value) =>
            new(IsValid(value), value, propertyName, _comparisonValue.AsUInt64);
        public AttributeResult<long> Validate(object obj, string propertyName, long value) =>
            new(IsValid(value), value, propertyName, _comparisonValue.AsInt64);
        public AttributeResult<ulong> Validate(object obj, string propertyName, ulong value) =>
            new(IsValid(value), value, propertyName, _comparisonValue.AsUInt64);
        public AttributeResult<float> Validate(object obj, string propertyName, float value) =>
            new(IsValid(value), value, propertyName, _comparisonValue.AsFloat);
        public AttributeResult<double> Validate(object obj, string propertyName, double value) =>
            new(IsValid(value), value, propertyName, _comparisonValue.AsDouble);
        public AttributeResult<decimal> Validate(object obj, string propertyName, decimal value) =>
            new(IsValid(value), value, propertyName, _comparisonValue.AsDecimal);
    }
}