using System.Collections.Generic;

namespace EasyValidate.Abstraction.Rules
{
    public class InclusiveBetweenHandler : IValidationAttributeHandler
    {
        public IEnumerable<string> RequiredHelpers => System.Array.Empty<string>();
        public bool CanHandle(string attributeName)
            => attributeName == "InclusiveBetweenAttribute";

        public string GenerateCheck(string propertyName, object[] constructorArgs)
        {
            var min = constructorArgs[0];
            var max = constructorArgs[1];
            return $"            if ({propertyName} < {min} || {propertyName} > {max}) errors.Add(\"{propertyName} must be between {min} and {max}.\");";
        }
    }
}