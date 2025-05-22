using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DateRangeAttribute : ValidationAttributeBase
    {
        public DateTime Minimum { get; }
        public DateTime Maximum { get; }

        public DateRangeAttribute(string minimum, string maximum)
        {
            Minimum = DateTime.Parse(minimum);
            Maximum = DateTime.Parse(maximum);
        }

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
