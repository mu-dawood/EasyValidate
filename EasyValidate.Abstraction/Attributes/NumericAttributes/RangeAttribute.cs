using System;
using System.Collections.Generic;
using System.Linq;
using EasyValidate.Abstraction;

namespace EasyValidate.Abstraction.Attributes
{
    /// <summary>
    /// Specifies that a numeric value must fall within a specified range.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class RangeAttribute : ValidationAttributeBase
    {
        public double Minimum { get; }
        public double Maximum { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeAttribute"/> class.
        /// </summary>
        /// <param name="minimum">The minimum value of the range.</param>
        /// <param name="maximum">The maximum value of the range.</param>
        public RangeAttribute(double minimum, double maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeAttribute"/> class for integer values.
        /// </summary>
        /// <param name="minimum">The minimum value of the range.</param>
        /// <param name="maximum">The maximum value of the range.</param>
        public RangeAttribute(int minimum, int maximum)
            : this((double)minimum, (double)maximum)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeAttribute"/> class for float values.
        /// </summary>
        /// <param name="minimum">The minimum value of the range.</param>
        /// <param name="maximum">The maximum value of the range.</param>
        public RangeAttribute(float minimum, float maximum)
            : this((double)minimum, (double)maximum)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeAttribute"/> class for decimal values.
        /// </summary>
        /// <param name="minimum">The minimum value of the range.</param>
        /// <param name="maximum">The maximum value of the range.</param>
        public RangeAttribute(decimal minimum, decimal maximum)
            : this((double)minimum, (double)maximum)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RangeAttribute"/> class for long values.
        /// </summary>
        /// <param name="minimum">The minimum value of the range.</param>
        /// <param name="maximum">The maximum value of the range.</param>
        public RangeAttribute(long minimum, long maximum)
            : this((double)minimum, (double)maximum)
        {
        }

        /// <summary>
        /// Gets the error code for the RangeAttribute.
        /// </summary>
        public override string ErrorCode => "RangeValidationError";

        private AttributeResult ValidateInternal<T>(string propertyName, T value) where T : IComparable
        {
            double numericValue = Convert.ToDouble(value);

            if (numericValue < Minimum || numericValue > Maximum)
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be between {1} and {2}.",
                    MessageArgs = new object[] { propertyName, Minimum, Maximum }
                };
            }

            return new AttributeResult { IsValid = true };
        }

        public AttributeResult Validate<T>(string propertyName, T value) where T : IComparable
        {
            return ValidateInternal(propertyName, value);
        }
    }
}
