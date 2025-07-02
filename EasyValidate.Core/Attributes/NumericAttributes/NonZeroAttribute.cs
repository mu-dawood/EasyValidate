using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
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
        IValidationAttribute<sbyte, sbyte>,
        IValidationAttribute<short, short>,
        IValidationAttribute<int, int>,
        IValidationAttribute<long, long>,
        IValidationAttribute<byte, byte>,
        IValidationAttribute<ushort, ushort>,
        IValidationAttribute<uint, uint>,
        IValidationAttribute<ulong, ulong>,
        IValidationAttribute<float, float>,
        IValidationAttribute<double, double>,
        IValidationAttribute<decimal, decimal>
    {
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "NotZeroValidationError";

        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must not be zero.";

        // Integer validation
        public static bool IsValid(long value) => value != 0;
        public static bool IsValid(int value) => value != 0;
        public static bool IsValid(short value) => value != 0;
        public static bool IsValid(sbyte value) => value != 0;
        public static bool IsValid(ulong value) => value != 0;
        public static bool IsValid(uint value) => value != 0;
        public static bool IsValid(ushort value) => value != 0;
        public static bool IsValid(byte value) => value != 0;

        // Floating point/decimal validation
        public static bool IsValid(double value) => value != 0;
        public static bool IsValid(float value) => value != 0;
        public static bool IsValid(decimal value) => value != 0;

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
        public AttributeResult Validate(object obj, string propertyName, byte value, out byte output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(object obj, string propertyName, ushort value, out ushort output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(object obj, string propertyName, uint value, out uint output)
        {
            output = value;
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        public AttributeResult Validate(object obj, string propertyName, ulong value, out ulong output)
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
