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
        IValidationAttribute<sbyte, sbyte>,
        IValidationAttribute<short, short>,
        IValidationAttribute<int, int>,
        IValidationAttribute<long, long>,
        IValidationAttribute<float, float>,
        IValidationAttribute<double, double>,
        IValidationAttribute<decimal, decimal>
    {
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "PositiveValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be a positive number.";

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
        public AttributeResult<sbyte> Validate(object obj, string propertyName, sbyte value) =>
            new(PositiveAttribute.IsValid(value), value, propertyName);
        public AttributeResult<short> Validate(object obj, string propertyName, short value) =>
            new(PositiveAttribute.IsValid(value), value, propertyName);
        public AttributeResult<int> Validate(object obj, string propertyName, int value) =>
            new(PositiveAttribute.IsValid(value), value, propertyName);
        public AttributeResult<long> Validate(object obj, string propertyName, long value) =>
            new(PositiveAttribute.IsValid(value), value, propertyName);
        public AttributeResult<float> Validate(object obj, string propertyName, float value) =>
            new(PositiveAttribute.IsValid(value), value, propertyName);
        public AttributeResult<double> Validate(object obj, string propertyName, double value) =>
            new(PositiveAttribute.IsValid(value), value, propertyName);
        public AttributeResult<decimal> Validate(object obj, string propertyName, decimal value) =>
            new(PositiveAttribute.IsValid(value), value, propertyName);
    }
}
