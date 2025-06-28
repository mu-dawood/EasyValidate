using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a numeric value is greater than or equal to a specified comparison value.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [GreaterThanOrEqualTo(0)]
    ///     public int Quantity { get; set; } // Valid: 0, 10, 100, Invalid: -1
    ///     
    ///     [GreaterThanOrEqualTo(18.5)]
    ///     public double MinimumAge { get; set; } // Valid: 18.5, 25.0, Invalid: 17.9
    /// }
    /// </code>
    /// </example>
    public class GreaterThanOrEqualToAttribute : NumericValidationAttributeBase,
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

        public GreaterThanOrEqualToAttribute(int value)    { _comparisonValue = value; }
        public GreaterThanOrEqualToAttribute(long value)   { _comparisonValue = value; }
        public GreaterThanOrEqualToAttribute(double value) { _comparisonValue = value; }
        public GreaterThanOrEqualToAttribute(decimal value){ _comparisonValue = value; }
        public GreaterThanOrEqualToAttribute(float value)  { _comparisonValue = value; }
        public GreaterThanOrEqualToAttribute(short value)  { _comparisonValue = value; }
        public GreaterThanOrEqualToAttribute(ushort value) { _comparisonValue = value; }
        public GreaterThanOrEqualToAttribute(uint value)   { _comparisonValue = value; }
        public GreaterThanOrEqualToAttribute(ulong value)  { _comparisonValue = value; }
        public GreaterThanOrEqualToAttribute(byte value)   { _comparisonValue = value; }
        public GreaterThanOrEqualToAttribute(sbyte value)  { _comparisonValue = value; }

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "GreaterThanOrEqualToValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be greater than or equal to {1}.";

        // Integer validation
        public bool IsValid(long value) => value >= _comparisonValue.AsInt64;
        public bool IsValid(ulong value) => value >= _comparisonValue.AsUInt64;
        public bool IsValid(int value) => value >= _comparisonValue.AsInt64;
        public bool IsValid(uint value) => value >= _comparisonValue.AsUInt64;
        public bool IsValid(short value) => value >= _comparisonValue.AsInt64;
        public bool IsValid(ushort value) => value >= _comparisonValue.AsUInt64;
        public bool IsValid(byte value) => value >= _comparisonValue.AsUInt64;
        public bool IsValid(sbyte value) => value >= _comparisonValue.AsInt64;

        // Floating point validation
        public bool IsValid(double value) => value >= _comparisonValue.AsDouble;
        public bool IsValid(float value) => value >= _comparisonValue.AsFloat;
        public bool IsValid(decimal value) => value >= _comparisonValue.AsDecimal;

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