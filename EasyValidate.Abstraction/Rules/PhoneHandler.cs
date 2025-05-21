using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EasyValidate.Abstraction.Rules
{
    public class PhoneHandler : IValidationAttributeHandler
    {
        public bool CanHandle(string attributeName)
            => attributeName == "PhoneAttribute";

        public string GenerateCheck(string propertyName, object[] constructorArgs)
            => $"            if (!IsValidPhone({propertyName})) errors.Add(\"{propertyName} must be a valid phone number.\");";

        // helper method required by this handler
        public IEnumerable<string> RequiredHelpers => new[] {
            @"        private static bool IsValidPhone(string? v)
        {
            return !string.IsNullOrWhiteSpace(v)
                   && Regex.IsMatch(v, @""^\+?[1-9]\d{1,14}$"");
        }"
        };
    }
}