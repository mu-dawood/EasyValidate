using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public abstract class NumericAttribute : ValidationAttributeBase
    {
        public AttributeResult Validate(string propertyName, string value)
        {
            if (string.IsNullOrWhiteSpace(value) || !double.TryParse(value, out _))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be a valid numeric value.",
                    MessageArgs = [propertyName]
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}