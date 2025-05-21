using System.Collections.Generic;

namespace EasyValidate.Abstraction.Rules
{
    public class NotEmptyHandler : IValidationAttributeHandler
    {
        public IEnumerable<string> RequiredHelpers => System.Array.Empty<string>();
        public bool CanHandle(string attributeName)
            => attributeName == "NotEmptyAttribute";

        public string GenerateCheck(string propertyName, object[] constructorArgs)
            => $"            if (string.IsNullOrWhiteSpace({propertyName})) errors.Add(\"{propertyName} must not be empty.\");";
    }
}