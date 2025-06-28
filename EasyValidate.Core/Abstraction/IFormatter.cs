namespace EasyValidate.Core.Abstraction
{
    /// <summary>
    /// Defines a formatter interface for formatting messages with arguments.
    /// </summary>
    /// <docs-display-name>Message Formatter Interface</docs-display-name>
    /// <docs-icon>MessageSquare</docs-icon>
    /// <docs-description>Interface for formatting validation error messages with dynamic arguments and placeholders.</docs-description>
    public interface IFormatter
    {
        /// <summary>
        /// Formats a message template with the provided arguments.
        /// </summary>
        /// <param name="message">The message template with placeholders.</param>
        /// <param name="args">The arguments to substitute into the message template.</param>
        /// <returns>The formatted message with arguments substituted.</returns>
        /// <example>
        /// <code>
        /// var formatter = new CustomFormatter();
        /// var result = formatter.Format("Hello {0}!", "World");
        /// // Returns: "Hello World!"
        /// </code>
        /// </example>
        /// <docs-member>Format(string, object[])</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>string</docs-return-type>
        string Format(string message, params object[] args);

        /// <summary>
        /// Formats a message using the validation attribute and arguments.
        /// </summary>
        /// <typeparam name="TInput">The input type for the validation attribute.</typeparam>
        /// <typeparam name="TOutput">The output type for the validation attribute.</typeparam>
        /// <param name="attribute">The validation attribute instance.</param>
        /// <param name="args">The arguments for message formatting.</param>
        /// <returns>The formatted message.</returns>
        /// <example>
        /// <code>
        /// var formatter = new CustomFormatter();
        /// var result = formatter.GetFormatedMessage(attribute, new object[] { "value" });
        /// </code>
        /// </example>
        /// <docs-member>GetFormatedMessage()</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>string</docs-return-type>
        string GetFormatedMessage<TInput, TOutput>(IValidationAttribute<TInput, TOutput> attribute, object?[] args);
    }
}
