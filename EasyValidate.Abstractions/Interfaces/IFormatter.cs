namespace EasyValidate.Abstractions;

/// <summary>
/// Provides an interface for formatting validation messages using templates and arguments.
/// </summary>
/// <docs-display-name>Message Formatter Interface</docs-display-name>
/// <docs-icon>MessageSquare</docs-icon>
/// <docs-description>
/// Used to format validation error messages with dynamic arguments and placeholders.
/// </docs-description>
public interface IFormatter
{
    /// <summary>
    /// Formats a validation message using a template string and arguments.
    /// </summary>
    /// <param name="messageTemplate">The message template containing placeholders (e.g., "Value must be between {0} and {1}").</param>
    /// <param name="args">Arguments to substitute into the template.</param>
    /// <returns>The formatted message with arguments substituted into the template.</returns>
    /// <example>
    /// <code>
    /// var formatter = new CustomFormatter();
    /// var message = formatter.Format("Value must be between {0} and {1}", 10, 100);
    /// // Returns: "Value must be between 10 and 100"
    /// </code>
    /// </example>
    /// <docs-member>Format(string, object[])</docs-member>
    /// <docs-type>Method</docs-type>
    /// <docs-return-type>string</docs-return-type>
    string Format(string messageTemplate, object[] args);
}

