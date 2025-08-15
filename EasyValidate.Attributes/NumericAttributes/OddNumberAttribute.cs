using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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
        IValidationAttribute<sbyte>,
        IValidationAttribute<short>,
        IValidationAttribute<int>,
        IValidationAttribute<long>,
        IValidationAttribute<byte>,
        IValidationAttribute<ushort>,
        IValidationAttribute<uint>,
        IValidationAttribute<ulong>
    {

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "OddNumberValidationError";


        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must be an odd number.";

        // Integer validation
        /// <inheritdoc/>
        public static bool IsValid(long value) => value % 2 != 0;
        /// <inheritdoc/>
        public static bool IsValid(int value) => value % 2 != 0;
        /// <inheritdoc/>
        public static bool IsValid(short value) => value % 2 != 0;
        /// <inheritdoc/>
        public static bool IsValid(sbyte value) => value % 2 != 0;
        /// <inheritdoc/>
        public static bool IsValid(ulong value) => value % 2 != 0;
        /// <inheritdoc/>
        public static bool IsValid(uint value) => value % 2 != 0;
        /// <inheritdoc/>
        public static bool IsValid(ushort value) => value % 2 != 0;
        /// <inheritdoc/>
        public static bool IsValid(byte value) => value % 2 != 0;

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
    }
}
