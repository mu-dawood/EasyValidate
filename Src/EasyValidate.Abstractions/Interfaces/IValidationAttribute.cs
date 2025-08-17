namespace EasyValidate.Abstractions
{

    /// <summary>
    /// Represents a validation attribute that can be applied to a member, providing metadata for error handling,
    /// conditional execution, and validation chaining.
    /// </summary>
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
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="value">The value to validate.</param>
        /// <returns>
        /// An <see cref="AttributeResult{TOutput}"/> containing the validation result and the transformed value (or <c>default</c> if validation fails).
        /// </returns>
        AttributeResult<TOutput> Validate(string propertyName, TInput value);
    }
    
    /// <summary>
    /// Defines a contract for validation attributes that operate on a specific input type.
    /// </summary>
    /// <typeparam name="TInput">
    /// The type of the value to be validated.
    /// </typeparam>
    public interface IValidationAttribute<TInput> : IValidationAttribute
    {
        /// <summary>
        /// Validates the specified value without transforming it.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="value">The value to validate.</param>
        /// <returns>
        /// An <see cref="AttributeResult"/> indicating whether the value is valid or not.
        /// </returns>
        AttributeResult Validate(string propertyName, TInput value);
    }


    /// <summary>
    /// Async validation attribute interface for operations that do not transform output.
    /// </summary>
    /// <typeparam name="TInput">The input type to validate.</typeparam>
    public interface IAsyncValidationAttribute<TInput> : IValidationAttribute
    {
        /// <summary>
        /// Asynchronously validates the specified value without transforming it, using the provided service provider for dependency resolution.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="value">The value to validate.</param>
        /// <returns>
        /// A <see cref="Task{AttributeResult}"/> indicating whether the value is valid or not.
        /// </returns>
        Task<AttributeResult> ValidateAsync(string propertyName, TInput value);
    }

    /// <summary>
    /// Async validation attribute interface for operations that transform output.
    /// </summary>
    /// <typeparam name="TInput">The input type to validate.</typeparam>
    /// <typeparam name="TOutput">The output type after validation and potential transformation.</typeparam>
    public interface IAsyncValidationAttribute<TInput, TOutput> : IValidationAttribute
    {
        /// <summary>
        /// Asynchronously validates the specified value and potentially transforms it.
        /// </summary>
        /// <param name="propertyName">The name of the property being validated.</param>
        /// <param name="value">The value to validate.</param>
        /// <returns>
        /// A Task containing an <see cref="AttributeResult{TOutput}"/> with the validation result and the transformed value (or <c>default</c> if validation fails).
        /// </returns>
        Task<AttributeResult<TOutput>> ValidateAsync(string propertyName, TInput value);

    }
}
