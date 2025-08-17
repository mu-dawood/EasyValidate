using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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
        IValidationAttribute<byte>,
        IValidationAttribute<sbyte>,
        IValidationAttribute<short>,
        IValidationAttribute<ushort>,
        IValidationAttribute<int>,
        IValidationAttribute<uint>,
        IValidationAttribute<long>,
        IValidationAttribute<ulong>
    {
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute.
        /// </summary>


        public override string ErrorCode { get; set; } = "EvenNumberValidationError";


        /// <inheritdoc/>
        public string ErrorMessage { get; set; } = "The {0} field must be an even number.";

        // Integer validation
        /// <inheritdoc/>
        public static bool IsValid(long value) => value % 2 == 0;
        /// <inheritdoc/>
        public static bool IsValid(ulong value) => value % 2 == 0;
        /// <inheritdoc/>
        public static bool IsValid(int value) => value % 2 == 0;
        /// <inheritdoc/>
        public static bool IsValid(uint value) => value % 2 == 0;
        /// <inheritdoc/>
        public static bool IsValid(short value) => value % 2 == 0;
        /// <inheritdoc/>
        public static bool IsValid(ushort value) => value % 2 == 0;
        /// <inheritdoc/>
        public static bool IsValid(byte value) => value % 2 == 0;
        /// <inheritdoc/>
        public static bool IsValid(sbyte value) => value % 2 == 0;

        // IValidationAttribute implementations
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, byte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
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
        public AttributeResult Validate(string propertyName, ushort value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, int value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, uint value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName);
        }
        /// <inheritdoc/>
        public AttributeResult Validate(string propertyName, long value)
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
