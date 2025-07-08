using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string is a valid URL.
    /// </summary>
    /// <example>
    /// <code>
    /// public class Website
    /// {
    ///     [Url]
    ///     public string Homepage { get; set; }
    /// }
    /// 
    /// var site = new Website { Homepage = "https://www.example.com" }; // Valid
    /// var invalidSite = new Website { Homepage = "not-a-url" }; // Invalid
    /// </code>
    /// </example>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]
    public class UrlAttribute : Attribute, IValidationAttribute<string, Uri>
    {
        public static readonly Lazy<UrlAttribute> Instance = new(() => new UrlAttribute());
        /// <summary>
        /// Gets or sets the name of the validation chain this attribute belongs to.
        /// </summary>
        public string Chain { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of a method that determines if this validation should be executed. The method must be parameterless and return bool. If null or empty, validation always executes.
        /// </summary>
        public string? ConditionalMethod { get; set; }

        /// <summary>
        /// Gets or sets the execution strategy for this validation attribute. Determines how this validation interacts with the validation chain.
        /// </summary>
        public ExecutionStrategy Strategy { get; set; } = ExecutionStrategy.ValidateAndStop;
        /// <inheritdoc/>
        public string ErrorCode { get; set; } = "UrlValidationError";

        /// <inheritdoc/>
        public AttributeResult Validate(object obj, string propertyName, string value, out Uri output)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                output = new Uri("about:blank");
                return AttributeResult.Fail("The {0} field must be a valid URL.", propertyName);
            }
            bool isValid = IsUrl(value!, out Uri? uriOutput);
            output = uriOutput ?? new Uri("about:blank");
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be a valid URL.", propertyName);
        }

        private static bool IsUrl(string value, out Uri? output)
        {
            return Uri.TryCreate(value, UriKind.Absolute, out output);
        }
    }
}