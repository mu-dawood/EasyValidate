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
        public AttributeResult Validate(object obj, string propertyName, sbyte value, out sbyte output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(object obj, string propertyName, short value, out short output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(object obj, string propertyName, int value, out int output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(object obj, string propertyName, long value, out long output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(object obj, string propertyName, float value, out float output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(object obj, string propertyName, double value, out double output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(object obj, string propertyName, decimal value, out decimal output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
    }
}
