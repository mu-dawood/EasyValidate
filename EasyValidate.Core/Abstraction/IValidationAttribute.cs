using System.ComponentModel;

namespace EasyValidate.Core.Abstraction
{
    /// <summary>
    /// Controls how a validation attribute is executed in a chain.
    /// </summary>
    public enum ExecutionStrategy
    {
        /// Run validation. If it fails, add error and stop the chain.
        ValidateAndStop = 0,

        /// Check conditional method. If false, skip and stop the chain.
        ConditionalAndStopChain = 1,

        /// Check conditional method. If false, skip but continue the chain.
        ConditionalAndContinue = 2,

        /// Always run. Add error if failed, but continue the chain regardless.
        ValidateErrorAndContinue,
        [EditorBrowsable(EditorBrowsableState.Never)]
        SkipErrorAndStop
    }

    public interface IValidationAttribute
    {
        /// <summary>
        /// Gets the error code for this validation attribute.
        /// </summary>
        string ErrorCode { get; }

        /// <summary>
        /// Gets or sets the name of a method that returns a boolean value to determine if this validation should be executed.
        /// The method must be parameterless and return bool. If null or empty, validation always executes.
        /// </summary>
        string? ConditionalMethod { get; }

        /// <summary>
        /// Gets or sets the execution strategy for this validation attribute.
        /// Determines how this validation interacts with the validation chain, such as whether to skip if input is null,
        /// stop on the first failure, or run conditionally based on a method.
        /// if null, defaults to <see cref="ExecutionStrategy.ValidateAndStop"/>.
        /// </summary>
        ExecutionStrategy Strategy { get; }


        /// <summary>
        /// Gets the name of the validation chain this attribute belongs to.
        /// </summary>
        string Chain { get; }
    }

    /// <summary>
    /// Defines the contract for validation attributes that can validate and transform input values.
    /// </summary>
    /// <typeparam name="TInput">The input type to validate.</typeparam>
    /// <typeparam name="TOutput">The output type after validation and potential transformation.</typeparam>
    /// <docs-display-name>Validation Attribute Interface</docs-display-name>
    /// <docs-icon>Shield</docs-icon>
    /// <docs-description>Core interface for validation attributes providing validation logic, error handling, and value transformation capabilities.</docs-description>
    public interface IValidationAttribute<TInput, TOutput> : IValidationAttribute
    {


        /// <summary>
        /// Validates the specified value and potentially transforms it.
        /// </summary>
        /// <param name="obj">The main object being validated.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="value">The value to validate.</param>
        /// <param name="output">The output value after validation and transformation. If validation fails, this will be set to <c>default</c>.</param>
        /// <returns>
        /// An <see cref="AttributeResult"/> indicating success or failure. If successful, <paramref name="output"/> contains the transformed value; otherwise, it is <c>default</c>.
        /// </returns>
        AttributeResult Validate(object obj, string propertyName, TInput value, out TOutput output);
    }

    public interface IValidationAttribute<TInput> : IValidationAttribute
    {
        /// <summary>
        /// Validates the specified value without transforming it.
        /// </summary>
        /// <param name="obj">The main object being validated.</param>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="value">The value to validate.</param>
        /// <returns>
        /// An <see cref="AttributeResult"/> indicating whether the value is valid or not.
        /// </returns>
        AttributeResult Validate(object obj, string propertyName, TInput value);
    }
}
