using System;

namespace EasyValidate.Abstraction.Attributes
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class DayOfWeekAttribute(DayOfWeek day) : ValidationAttributeBase
    {
        public DayOfWeek Day { get; } = day;

        public override string ErrorCode => "DayOfWeekValidationError";

        public AttributeResult Validate(string propertyName, DateTime value)
        {
            if (value.DayOfWeek != Day)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must fall on a {1}.",
                    MessageArgs = [propertyName, Day]
                };
            }

            return new AttributeResult { IsValid = true };
        }
    }
}
