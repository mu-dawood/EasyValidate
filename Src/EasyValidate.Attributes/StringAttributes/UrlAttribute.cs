using System;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
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
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]
    public class UrlAttribute : Attribute, IValidationAttribute<string, Uri>
    {

        /// <summary>
        /// Gets or sets the name of the validation chain this attribute belongs to.
        /// </summary>
        public string Chain { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name of a method that determines if this validation should be executed. The method must be parameterless and return bool. If null or empty, validation always executes.
        /// </summary>
        public string? ConditionalMethod { get; set; }

        /// <inheritdoc/>
        public string ErrorCode { get; set; } = "UrlValidationError";


        /// <inheritdoc/>
        public AttributeResult<Uri> Validate(string propertyName, string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return AttributeResult.Fail<Uri>("The {0} field must be a valid URL.", propertyName);
            }
            bool isValid = IsUrl(value!, out Uri? uriOutput);
            return isValid ? AttributeResult.Success(uriOutput ?? new Uri("about:blank")) : AttributeResult.Fail<Uri>("The {0} field must be a valid URL.", propertyName);
        }

        private static bool IsUrl(string value, out Uri? output)
        {
            return Uri.TryCreate(value, UriKind.Absolute, out output);
        }
    }
}