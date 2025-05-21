using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MaximumLengthAttribute : Attribute
    {
        public int Max { get; }
        public MaximumLengthAttribute(int max) => Max = max;
    }
}
