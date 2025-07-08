namespace EasyValidate.Core.Abstraction
{
    /// <summary>
    /// Interface for configuring validation behavior for attributes.
    /// </summary>
    /// <remarks>
    /// Provides methods to configure validation attributes, including nullable handling, message formatting, and other aspects for specific attribute types.
    /// </remarks>
    /// <example>
    /// <code>
    /// var configurator = new CustomConfigurator();
    /// var model = new UserModel();
    /// var result = model.Validate(configurator);
    /// </code>
    /// </example>
    public interface IConfigureValidator
    {
        /// <summary>
        /// Configures validation behavior for a specific attribute type.
        /// </summary>
        /// <typeparam name="TInput">The input type for the validation attribute.</typeparam>
        /// <typeparam name="TOutput">The output type for the validation attribute.</typeparam>
        /// <param name="validator">The validation attribute instance to configure.</param>
        /// <returns>The configured attribute instance.</returns>
        IValidationAttribute<TInput, TOutput> Configure<TInput, TOutput>(IValidationAttribute<TInput, TOutput> validator);
    }
}
