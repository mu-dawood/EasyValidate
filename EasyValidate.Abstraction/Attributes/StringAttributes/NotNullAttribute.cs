using System;

namespace EasyValidate.Abstraction.Attributes.StringAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class NotNullAttribute : ValidationAttributeBase
    {
        public override string ErrorCode => "NOT_NULL";
    }
}