using System;
using System.Text.RegularExpressions;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MatchesAttribute(string pattern) : ValidationAttributeBase
    {
        public string Pattern { get; } = pattern;

        public override string ErrorCode => "MatchesValidationError";

        public AttributeResult Validate(string propertyName, string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, Pattern))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must match the pattern {1}.",
                    MessageArgs = [propertyName, Pattern]
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}