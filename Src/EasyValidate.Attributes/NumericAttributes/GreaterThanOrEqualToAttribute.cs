using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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
        public GreaterThanOrEqualToAttribute(int value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public GreaterThanOrEqualToAttribute(long value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public GreaterThanOrEqualToAttribute(double value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public GreaterThanOrEqualToAttribute(decimal value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public GreaterThanOrEqualToAttribute(float value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public GreaterThanOrEqualToAttribute(short value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public GreaterThanOrEqualToAttribute(ushort value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public GreaterThanOrEqualToAttribute(uint value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public GreaterThanOrEqualToAttribute(ulong value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public GreaterThanOrEqualToAttribute(byte value) { _comparisonValue = value; }
        /// <inheritdoc/>
        public GreaterThanOrEqualToAttribute(sbyte value) { _comparisonValue = value; }


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "GreaterThanOrEqualToValidationError";


        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must be greater than or equal to {1}.";

        // Integer validation
        /// <inheritdoc/>
        public bool IsValid(long value) => value >= _comparisonValue.AsInt64;
        /// <inheritdoc/>
        public bool IsValid(ulong value) => value >= _comparisonValue.AsUInt64;
        /// <inheritdoc/>
        public bool IsValid(int value) => value >= _comparisonValue.AsInt64;
        /// <inheritdoc/>
        public bool IsValid(uint value) => value >= _comparisonValue.AsUInt64;
        /// <inheritdoc/>
        public bool IsValid(short value) => value >= _comparisonValue.AsInt64;
        /// <inheritdoc/>
        public bool IsValid(ushort value) => value >= _comparisonValue.AsUInt64;
        /// <inheritdoc/>
        public bool IsValid(byte value) => value >= _comparisonValue.AsUInt64;
        /// <inheritdoc/>
        public bool IsValid(sbyte value) => value >= _comparisonValue.AsInt64;

        // Floating point validation
        /// <inheritdoc/>
        public bool IsValid(double value) => value >= _comparisonValue.AsDouble;
        /// <inheritdoc/>
        public bool IsValid(float value) => value >= _comparisonValue.AsFloat;
        /// <inheritdoc/>
        public bool IsValid(decimal value) => value >= _comparisonValue.AsDecimal;

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