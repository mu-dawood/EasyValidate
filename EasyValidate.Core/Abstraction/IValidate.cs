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
        /// Validates the object and returns a ValidationResult.
        /// </summary>
        /// <param name="formatter">The formatter to use for formatting the validation result.</param>
        /// <param name="configureValidator">The configurator to use for configuring validation attributes.</param>
        /// <param name="parentPath"></param>
        /// <returns>A ValidationResult containing the validation errors.</returns>
        /// <example>
        /// <code>
        /// var formatter = new CustomFormatter();
        /// var configurator = new CustomConfigurator();
        /// var result = myObject.Validate(formatter, configurator);
        /// if (!result.IsValid()) {
        ///     // Handle validation errors
        /// }
        /// </code>
        /// </example>
        /// <docs-member>Validate()</docs-member>
        /// <docs-type>Method</docs-type>
        IValidationResult Validate(IFormatter formatter, IConfigureValidator configureValidator, params string[] parentPath);

        /// <summary>
        /// Validates the object and returns a ValidationResult using the specified formatter and default configurator.
        /// </summary>
        /// <param name="formatter">The formatter to use for formatting the validation result.</param>
        /// <param name="parentPath"></param>
        /// <returns>A ValidationResult containing the validation errors.</returns>
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
        IValidationResult Validate(IFormatter formatter, params string[] parentPath);

        /// <summary>
        /// Validates the object and returns a ValidationResult using the default formatter and specified configurator.
        /// </summary>
        /// <param name="configureValidator">The configurator to use for configuring validation attributes.</param>
        /// <param name="parentPath"></param>
        /// <returns>A ValidationResult containing the validation errors.</returns>
        /// <example>
        /// <code>
        /// var configurator = new CustomConfigurator();
        /// var result = myObject.Validate(configurator);
        /// if (!result.IsValid()) {
        ///     // Handle validation errors
        /// }
        /// </code>
        /// </example>
        /// <docs-member>Validate(IConfigureValidator)</docs-member>
        /// <docs-type>Method</docs-type>
        IValidationResult Validate(IConfigureValidator configureValidator, params string[] parentPath);

        /// <summary>
        /// Validates the object and returns a ValidationResult using the default formatter and configurator.
        /// </summary>
        /// <returns>A ValidationResult containing the validation errors.</returns>
        /// <remarks>
        /// This method uses the default formatter and default configurator for validation.
        /// </remarks>
        /// <example>
        /// <code>
        /// var result = myObject.Validate();
        /// if (result.HasErrors()) {
        ///     Console.WriteLine("Validation failed!");
        /// }
        /// </code>
        /// </example>
        /// <docs-member>Validate() (Default)</docs-member>
        /// <docs-type>Method</docs-type>
        IValidationResult Validate(params string[] parentPath);

    }
}