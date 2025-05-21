using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class InclusiveBetweenAttribute : Attribute
    {
        public double Min { get; }
        public double Max { get; }

        public InclusiveBetweenAttribute(double min, double max)
        {
            Min = min;
            Max = max;
        }
    }
}
