using System;
using EasyValidate.Abstraction.Rules;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MatchesAttribute : ValidationAttributeBase
    {
        public string Pattern { get; }

        public MatchesAttribute(string pattern) => Pattern = pattern;

        public override IValidationAttributeHandler Handler => new MatchesHandler();

        public override bool IsCompatibleType(Type clrType) => clrType == typeof(string);
    }
}
