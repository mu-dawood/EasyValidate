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
        /// Formats a validation message using the template and arguments from an <see cref="AttributeResult"/> and the validated value.
        /// </summary>
        /// <typeparam name="T">The type of the validated value.</typeparam>
        /// <param name="result">The result of attribute validation containing the message template and arguments.</param>
        /// <param name="value">The value that was validated (may be used for custom formatting).</param>
        /// <returns>The formatted message with arguments substituted into the template.</returns>
        /// <example>
        /// <code>
        /// var formatter = new CustomFormatter();
        /// var result = AttributeResult.Fail("Value must be between {0} and {1}", 10, 100);
        /// var message = formatter.Format(result, 42);
        /// // Returns: "Value must be between 10 and 100"
        /// </code>
        /// </example>
        /// <docs-member>Format(string, object[])</docs-member>
        /// <docs-type>Method</docs-type>
        /// <docs-return-type>string</docs-return-type>
        string Format<T>(AttributeResult result, T value);
    }
}
