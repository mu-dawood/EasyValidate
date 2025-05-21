using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class GreaterThanAttribute : Attribute
    {
        public double Value { get; }
        public GreaterThanAttribute(double value) => Value = value;
    }
}
