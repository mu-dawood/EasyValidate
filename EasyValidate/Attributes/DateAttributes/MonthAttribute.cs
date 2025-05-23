using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date falls in a specific month.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class MonthAttribute(int month) : DateValidationAttributeBase
    {
        /// <summary>
        /// The required month (1-12).
        /// </summary>
        public int Month { get; } = month;

        public override string ErrorCode => "MonthValidationError";

        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            if (value.Month != Month)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be in month {1}.",
                    MessageArgs = new object?[] { propertyName, Month }
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
