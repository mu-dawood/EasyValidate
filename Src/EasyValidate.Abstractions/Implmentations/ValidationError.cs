namespace EasyValidate.Abstractions
{
    /// <summary>
    /// Represents a validation error with detailed information.
    /// </summary>
    /// <docs-display-name>Validation Error</docs-display-name>
    /// <docs-icon>AlertCircle</docs-icon>
    /// <docs-description>Detailed validation error information including error codes, messages, and attribute context for comprehensive error reporting.</docs-description>
    public sealed class ValidationError
    {
        internal ValidationError(string errorCode, string attributeName, string formattedMessage, string propertyName, string chainName)
        {
            ErrorCode = errorCode;
            AttributeName = attributeName;
            FormattedMessage = formattedMessage;
            PropertyName = propertyName;
            ChainName = chainName;
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

        /// <summary>
        /// Gets the name of the property associated with this validation error.
        /// </summary>
        /// <value>A string representing the property name for which the validation error was generated.</value>
        /// <remarks>
        /// Useful for identifying which property failed validation, especially in complex or nested objects.
        /// </remarks>
        /// <example>
        /// <code>
        /// if (error.PropertyName == "Email") {
        ///     // Handle email property error
        /// }
        /// </code>
        /// </example>
        /// <docs-member>PropertyName</docs-member>
        /// <docs-type>Property</docs-type>
        /// <docs-return-type>string</docs-return-type>
        public string PropertyName { get; }

        /// <summary>
        /// Gets the name of the validation chain associated with this error, if any.
        /// </summary>
        /// <value>A string representing the chain name for which the validation error was generated.</value>
        /// <remarks>
        /// Useful for distinguishing errors that occur in different validation chains or strategies.
        /// </remarks>
        /// <example>
        /// <code>
        /// if (error.ChainName == "primary") {
        ///     // Handle errors from the primary validation chain
        /// }
        /// </code>
        /// </example>
        /// <docs-member>ChainName</docs-member>
        /// <docs-type>Property</docs-type>
        /// <docs-return-type>string</docs-return-type>
        public string ChainName { get; }

        internal ValidationError WithPropertyName(string propertyName)
        {
            return new ValidationError(ErrorCode, AttributeName, FormattedMessage, propertyName, ChainName);
        }
    }
}