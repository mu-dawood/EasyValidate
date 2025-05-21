using System.Collections.Generic;

namespace EasyValidate.Abstraction.Rules
{
    public class RequiredHandler : IValidationAttributeHandler
    {
        public IEnumerable<string> RequiredHelpers => System.Array.Empty<string>();

        public bool CanHandle(string attributeName)
            => attributeName == "RequiredAttribute";

        public string GenerateCheck(string propertyName, object[] constructorArgs)
            => $"            if ({propertyName} == null) errors.Add(\"{propertyName} is required.\");";
    }
}
