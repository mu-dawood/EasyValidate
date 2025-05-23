using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date falls on a weekday (Monday through Friday).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class WeekdayAttribute : DateValidationAttributeBase
    {
        public override string ErrorCode => "WeekdayValidationError";

        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            var day = value.DayOfWeek;
            if (day == DayOfWeek.Saturday || day == DayOfWeek.Sunday)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must fall on a weekday (Monday through Friday).",
                    MessageArgs = [propertyName]
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
