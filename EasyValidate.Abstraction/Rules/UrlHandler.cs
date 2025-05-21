using System.Collections.Generic;

namespace EasyValidate.Abstraction.Rules
{
    public class UrlHandler : IValidationAttributeHandler
    {
        public IEnumerable<string> RequiredHelpers => System.Array.Empty<string>();
        public bool CanHandle(string attributeName)
            => attributeName == "UrlAttribute";

        public string GenerateCheck(string propertyName, object[] constructorArgs)
            => $"            if (!Uri.IsWellFormedUriString({propertyName}, UriKind.RelativeOrAbsolute)) errors.Add(\"{propertyName} must be a valid URL.\");";
    }
}