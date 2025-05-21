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
        /// Emits the indented validation check line for the property.
        /// </summary>
        string GenerateCheck(string propertyName, object[] constructorArgs);

        /// <summary>
        /// Code snippets of helper methods required by this handler.
        /// </summary>
        IEnumerable<string> RequiredHelpers { get; }
    }
}