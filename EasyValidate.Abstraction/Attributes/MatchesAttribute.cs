using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MatchesAttribute : Attribute
    {
        public string Pattern { get; }
        public MatchesAttribute(string pattern) => Pattern = pattern;
    }
}
