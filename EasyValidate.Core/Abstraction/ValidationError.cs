namespace EasyValidate.Core.Abstraction
{
    /// <summary>
    /// Represents a validation error with detailed information.
    /// </summary>
    /// <docs-display-name>Validation Error</docs-display-name>
    /// <docs-icon>AlertCircle</docs-icon>
    /// <docs-description>Detailed validation error information including error codes, messages, and attribute context for comprehensive error reporting.</docs-description>
    public sealed class ValidationError
    {
        internal ValidationError(string errorCode, string message, object?[] args, string attributeName, string formattedMessage, string[] path, string chain)
        {
            ErrorCode = errorCode;
            Message = message;
            Args = args;
            AttributeName = attributeName;
            FormattedMessage = formattedMessage;
            Path = path;
            Chain = chain;
        }
        /// <summary>
        /// Gets or sets the error code associated with this validation error.
        /// </summary>
        /// <value>A string representing the unique error code for this validation failure.</value>
        /// <remarks>
        /// Error codes provide a way to programmatically identify specific types of validation failures.
        /// They can be used for internationalization, custom error handling, or logging purposes.
        /// </remarks>
        /// <example>
        /// <code>
        /// if (error.ErrorCode == "INVALID_EMAIL") {
        ///     // Handle email validation error specifically
        /// }
        /// </code>
        /// </example>
        /// <docs-member>ErrorCode</docs-member>
        /// <docs-type>Property</docs-type>
        /// <docs-return-type>string</docs-return-type>
        public string ErrorCode { get; }



        /// <summary>
        /// Gets or sets the validation error message template.
        /// </summary>
        /// <value>A string containing the message template with placeholders for formatting arguments.</value>
        /// <remarks>
        /// This is the raw message template before argument substitution. Use FormattedMessage
        /// to get the final formatted message ready for display.
        /// </remarks>
        /// <example>
        /// <code>
        /// error.Message = "The {0} field is required.";
        /// error.Args = new object[] { "Email" };
        /// // FormattedMessage will be: "The Email field is required."
        /// </code>
        /// </example>
        /// <docs-member>Message</docs-member>
        /// <docs-type>Property</docs-type>
        /// <docs-return-type>string</docs-return-type>
        public string Message { get; }

        /// <summary>
        /// Gets or sets the arguments used for formatting the validation message.
        /// </summary>
        /// <value>An array of objects that provide values for message template placeholders.</value>
        /// <remarks>
        /// These arguments are used in conjunction with the Message property to create the FormattedMessage.
        /// The arguments correspond to placeholders in the message template (e.g., {0}, {1}).
        /// </remarks>
        /// <example>
        /// <code>
        /// error.Args = new object[] { "Email", 5, 100 };
        /// // Used with message: "The {0} field must be between {1} and {2} characters."
        /// </code>
        /// </example>
        /// <docs-member>Args</docs-member>
        /// <docs-type>Property</docs-type>
        /// <docs-return-type>object?[]</docs-return-type>
        public object?[] Args { get; }

        /// <summary>
        /// Gets or sets the name of the attribute that generated this validation error.
        /// </summary>
        /// <value>A string representing the name of the validation attribute class.</value>
        /// <remarks>
        /// This property helps identify which validation attribute caused the error, useful for
        /// debugging, logging, or implementing custom error handling logic.
        /// </remarks>
        /// <example>
        /// <code>
        /// if (error.AttributeName == "RequiredAttribute") {
        ///     // Handle required field validation error
        /// }
        /// </code>
        /// </example>
        /// <docs-member>AttributeName</docs-member>
        /// <docs-type>Property</docs-type>
        /// <docs-return-type>string</docs-return-type>
        public string AttributeName { get; }

        /// <summary>
        /// Gets or sets the fully formatted validation error message.
        /// </summary>
        /// <value>A string containing the final formatted error message ready for display.</value>
        /// <remarks>This property is set internally during the validation process.</remarks>
        /// <example>
        /// <code>
        /// Console.WriteLine($"Error: {error.FormattedMessage}");
        /// // Output: Error: The Email field is not a valid email address.
        /// </code>
        /// </example>
        /// <docs-member>FormattedMessage</docs-member>
        /// <docs-type>Property</docs-type>
        /// <docs-return-type>string</docs-return-type>
        public string FormattedMessage { get; }

        public string[] Path { get; } = [];
        public string Chain { get; }

        internal ValidationError WithPath(string[] path)
        {
            return new ValidationError(ErrorCode, Message, Args, AttributeName, FormattedMessage, path, Chain);
        }
    }
}