using System.Collections.Generic;

namespace EasyValidate.Abstraction.Rules
{
    public class LengthHandler : IValidationAttributeHandler
    {
        public IEnumerable<string> RequiredHelpers => System.Array.Empty<string>();
        public bool CanHandle(string attributeName)
            => attributeName == "LengthAttribute";

        public string GenerateCheck(string propertyName, object[] constructorArgs)
        {
            var min = constructorArgs[0];
            var max = constructorArgs[1];
            return $"            if ({propertyName} == null || {propertyName}.Length < {min} || {propertyName}.Length > {max}) errors.Add(\"{propertyName} length must be between {min} and {max}.\");";
        }
    }
}