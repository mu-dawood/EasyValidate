using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a numeric value is less than a specified comparison value.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [LessThan(100)]
    ///     public int Score { get; set; } // Valid: 0, 50, 99, Invalid: 100, 150
    ///     
    ///     [LessThan(18.5)]
    ///     public double Temperature { get; set; } // Valid: 15.0, 18.0, Invalid: 18.5, 20.0
    /// }
    /// </code>
    /// </example>
    public class LessThanAttribute : NumericValidationAttributeBase,
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

        public LessThanAttribute(int value)    { _comparisonValue = value; }
        public LessThanAttribute(long value)   { _comparisonValue = value; }
        public LessThanAttribute(double value) { _comparisonValue = value; }
        public LessThanAttribute(decimal value){ _comparisonValue = value; }
        public LessThanAttribute(float value)  { _comparisonValue = value; }
        public LessThanAttribute(short value)  { _comparisonValue = value; }
        public LessThanAttribute(ushort value) { _comparisonValue = value; }
        public LessThanAttribute(uint value)   { _comparisonValue = value; }
        public LessThanAttribute(ulong value)  { _comparisonValue = value; }
        public LessThanAttribute(byte value)   { _comparisonValue = value; }
        public LessThanAttribute(sbyte value)  { _comparisonValue = value; }

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "LessThanValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must be less than {1}.";

        // Integer validation
        public bool IsValid(long value) => value < _comparisonValue.AsInt64;
        public bool IsValid(ulong value) => value < _comparisonValue.AsUInt64;
        public bool IsValid(int value) => value < _comparisonValue.AsInt64;
        public bool IsValid(uint value) => value < _comparisonValue.AsUInt64;
        public bool IsValid(short value) => value < _comparisonValue.AsInt64;
        public bool IsValid(ushort value) => value < _comparisonValue.AsUInt64;
        public bool IsValid(byte value) => value < _comparisonValue.AsUInt64;
        public bool IsValid(sbyte value) => value < _comparisonValue.AsInt64;

        // Floating point validation
        public bool IsValid(double value) => value < _comparisonValue.AsDouble;
        public bool IsValid(float value) => value < _comparisonValue.AsFloat;
        public bool IsValid(decimal value) => value < _comparisonValue.AsDecimal;

        // IValidationAttribute implementations
        public AttributeResult Validate(object obj, string propertyName, byte value, out byte output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsUInt64);
        }
        public AttributeResult Validate(object obj, string propertyName, sbyte value, out sbyte output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsInt64);
        }
        public AttributeResult Validate(object obj, string propertyName, short value, out short output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsInt64);
        }
        public AttributeResult Validate(object obj, string propertyName, ushort value, out ushort output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsUInt64);
        }
        public AttributeResult Validate(object obj, string propertyName, int value, out int output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsInt64);
        }
        public AttributeResult Validate(object obj, string propertyName, uint value, out uint output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsUInt64);
        }
        public AttributeResult Validate(object obj, string propertyName, long value, out long output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsInt64);
        }
        public AttributeResult Validate(object obj, string propertyName, ulong value, out ulong output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsUInt64);
        }
        public AttributeResult Validate(object obj, string propertyName, float value, out float output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsFloat);
        }
        public AttributeResult Validate(object obj, string propertyName, double value, out double output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsDouble);
        }
        public AttributeResult Validate(object obj, string propertyName, decimal value, out decimal output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsDecimal);
        }
    }
}