namespace EasyValidate.Abstraction
{
    /// <summary>
    /// Represents the result of an attribute validation operation.
    /// </summary>
    public sealed class AttributeResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether the validation was successful.
        /// </summary>
        public bool IsValid { get; set; }

        /// <summary>
        /// Gets or sets the validation message.
        /// </summary>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the arguments for the validation message.
        /// </summary>
        public object?[] MessageArgs { get; set; } = [];
    }
}
