using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace EasyValidate.Abstraction.Rules
{
    public class EmailAddressHandler : IValidationAttributeHandler
    {
        public IEnumerable<string> RequiredHelpers => new[] {
            @"        private static bool IsValidEmail(string? v)
        {
            return !string.IsNullOrWhiteSpace(v)
                   && Regex.IsMatch(v, @""^[^@\\s]+@[^@\\s]+\\.[^@\\s]+$"");
        }"
        };
        public bool CanHandle(string attributeName)
            => attributeName == "EmailAddressAttribute";

        public string GenerateCheck(string propertyName, object[] constructorArgs)
            => $"            if (!IsValidEmail({propertyName})) errors.Add(\"{propertyName} must be a valid email address.\");";
    }
}