using System.Collections.Generic;

namespace EasyValidate.Abstraction.Rules
{
    public class CreditCardHandler : IValidationAttributeHandler
    {
        public bool CanHandle(string attributeName)
            => attributeName == "CreditCardAttribute";

        public string GenerateCheck(string propertyName, object[] constructorArgs)
            => $"            if (!IsValidCreditCard({propertyName})) errors.Add(\"{propertyName} must be a valid credit card number.\");";

        // helper method required by this handler
        public IEnumerable<string> RequiredHelpers => new[] {
            @"        private static bool IsValidCreditCard(string? n)
        {
            if (string.IsNullOrWhiteSpace(n)) return false;
            n = Regex.Replace(n, ""[^0-9]"", """");
            int sum = 0, alt = 0;
            for (int i = n.Length - 1; i >= 0; i--)
            {
                int d = n[i] - '0';
                if (alt++ % 2 == 1)
                {
                    d *= 2;
                    if (d > 9) d -= 9;
                }
                sum += d;
            }
            return sum % 10 == 0;
        }"
        };
    }
}