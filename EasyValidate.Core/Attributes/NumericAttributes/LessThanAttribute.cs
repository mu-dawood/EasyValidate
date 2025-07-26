using System;
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
        private readonly NumericValue _comparisonValue;

        public LessThanAttribute(int value) { _comparisonValue = value; }
        public LessThanAttribute(long value) { _comparisonValue = value; }
        public LessThanAttribute(double value) { _comparisonValue = value; }
        public LessThanAttribute(decimal value) { _comparisonValue = value; }
        public LessThanAttribute(float value) { _comparisonValue = value; }
        public LessThanAttribute(short value) { _comparisonValue = value; }
        public LessThanAttribute(ushort value) { _comparisonValue = value; }
        public LessThanAttribute(uint value) { _comparisonValue = value; }
        public LessThanAttribute(ulong value) { _comparisonValue = value; }
        public LessThanAttribute(byte value) { _comparisonValue = value; }
        public LessThanAttribute(sbyte value) { _comparisonValue = value; }

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
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, byte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsUInt64);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, sbyte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsInt64);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, short value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsInt64);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, ushort value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsUInt64);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, int value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsInt64);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, uint value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsUInt64);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, long value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsInt64);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, ulong value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsUInt64);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, float value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsFloat);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, double value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsDouble);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, decimal value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsDecimal);
        }
    }
}