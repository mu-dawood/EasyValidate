using System;

namespace EasyValidate.Core.Abstraction
{
    /// <summary>
    /// Interface for validation.
    /// </summary>
    /// <docs-display-name>Validation Interface</docs-display-name>
    /// <docs-icon>CheckCircle</docs-icon>
    /// <docs-description>Core validation interface that provides the contract for validating objects and returning formatted validation results.</docs-description>
    public interface IValidate
    {

        /// <summary>
        /// Validates the object and returns a validation result using the specified formatter.
        /// </summary>
        /// <param name="formatter">The <see cref="IFormatter"/> to use for formatting the validation result.</param>
        /// <returns>An <see cref="IValidationResult"/> containing the validation results for this object.</returns>
        /// <example>
        /// <code>
        /// var formatter = new CustomFormatter();
        /// var result = myObject.Validate(formatter);
        /// if (!result.IsValid()) {
        ///     // Handle validation errors
        /// }
        /// </code>
        /// </example>
        /// <docs-member>Validate(IFormatter)</docs-member>
        /// <docs-type>Method</docs-type>
        IValidationResult Validate(IFormatter formatter);

        /// <summary>
        /// Validates the object and returns a validation result using the specified service provider for dependency resolution.
        /// </summary>
        /// <param name="provider">The <see cref="IServiceProvider"/> used to resolve dependencies or services for validation.</param>
        /// <returns>An <see cref="IValidationResult"/> containing the validation results for this object.</returns>
        /// <example>
        /// <code>
        /// var provider = new ServiceCollection().BuildServiceProvider();
        /// var result = myObject.Validate(provider);
        /// if (!result.IsValid()) {
        ///     // Handle validation errors
        /// }
        /// </code>
        /// </example>
        /// <docs-member>Validate(IServiceProvider)</docs-member>
        /// <docs-type>Method</docs-type>
        IValidationResult Validate(IServiceProvider provider);


        /// <summary>
        /// Validates the object and returns a validation result using the default formatter and configuration.
        /// </summary>
        /// <returns>An <see cref="IValidationResult"/> containing the validation results for this object.</returns>
        /// <example>
        /// <code>
        /// var result = myObject.Validate();
        /// if (!result.IsValid()) {
        ///     // Handle validation errors
        /// }
        /// </code>
        /// </example>
        /// <docs-member>Validate()</docs-member>
        /// <docs-type>Method</docs-type>
        IValidationResult Validate();


    }
}