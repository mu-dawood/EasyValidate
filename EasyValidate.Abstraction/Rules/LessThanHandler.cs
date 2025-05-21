using System.Collections.Generic;

namespace EasyValidate.Abstraction.Rules
{
    public class LessThanHandler : IValidationAttributeHandler
    {
        public IEnumerable<string> RequiredHelpers => System.Array.Empty<string>();
        public bool CanHandle(string attributeName)
            => attributeName == "LessThanAttribute";

        public string GenerateCheck(string propertyName, object[] constructorArgs)
        {
            var val = constructorArgs[0];
            return $"            if ({propertyName} >= {val}) errors.Add(\"{propertyName} must be < {val}.\");";
        }
    }
}