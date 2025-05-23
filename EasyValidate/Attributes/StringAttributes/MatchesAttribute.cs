using System;
using System.Text.RegularExpressions;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string matches the specified regular expression pattern.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MatchesAttribute(string pattern) : StringValidationAttributeBase
    {
        /// <summary>
        /// The regular expression pattern to match.
        /// </summary>
        public string Pattern { get; } = pattern;

        public override string ErrorCode => "MatchesValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (string.IsNullOrWhiteSpace(value) || !Regex.IsMatch(value, Pattern))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must match the pattern {1}.",
                    MessageArgs = new object?[] { propertyName, Pattern }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}