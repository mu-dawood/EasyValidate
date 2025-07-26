using System;
using System.Threading.Tasks;

namespace EasyValidate.Core.Abstraction
{
    /// <summary>
    /// Interface for validation.
    /// </summary>
    /// <docs-display-name>Validation Interface</docs-display-name>
    /// <docs-icon>CheckCircle</docs-icon>
    /// <docs-description>Core validation interface that provides the contract for validating objects and returning formatted validation results.</docs-description>
    public interface IAsyncValidate
    {

        /// <summary>
        /// Asynchronously validates the object and returns a validation result using the specified formatter.
        /// </summary>
        /// <param name="formatter">The <see cref="IFormatter"/> to use for formatting the validation result.</param>
        /// <returns>A <see cref="ValueTask{IValidationResult}"/> containing the validation results for this object.</returns>
        /// <example>
        /// <code>
        /// var formatter = new CustomFormatter();
        /// var result = await myObject.ValidateAsync(formatter);
        /// if (!result.IsValid()) {
        ///     // Handle validation errors
        /// }
        /// </code>
        /// </example>
        /// <docs-member>ValidateAsync(IFormatter)</docs-member>
        /// <docs-type>Method</docs-type>
        ValueTask<IValidationResult> ValidateAsync(IFormatter formatter);

        /// <summary>
        /// Asynchronously validates the object and returns a validation result using the specified service provider for dependency resolution.
        /// </summary>
        /// <param name="provider">The <see cref="IServiceProvider"/> used to resolve dependencies or services for validation.</param>
        /// <returns>A <see cref="ValueTask{IValidationResult}"/> containing the validation results for this object.</returns>
        /// <example>
        /// <code>
        /// var provider = new ServiceCollection().BuildServiceProvider();
        /// var result = await myObject.ValidateAsync(provider);
        /// if (!result.IsValid()) {
        ///     // Handle validation errors
        /// }
        /// </code>
        /// </example>
        /// <docs-member>ValidateAsync(IServiceProvider)</docs-member>
        /// <docs-type>Method</docs-type>
        ValueTask<IValidationResult> ValidateAsync(IServiceProvider provider);


        /// <summary>
        /// Asynchronously validates the object and returns a validation result using the default formatter and configuration.
        /// </summary>
        /// <returns>A <see cref="ValueTask{IValidationResult}"/> containing the validation results for this object.</returns>
        /// <example>
        /// <code>
        /// var result = await myObject.ValidateAsync();
        /// if (!result.IsValid()) {
        ///     // Handle validation errors
        /// }
        /// </code>
        /// </example>
        /// <docs-member>ValidateAsync()</docs-member>
        /// <docs-type>Method</docs-type>
        ValueTask<IValidationResult> ValidateAsync();


    }
}