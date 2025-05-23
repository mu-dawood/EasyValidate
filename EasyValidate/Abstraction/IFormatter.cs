namespace EasyValidate.Abstraction
{
    /// <summary>
    /// Defines a formatter interface for formatting messages with arguments.
    /// </summary>
    public interface IFormatter
    {
        /// <summary>
        /// Formats a message with the provided arguments.
        /// </summary>
        /// <param name="message">The message template.</param>
        /// <param name="args">The arguments to format the message with.</param>
        /// <returns>The formatted message.</returns>
        string Format(string message, object?[] args);
    }
}
