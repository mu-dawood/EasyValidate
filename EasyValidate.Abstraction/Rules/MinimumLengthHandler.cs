using System.Collections.Generic;

namespace EasyValidate.Abstraction.Rules
{
    public class MinimumLengthHandler : IValidationAttributeHandler
    {
        public IEnumerable<string> RequiredHelpers => System.Array.Empty<string>();
        public bool CanHandle(string attributeName)
            => attributeName == "MinimumLengthAttribute";

        public string GenerateCheck(string propertyName, object[] constructorArgs)
        {
            var min = constructorArgs[0];
            return $"            if ({propertyName} == null || {propertyName}.Length < {min}) errors.Add(\"{propertyName} minimum length is {min}.\");";
        }
    }
}