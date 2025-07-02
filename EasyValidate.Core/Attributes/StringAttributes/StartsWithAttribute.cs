using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string starts with the specified prefix.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [StartsWith("https://")]
    ///     public string WebsiteUrl { get; set; } // Valid: "https://example.com", Invalid: "http://example.com"
    ///     
    ///     [StartsWith("Mr.")]
    ///     public string Title { get; set; } // Valid: "Mr. John Smith", Invalid: "John Smith"
    /// }
    /// </code>
    /// </example>
    public class StartsWithAttribute(string prefix) : StringValidationAttributeBase
    {
        /// <summary>
        /// The required prefix.
        /// </summary>
        public string Prefix { get; } = prefix;

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "StartsWithValidationError";

        /// <summary>
        /// The comparison type to use when checking for the substring.
        /// Defaults to <see cref="StringComparison.Ordinal"/>.
        /// </summary>
        public StringComparison Comparison { get; set; } = StringComparison.Ordinal;
        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, string value, out string output)
        {
            bool isValid = value.StartsWith(Prefix, Comparison);
            output = value;
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must start with '{1}'.", propertyName, Prefix);
        }
    }
}
