using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class GreaterThanOrEqualToAttribute : Attribute
    {
        public double Value { get; }
        public GreaterThanOrEqualToAttribute(double value) => Value = value;
    }
}
