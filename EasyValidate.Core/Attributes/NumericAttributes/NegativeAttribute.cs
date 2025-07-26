using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a numeric value is negative (less than zero).
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Negative]
    ///     public int Deficit { get; set; } // Valid: -1, -100, -50, Invalid: 0, 1, 100
    ///     
    ///     [Negative]
    ///     public double Loss { get; set; } // Valid: -0.1, -5.5, -1000.0, Invalid: 0.0, 5.5
    /// }
    /// </code>
    /// </example>
    public class NegativeAttribute : NumericValidationAttributeBase,
        IValidationAttribute<sbyte>,
        IValidationAttribute<short>,
        IValidationAttribute<int>,
        IValidationAttribute<long>,
        IValidationAttribute<float>,
        IValidationAttribute<double>,
        IValidationAttribute<decimal>
    {
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NegativeValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must be a negative number.";

        // Integer validation
        public static bool IsValid(long value) => value < 0;
        public static bool IsValid(int value) => value < 0;
        public static bool IsValid(short value) => value < 0;
        public static bool IsValid(sbyte value) => value < 0;

        // Floating point/decimal validation
        public static bool IsValid(double value) => value < 0;
        public static bool IsValid(float value) => value < 0;
        public static bool IsValid(decimal value) => value < 0;

        // IValidationAttribute implementations
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, sbyte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, short value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, int value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, long value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, float value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, double value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, decimal value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
    }
}
