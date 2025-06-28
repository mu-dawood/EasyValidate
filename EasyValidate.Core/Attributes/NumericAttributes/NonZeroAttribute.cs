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
        public override string ErrorMessage { get; set; } = "The {0} field must not be zero.";

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
        public AttributeResult<sbyte> Validate(object obj, string propertyName, sbyte value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<short> Validate(object obj, string propertyName, short value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<int> Validate(object obj, string propertyName, int value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<long> Validate(object obj, string propertyName, long value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<byte> Validate(object obj, string propertyName, byte value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<ushort> Validate(object obj, string propertyName, ushort value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<uint> Validate(object obj, string propertyName, uint value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<ulong> Validate(object obj, string propertyName, ulong value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<float> Validate(object obj, string propertyName, float value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<double> Validate(object obj, string propertyName, double value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<decimal> Validate(object obj, string propertyName, decimal value) =>
            new(IsValid(value), value, propertyName);
    }
}
