using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a numeric value is a prime number (for integer values only).
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class PrimeAttribute : NumericValidationAttributeBase
    {
        public override string ErrorCode => "PrimeValidationError";

        /// <inheritdoc/>
        public override AttributeResult ValidateNumber(string propertyName, decimal value)
        {
            if (value != Math.Floor(value) || value < 2 || !IsPrime((long)value))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be a prime number.",
                    MessageArgs = [propertyName]
                };
            }
            return new AttributeResult { IsValid = true };
        }

        private static bool IsPrime(long n)
        {
            if (n < 2) return false;
            if (n == 2) return true;
            if (n % 2 == 0) return false;
            var limit = (long)Math.Sqrt(n);
            for (long i = 3; i <= limit; i += 2)
            {
                if (n % i == 0) return false;
            }
            return true;
        }
    }
}
