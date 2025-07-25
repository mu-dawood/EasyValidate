using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validation attribute to mark a property or field as optional (null is allowed, no error is reported).
    /// Inherits from NotNullAttribute but overrides the strategy and error message to allow null values.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Optional]
    ///     public string? Nickname { get; set; } // Valid: null, "Alice", ""
    /// }
    /// </code>
    /// </example>
    public sealed class OptionalAttribute : GeneralValidationAttributeBase
    {
        public static readonly Lazy<OptionalAttribute> Instance = new(() => new OptionalAttribute());
        /// <inheritdoc/>
        public override ExecutionStrategy Strategy => ExecutionStrategy.SkipErrorAndStop;

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "OptionalValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, object? value)
        {
            return AttributeResult.Success();
        }
    }
}
