namespace EasyValidate.Core.Abstraction
{
    /// <summary>
    /// Interface for configuring validation behavior for attributes.
    /// </summary>
    /// <docs-display-name>Validator Configuration Interface</docs-display-name>
    /// <docs-icon>Settings</docs-icon>
    /// <docs-description>Interface for configuring validation behavior, nullable handling, message formatting, and other validation aspects for specific attribute types.</docs-description>
    public interface IConfigureValidator
    {
        /// <summary>
        /// Configures validation behavior for a specific attribute type.
        /// </summary>
        /// <typeparam name="TAttribute">The type of validation attribute to configure.</typeparam>
        /// <param name="attribute">The validation attribute instance to configure.</param>
        /// <returns>The configured attribute instance.</returns>
        /// <remarks>
        /// This method provides a way to configure validation attributes with custom behavior.
        /// The implementation can modify the attribute's properties or apply custom configuration logic.
        /// </remarks>
        /// <example>
        /// <code>
        /// var configurator = new CustomConfigurator();
        /// var model = new UserModel();
        /// var result = model.Validate(configurator);
        /// </code>
        /// </example>
        /// <docs-member>Configure()</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>TAttribute</docs-return-type>
        IValidationAttribute<TInput, TOutput> Configure<TInput, TOutput>(IValidationAttribute<TInput, TOutput> validator);
    }
}
