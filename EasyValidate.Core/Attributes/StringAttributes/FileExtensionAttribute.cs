using System;
using System.IO;
using System.Linq;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string has a specific file extension.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [FileExtension(".jpg", ".png", ".gif")]
    ///     public string ImagePath { get; set; } // Valid: "photo.jpg", "image.png", Invalid: "document.pdf", "file.txt"
    ///     
    ///     [FileExtension(".pdf", ".doc", ".docx")]
    ///     public string DocumentPath { get; set; } // Valid: "report.pdf", "letter.doc", Invalid: "image.jpg"
    /// }
    /// </code>
    /// </example>
    public class FileExtensionAttribute(params string[] allowedExtensions) : StringValidationAttributeBase
    {
        public string[] AllowedExtensions { get; } = allowedExtensions;

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "FileExtensionValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, string value)
        {
            var extension = Path.GetExtension(value);
            bool valid = AllowedExtensions.Contains(extension, StringComparer.OrdinalIgnoreCase);
            return valid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must have one of the following extensions: {1}.", propertyName, string.Join(", ", AllowedExtensions));
        }
    }
}
