using System.Collections.Generic;

namespace EasyValidate.Abstraction.Rules
{
    public class MatchesHandler : IValidationAttributeHandler
    {
         public IEnumerable<string> RequiredHelpers => System.Array.Empty<string>();
        public bool CanHandle(string attributeName)
            => attributeName == "MatchesAttribute";

        public string GenerateCheck(string propertyName, object[] constructorArgs)
        {
            var pattern = constructorArgs[0];
            return $"            if (!Regex.IsMatch({propertyName} ?? string.Empty, @\"{pattern}\")) errors.Add(\"{propertyName} must match /{pattern}/.\");";
        }
    }
}