namespace EasyValidate.Abstractions
{


    /// <summary>
    /// Core validation interface that provides the contract for validating objects and returning formatted validation results.
    /// </summary>
    /// <docs-display-name>Validation Interface</docs-display-name>
    /// <docs-icon>CheckCircle</docs-icon>
    /// <docs-description>Provides methods for validating objects and returning formatted validation results.</docs-description>
    public interface IValidate
    {
        /// <summary>
        /// Validates the object and returns a validation result using the default formatter and configuration.
        /// </summary>
        /// <returns>A validation result containing errors and status for this object.</returns>
        /// <example>
        /// var result = myObject.Validate();
        /// if (!result.IsValid()) {
        ///     // Handle validation errors
        /// }
        /// </example>
        IValidationResult Validate(ValidationConfig? config = null);

        /// <summary>
        /// Validates the object using an action to configure a <see cref="ValidationConfig"/> instance.
        /// </summary>
        /// <param name="configure">An action that configures the <see cref="ValidationConfig"/>.</param>
        /// <returns>A validation result containing errors and status for this object.</returns>
        /// <example>
        /// var result = myObject.Validate(cfg => cfg.SetFormatter(...));
        /// </example>
        IValidationResult Validate(Action<ValidationConfig> configure);
    }


    /// <summary>
    /// Asynchronous validation interface that provides the contract for validating objects and returning formatted validation results asynchronously.
    /// </summary>
    /// <docs-display-name>Async Validation Interface</docs-display-name>
    /// <docs-icon>CheckCircle</docs-icon>
    /// <docs-description>Provides asynchronous methods for validating objects and returning formatted validation results.</docs-description>
    public interface IAsyncValidate
    {
        /// <summary>
        /// Asynchronously validates the object and returns a validation result using the default formatter and configuration, or the provided <see cref="ValidationConfig"/> if specified.
        /// </summary>
        /// <param name="config">Optional configuration object containing service provider and formatter.</param>
        /// <returns>A <see cref="ValueTask{IValidationResult}"/> containing errors and status for this object.</returns>
        /// <example>
        /// var result = await myObject.ValidateAsync();
        /// var resultWithConfig = await myObject.ValidateAsync(new ValidationConfig(...));
        /// if (!result.IsValid()) {
        ///     // Handle validation errors
        /// }
        /// </example>
        ValueTask<IValidationResult> ValidateAsync(ValidationConfig? config = null);

        /// <summary>
        /// Asynchronously validates the object using an action to configure a <see cref="ValidationConfig"/> instance.
        /// </summary>
        /// <param name="configure">An action that configures the <see cref="ValidationConfig"/>.</param>
        /// <returns>A <see cref="ValueTask{IValidationResult}"/> containing errors and status for this object.</returns>
        /// <example>
        /// var result = await myObject.ValidateAsync(cfg => cfg.SetFormatter(...));
        /// </example>
        ValueTask<IValidationResult> ValidateAsync(Action<ValidationConfig> configure);
    }

}