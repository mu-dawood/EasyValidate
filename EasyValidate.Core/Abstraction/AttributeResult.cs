namespace EasyValidate.Core.Abstraction
{
    /// <summary>
    /// Represents the result of an attribute validation operation.
    /// </summary>
    /// <docs-display-name>Attribute Validation Result</docs-display-name>
    /// <docs-icon>CheckSquare</docs-icon>
    /// <docs-description>Encapsulates the result of a single attribute validation, including success status, error messages, and formatting arguments.</docs-description>
    public readonly struct AttributeResult
    {
        private AttributeResult(bool isValid, string messageTemplate, object[]? args)
        {
            IsValid = isValid;
            MessageTemplate = messageTemplate;
            MessageArgs = args;
        }

        /// <summary>
        /// Gets a value indicating whether the validation was successful.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Gets the arguments used for formatting the validation message.
        /// </summary>
        /// <value>An array of objects that provide values for placeholders in the message template.</value>
        /// <remarks>
        /// These arguments are used with <see cref="MessageTemplate"/> to create formatted error messages.
        /// </remarks>
        public object[]? MessageArgs { get; }

        /// <summary>
        /// Gets the message template for the validation error, if validation failed.
        /// </summary>
        /// <value>The unformatted error message template, or <c>string.Empty</c> if validation succeeded.</value>
        public string MessageTemplate { get; }


        /// <summary>
        /// Creates a successful validation result.
        /// </summary>
        /// <returns>An <see cref="AttributeResult"/> representing a successful validation.</returns>
        public static AttributeResult Success() => new(true, string.Empty, null);

        /// <summary>
        /// Creates a failed validation result with the specified message template and arguments.
        /// </summary>
        /// <param name="template">The error message template.</param>
        /// <param name="args">Arguments for formatting the error message.</param>
        /// <returns>An <see cref="AttributeResult"/> representing a failed validation.</returns>
        public static AttributeResult Fail(string template, params object[] args)
            => new(false, template, args);

        /// <summary>
        /// Creates a successful validation result.
        /// </summary>
        /// <returns>An <see cref="AttributeResult"/> representing a successful validation.</returns>
        public static AttributeResult<R> Success<R>(R value) => new(true, value, string.Empty, null);

        /// <summary>
        /// Creates a failed validation result with the specified message template and arguments.
        /// </summary>
        /// <param name="template">The error message template.</param>
        /// <param name="args">Arguments for formatting the error message.</param>
        /// <returns>An <see cref="AttributeResult"/> representing a failed validation.</returns>
        public static AttributeResult<R> Fail<R>(string template, params object[] args)
            => new(false, default, template, args);

    }

    public readonly struct AttributeResult<T>
    {
        internal AttributeResult(bool isValid, T? output, string messageTemplate, object[]? args)
        {
            IsValid = isValid;
            MessageTemplate = messageTemplate;
            MessageArgs = args;
            Output = output;
        }
        public T? Output { get; }
        /// <summary>
        /// Gets a value indicating whether the validation was successful.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Gets the arguments used for formatting the validation message.
        /// </summary>
        /// <value>An array of objects that provide values for placeholders in the message template.</value>
        /// <remarks>
        /// These arguments are used with <see cref="MessageTemplate"/> to create formatted error messages.
        /// </remarks>
        public object[]? MessageArgs { get; }

        /// <summary>
        /// Gets the message template for the validation error, if validation failed.
        /// </summary>
        /// <value>The unformatted error message template, or <c>string.Empty</c> if validation succeeded.</value>
        public string MessageTemplate { get; }

        private static readonly object[] _empty = [];

        public static implicit operator AttributeResult(AttributeResult<T> result)
        {
            if (result.IsValid)
                return AttributeResult.Success();
            else
                return AttributeResult.Fail(result.MessageTemplate, result.MessageArgs ?? _empty);
        }


    }
}
