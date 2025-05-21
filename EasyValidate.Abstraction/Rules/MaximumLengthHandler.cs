using System.Collections.Generic;

namespace EasyValidate.Abstraction.Rules
{
    public class MaximumLengthHandler : IValidationAttributeHandler
    {
        public IEnumerable<string> RequiredHelpers => System.Array.Empty<string>();
        public bool CanHandle(string attributeName)
            => attributeName == "MaximumLengthAttribute";

        public string GenerateCheck(string propertyName, object[] constructorArgs)
        {
            var max = constructorArgs[0];
            return $"            if ({propertyName} != null && {propertyName}.Length > {max}) errors.Add(\"{propertyName} maximum length is {max}.\");";
        }
    }
}