using System.Collections.Generic;

namespace EasyValidate.Abstraction.Rules
{
    public class LessThanOrEqualToHandler : IValidationAttributeHandler
    {
        public IEnumerable<string> RequiredHelpers => System.Array.Empty<string>();
        public bool CanHandle(string attributeName)
            => attributeName == "LessThanOrEqualToAttribute";

        public string GenerateCheck(string propertyName, object[] constructorArgs)
        {
            var val = constructorArgs[0];
            return $"            if ({propertyName} > {val}) errors.Add(\"{propertyName} must be <= {val}.\");";
        }
    }
}