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
    public class UrlAttribute : StringValidationAttributeBase<Uri>
    {
        private static readonly Lazy<UrlAttribute> _instance = new(() => new UrlAttribute());
        public static UrlAttribute Instance => _instance.Value;
        /// <summary>
        /// Gets or sets the nullable behavior for this attribute. Defaults to NullIsInvalid.
        /// </summary>

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "UrlValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, string value, out Uri output)
        {
            if (string.IsNullOrEmpty(value))
            {
                output = new Uri("about:blank");
                return AttributeResult.Success();
            }
            bool isValid = IsUrl(value!, out Uri? uriOutput);
            output = uriOutput ?? new Uri(value!);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be a valid URL.", propertyName);
        }

        private static bool IsUrl(string value, out Uri? output)
        {
            return Uri.TryCreate(value, UriKind.Absolute, out output);
        }
    }
}