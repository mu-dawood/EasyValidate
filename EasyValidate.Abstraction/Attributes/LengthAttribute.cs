using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class LengthAttribute : Attribute
    {
        public int Min { get; }
        public int Max { get; }

        public LengthAttribute(int min, int max)
        {
            Min = min;
            Max = max;
        }
    }
}
