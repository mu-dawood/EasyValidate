using System.Collections.Generic;

namespace EasyValidate.Abstraction.Rules
{
    public class GreaterThanOrEqualToHandler : IValidationAttributeHandler
    {
        public IEnumerable<string> RequiredHelpers => System.Array.Empty<string>();
        public bool CanHandle(string attributeName)
            => attributeName == "GreaterThanOrEqualToAttribute";

        public string GenerateCheck(string propertyName, object[] constructorArgs)
        {
            var val = constructorArgs[0];
            return $"            if ({propertyName} < {val}) errors.Add(\"{propertyName} must be >= {val}.\");";
        }
    }
}