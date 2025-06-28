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
    public class UrlAttribute : StringValidationAttributeBase
    {
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsInvalid.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "UrlValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be a valid URL.";

        /// Arguments propertyName

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {
            bool isValid =  Uri.IsWellFormedUriString(value, UriKind.Absolute);
            return new AttributeResult<string>(isValid, value , propertyName);
        }
    }
}