using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateRangeAttribute(string minimum, string maximum) : ValidationAttributeBase
    {
        public DateTime Minimum { get; } = DateTime.Parse(minimum);
        public DateTime Maximum { get; } = DateTime.Parse(maximum);

        public override string ErrorCode => "DateRangeValidationError";

        public AttributeResult Validate(string propertyName, DateTime value)
        {
            if (value < Minimum || value > Maximum)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be between {1} and {2}.",
                    MessageArgs = [propertyName, Minimum, Maximum]
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
