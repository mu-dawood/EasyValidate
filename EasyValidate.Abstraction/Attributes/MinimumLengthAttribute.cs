using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MinimumLengthAttribute : Attribute
    {
        public int Min { get; }
        public MinimumLengthAttribute(int min) => Min = min;
    }
}
