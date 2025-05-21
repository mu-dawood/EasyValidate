using System.Collections.Generic;

namespace EasyValidate.Abstraction.Rules
{
    /// <summary>
    /// Generates one line of validation for a property,
    /// based purely on attribute name and ctor‐args.
    /// </summary>
    public interface IValidationAttributeHandler
    {
        /// <summary>
        /// True if this handler wants to handle an attribute
        /// whose class is called <c>attributeName</c>.
        /// </summary>
        bool CanHandle(string attributeName);

        /// <summary>
        /// Emits the indented line (with semicolon) to append to Validate().
        /// </summary>
        string GenerateCheck(string propertyName, object[] constructorArgs);

        /// <summary>
        /// Code snippets for helper methods required by this handler.
        /// </summary>
        IEnumerable<string> RequiredHelpers { get; }
    }
}