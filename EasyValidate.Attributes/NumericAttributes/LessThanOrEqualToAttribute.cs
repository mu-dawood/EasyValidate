using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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

        /// <inheritdoc/>
        public LessThanOrEqualToAttribute(int value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public LessThanOrEqualToAttribute(long value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public LessThanOrEqualToAttribute(double value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public LessThanOrEqualToAttribute(decimal value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public LessThanOrEqualToAttribute(float value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public LessThanOrEqualToAttribute(short value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public LessThanOrEqualToAttribute(ushort value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public LessThanOrEqualToAttribute(uint value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public LessThanOrEqualToAttribute(ulong value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public LessThanOrEqualToAttribute(byte value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public LessThanOrEqualToAttribute(sbyte value) { _comparisonValue = value; }


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "LessThanOrEqualToValidationError";


        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must be less than or equal to {1}.";

        // Integer validation
        /// <inheritdoc/>
        public bool IsValid(long value) => value <= _comparisonValue.AsInt64;
        /// <inheritdoc/>
        public bool IsValid(ulong value) => value <= _comparisonValue.AsUInt64;
        /// <inheritdoc/>
        public bool IsValid(int value) => value <= _comparisonValue.AsInt64;
        /// <inheritdoc/>
        public bool IsValid(uint value) => value <= _comparisonValue.AsUInt64;
        /// <inheritdoc/>
        public bool IsValid(short value) => value <= _comparisonValue.AsInt64;
        /// <inheritdoc/>
        public bool IsValid(ushort value) => value <= _comparisonValue.AsUInt64;
        /// <inheritdoc/>
        public bool IsValid(byte value) => value <= _comparisonValue.AsUInt64;
        /// <inheritdoc/>
        public bool IsValid(sbyte value) => value <= _comparisonValue.AsInt64;

        // Floating point validation
        /// <inheritdoc/>
        public bool IsValid(double value) => value <= _comparisonValue.AsDouble;
        /// <inheritdoc/>
        public bool IsValid(float value) => value <= _comparisonValue.AsFloat;
        /// <inheritdoc/>
        public bool IsValid(decimal value) => value <= _comparisonValue.AsDecimal;

        // IValidationAttribute implementations
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, byte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsUInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, sbyte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, short value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, ushort value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsUInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, int value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, uint value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsUInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, long value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, ulong value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsUInt64);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, float value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsFloat);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, double value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsDouble);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, decimal value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _comparisonValue.AsDecimal);
        }
    }
}