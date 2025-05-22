using System;
using System.Text.RegularExpressions;

namespace EasyValidate.Abstraction.Attributes.StringAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MatchesAttribute : ValidationAttributeBase
    {
        public string Pattern { get; }

        public MatchesAttribute(string pattern)
        {
            Pattern = pattern;
        }

        public override string ErrorCode => "MatchesValidationError";

        public AttributeResult Validate(string propertyName, string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, Pattern))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must match the pattern {1}.",
                    MessageArgs = new object[] { propertyName, Pattern }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}