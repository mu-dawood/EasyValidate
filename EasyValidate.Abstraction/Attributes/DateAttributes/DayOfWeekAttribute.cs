using System;

namespace EasyValidate.Abstraction.Attributes.DateAttributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DayOfWeekAttribute : ValidationAttributeBase
    {
        public DayOfWeek Day { get; }

        public DayOfWeekAttribute(DayOfWeek day)
        {
            Day = day;
        }

        public override string ErrorCode => "DayOfWeekValidationError";

        public AttributeResult Validate(string propertyName, DateTime value)
        {
            if (value.DayOfWeek != Day)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must fall on a {1}.",
                    MessageArgs = new object[] { propertyName, Day }
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
