namespace EasyValidate.Core.Abstraction
{
    /// <summary>
    /// Represents the result of an attribute validation operation.
    /// </summary>
    /// <docs-display-name>Attribute Validation Result</docs-display-name>
    /// <docs-icon>CheckSquare</docs-icon>
    /// <docs-description>Encapsulates the result of a single attribute validation, including success status, error messages, and formatting arguments.</docs-description>
    public sealed class AttributeResult<TOutput>(bool isValid, TOutput? transformedValue, params object?[] messageArgs)
    {
        /// <summary>
        /// Gets a value indicating whether the validation was successful.
        /// </summary>
        public bool IsValid { get; } = isValid;

        /// <summary>
        /// Gets or sets the arguments used for formatting the validation message.
        /// </summary>
        /// <value>An array of objects that provide values for message template placeholders.</value>
        /// <remarks>
        /// These arguments are used in conjunction with the Message property to create formatted error messages.
        /// The arguments are typically inserted into the message template using standard .NET string formatting.
        /// </remarks>
        /// <example>
        /// <code>
        /// var result = new AttributeResult {
        ///     Message = "Value must be between {0} and {1}",
        ///     MessageArgs = new object[] { 10, 100 }
        /// };
        /// // Results in: "Value must be between 10 and 100"
        /// </code>
        /// </example>
        /// <docs-member>MessageArgs</docs-member>
        /// <docs-type>Property</docs-type>
        /// <docs-return-type>object?[]</docs-return-type>
        public object?[] MessageArgs { get; } = messageArgs;

        /// <summary>
        /// Gets the transformed value from the validation operation.
        /// </summary>
        public TOutput? TransformedValue { get; } = transformedValue;

    }
}
