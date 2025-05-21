using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class LessThanOrEqualToAttribute : Attribute
    {
        public double Value { get; }
        public LessThanOrEqualToAttribute(double value) => Value = value;
    }
}
