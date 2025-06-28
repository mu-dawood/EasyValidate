using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a numeric value is odd.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [OddNumber]
    ///     public int PlayerNumber { get; set; } // Valid: 1, 3, 5, 99, Invalid: 2, 4, 6, 100
    ///     
    ///     [OddNumber]
    ///     public int LuckyNumber { get; set; } // Valid: 7, 13, 21, Invalid: 8, 12, 20
    /// }
    /// </code>
    /// </example>
    public class OddNumberAttribute : NumericValidationAttributeBase,
        IValidationAttribute<sbyte, sbyte>,
        IValidationAttribute<short, short>,
        IValidationAttribute<int, int>,
        IValidationAttribute<long, long>,
        IValidationAttribute<byte, byte>,
        IValidationAttribute<ushort, ushort>,
        IValidationAttribute<uint, uint>,
        IValidationAttribute<ulong, ulong>
    {
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "OddNumberValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be an odd number.";

        // Integer validation
        public static bool IsValid(long value) => value % 2 != 0;
        public static bool IsValid(int value) => value % 2 != 0;
        public static bool IsValid(short value) => value % 2 != 0;
        public static bool IsValid(sbyte value) => value % 2 != 0;
        public static bool IsValid(ulong value) => value % 2 != 0;
        public static bool IsValid(uint value) => value % 2 != 0;
        public static bool IsValid(ushort value) => value % 2 != 0;
        public static bool IsValid(byte value) => value % 2 != 0;

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
    }
}
