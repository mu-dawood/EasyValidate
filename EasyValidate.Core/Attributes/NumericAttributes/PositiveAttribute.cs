using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a numeric value is positive (greater than zero).
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Positive]
    ///     public int Quantity { get; set; } // Valid: 1, 5, 100, Invalid: 0, -1, -10
    ///     
    ///     [Positive]
    ///     public double Price { get; set; } // Valid: 0.01, 9.99, 1000.0, Invalid: 0.0, -5.50
    /// }
    /// </code>
    /// </example>
    public class PositiveAttribute : NumericValidationAttributeBase,
        IValidationAttribute<sbyte>,
        IValidationAttribute<short>,
        IValidationAttribute<int>,
        IValidationAttribute<long>,
        IValidationAttribute<float>,
        IValidationAttribute<double>,
        IValidationAttribute<decimal>
    {

        public override string ErrorCode { get; set; } = "PositiveValidationError";


        public string ErrorMessage { get; set; } = "The {0} field must be a positive number.";

        // Integer validation
        public static bool IsValid(long value) => value > 0;
        public static bool IsValid(int value) => value > 0;
        public static bool IsValid(short value) => value > 0;
        public static bool IsValid(sbyte value) => value > 0;

        // Floating point/decimal validation
        public static bool IsValid(double value) => value > 0;
        public static bool IsValid(float value) => value > 0;
        public static bool IsValid(decimal value) => value > 0;

        // IValidationAttribute implementations
        public AttributeResult Validate(string propertyName, sbyte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(string propertyName, short value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(string propertyName, int value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(string propertyName, long value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(string propertyName, float value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(string propertyName, double value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(string propertyName, decimal value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
    }
}
