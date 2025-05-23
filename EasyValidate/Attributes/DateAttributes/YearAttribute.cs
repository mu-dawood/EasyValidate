using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a date falls in a specific year.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class YearAttribute(int year) : DateValidationAttributeBase
    {
        /// <summary>
        /// The required year.
        /// </summary>
        public int Year { get; } = year;

        public override string ErrorCode => "YearValidationError";

        public override AttributeResult Validate(string propertyName, DateTime value)
        {
            if (value.Year != Year)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be in year {1}.",
                    MessageArgs = [propertyName, Year]
                };
            }
            return new AttributeResult { IsValid = true };
        }
    }
}
