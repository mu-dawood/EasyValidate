using System.Collections.Generic;

namespace EasyValidate.Abstraction.Rules
{
    public class NotEqualToHandler : IValidationAttributeHandler
    {
         public IEnumerable<string> RequiredHelpers => System.Array.Empty<string>();
        public bool CanHandle(string attributeName)
            => attributeName == "NotEqualToAttribute";

        public string GenerateCheck(string propertyName, object[] constructorArgs)
        {
            var val = constructorArgs[0];
            var lit = val is string s ? $"@\"{s.Replace("\"","\"\"")}\"" : val?.ToString();
            return $"            if (object.Equals({propertyName}, {lit})) errors.Add(\"{propertyName} must not be equal to {val}.\");";
        }
    }
}