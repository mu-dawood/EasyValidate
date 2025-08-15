using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a numeric value is not zero.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [NotZero]
    ///     public int Divisor { get; set; } // Valid: 1, -1, 5, -10, Invalid: 0
    ///     
    ///     [NotZero]
    ///     public double Coefficient { get; set; } // Valid: 0.1, -0.5, 100.0, Invalid: 0.0
    /// }
    /// </code>
    /// </example>
    public class NotZeroAttribute : NumericValidationAttributeBase,
        IValidationAttribute<sbyte>,
        IValidationAttribute<short>,
        IValidationAttribute<int>,
        IValidationAttribute<long>,
        IValidationAttribute<byte>,
        IValidationAttribute<ushort>,
        IValidationAttribute<uint>,
        IValidationAttribute<ulong>,
        IValidationAttribute<float>,
        IValidationAttribute<double>,
        IValidationAttribute<decimal>
    {

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NotZeroValidationError";


        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must not be zero.";

        // Integer validation
        /// <inheritdoc/>
        public static bool IsValid(long value) => value != 0;
        /// <inheritdoc/>
        public static bool IsValid(int value) => value != 0;
        /// <inheritdoc/>
        public static bool IsValid(short value) => value != 0;
        /// <inheritdoc/>
        public static bool IsValid(sbyte value) => value != 0;
        /// <inheritdoc/>
        public static bool IsValid(ulong value) => value != 0;
        /// <inheritdoc/>
        public static bool IsValid(uint value) => value != 0;
        /// <inheritdoc/>
        public static bool IsValid(ushort value) => value != 0;
        /// <inheritdoc/>
        public static bool IsValid(byte value) => value != 0;

        // Floating point/decimal validation
        /// <inheritdoc/>
        public static bool IsValid(double value) => value != 0;
        /// <inheritdoc/>
        public static bool IsValid(float value) => value != 0;
        /// <inheritdoc/>
        public static bool IsValid(decimal value) => value != 0;

        // IValidationAttribute implementations
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, sbyte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, short value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, int value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, long value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, byte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, ushort value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, uint value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, ulong value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, float value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, double value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, decimal value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
    }
}
