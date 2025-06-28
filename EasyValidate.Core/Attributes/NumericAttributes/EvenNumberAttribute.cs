using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a numeric value is even.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [EvenNumber]
    ///     public int PlayerNumber { get; set; } // Valid: 2, 4, 6, 100, Invalid: 1, 3, 5, 99
    ///     
    ///     [EvenNumber]
    ///     public int BatchSize { get; set; } // Valid: 10, 50, 1000, Invalid: 15, 33, 999
    /// }
    /// </code>
    /// </example>
    public class EvenNumberAttribute : NumericValidationAttributeBase,
        IValidationAttribute<byte, byte>,
        IValidationAttribute<sbyte, sbyte>,
        IValidationAttribute<short, short>,
        IValidationAttribute<ushort, ushort>,
        IValidationAttribute<int, int>,
        IValidationAttribute<uint, uint>,
        IValidationAttribute<long, long>,
        IValidationAttribute<ulong, ulong>
    {
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "EvenNumberValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be an even number.";

        // Integer validation
        public static bool IsValid(long value) => value % 2 == 0;
        public static bool IsValid(ulong value) => value % 2 == 0;
        public static bool IsValid(int value) => value % 2 == 0;
        public static bool IsValid(uint value) => value % 2 == 0;
        public static bool IsValid(short value) => value % 2 == 0;
        public static bool IsValid(ushort value) => value % 2 == 0;
        public static bool IsValid(byte value) => value % 2 == 0;
        public static bool IsValid(sbyte value) => value % 2 == 0;

        // IValidationAttribute implementations
        public AttributeResult<byte> Validate(object obj, string propertyName, byte value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<sbyte> Validate(object obj, string propertyName, sbyte value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<short> Validate(object obj, string propertyName, short value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<ushort> Validate(object obj, string propertyName, ushort value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<int> Validate(object obj, string propertyName, int value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<uint> Validate(object obj, string propertyName, uint value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<long> Validate(object obj, string propertyName, long value) =>
            new(IsValid(value), value, propertyName);
        public AttributeResult<ulong> Validate(object obj, string propertyName, ulong value) =>
            new(IsValid(value), value, propertyName);
    }
}
