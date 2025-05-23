using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date is today (ignoring time).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class TodayAttribute : DateValidationAttributeBase
    {
        public override string ErrorCode => "TodayValidationError";

        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            var today = DateTime.Today;
            if (value.Date != today)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be today ({1:yyyy-MM-dd}).",
                    MessageArgs = [propertyName, today]
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
