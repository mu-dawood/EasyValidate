using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class LessThanAttribute : Attribute
    {
        public double Value { get; }
        public LessThanAttribute(double value) => Value = value;
    }
}
