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
        IValidationAttribute<sbyte, sbyte>,
        IValidationAttribute<short, short>,
        IValidationAttribute<int, int>,
        IValidationAttribute<long, long>,
        IValidationAttribute<float, float>,
        IValidationAttribute<double, double>,
        IValidationAttribute<decimal, decimal>
    {
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NegativeValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be a negative number.";

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
        public AttributeResult<sbyte> Validate(object obj, string propertyName, sbyte value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<short> Validate(object obj, string propertyName, short value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<int> Validate(object obj, string propertyName, int value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<long> Validate(object obj, string propertyName, long value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<float> Validate(object obj, string propertyName, float value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<double> Validate(object obj, string propertyName, double value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<decimal> Validate(object obj, string propertyName, decimal value) =>
            new(IsValid(value), value, propertyName);
    }
}
