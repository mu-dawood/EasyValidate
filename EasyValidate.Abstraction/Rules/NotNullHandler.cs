using System.Collections.Generic;

namespace EasyValidate.Abstraction.Rules
{
    public class NotNullHandler : IValidationAttributeHandler
    {
        public IEnumerable<string> RequiredHelpers => System.Array.Empty<string>();
        public bool CanHandle(string attributeName)
            => attributeName == "NotNullAttribute";

        public string GenerateCheck(string propertyName, object[] constructorArgs)
            => $"            if ({propertyName} == null) errors.Add(\"{propertyName} must not be null.\");";
    }
}