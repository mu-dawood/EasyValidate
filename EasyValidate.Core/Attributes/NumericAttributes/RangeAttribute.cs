using System;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    public enum RangeBoundary
    {
        Inclusive,
        Exclusive,
        InclusiveMinExclusiveMax,
        ExclusiveMinInclusiveMax
    }

    /// <summary>
    /// Validates that a numeric value is within a specified range, with configurable inclusivity/exclusivity for each bound.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [Range(0, 100)]
    ///     public int Percentage { get; set; } // Valid: 0, 50, 100, Invalid: -1, 101
    ///     
    ///     [Range(18, 65, RangeBoundary.Inclusive)]
    ///     public int Age { get; set; } // Valid: 18, 30, 65, Invalid: 17, 66
    ///     
    ///     [Range(0.0, 1.0, RangeBoundary.Exclusive)]
    ///     public double Probability { get; set; } // Valid: 0.5, 0.8, Invalid: 0.0, 1.0
    /// }
    /// </code>
    /// </example>
    public class RangeAttribute : NumericValidationAttributeBase,
        IValidationAttribute<byte>,
        IValidationAttribute<sbyte>,
        IValidationAttribute<short>,
        IValidationAttribute<ushort>,
        IValidationAttribute<int>,
        IValidationAttribute<uint>,
        IValidationAttribute<long>,
        IValidationAttribute<ulong>,
        IValidationAttribute<float>,
        IValidationAttribute<double>,
        IValidationAttribute<decimal>
    {
        private readonly NumericValue _min;
        private readonly NumericValue _max;
        public RangeBoundary Boundary { get; }

        public RangeAttribute(double minimum, double maximum, RangeBoundary boundary = RangeBoundary.Inclusive)
        {
            _min = minimum;
            _max = maximum;
            Boundary = boundary;
        }
        public RangeAttribute(decimal minimum, decimal maximum, RangeBoundary boundary = RangeBoundary.Inclusive)
        {
            _min = minimum;
            _max = maximum;
            Boundary = boundary;
        }
        public RangeAttribute(int minimum, int maximum, RangeBoundary boundary = RangeBoundary.Inclusive)
        {
            _min = minimum;
            _max = maximum;
            Boundary = boundary;
        }
        public RangeAttribute(long minimum, long maximum, RangeBoundary boundary = RangeBoundary.Inclusive)
        {
            _min = minimum;
            _max = maximum;
            Boundary = boundary;
        }
        public RangeAttribute(float minimum, float maximum, RangeBoundary boundary = RangeBoundary.Inclusive)
        {
            _min = minimum;
            _max = maximum;
            Boundary = boundary;
        }
        public RangeAttribute(short minimum, short maximum, RangeBoundary boundary = RangeBoundary.Inclusive)
        {
            _min = minimum;
            _max = maximum;
            Boundary = boundary;
        }
        public RangeAttribute(ushort minimum, ushort maximum, RangeBoundary boundary = RangeBoundary.Inclusive)
        {
            _min = minimum;
            _max = maximum;
            Boundary = boundary;
        }
        public RangeAttribute(uint minimum, uint maximum, RangeBoundary boundary = RangeBoundary.Inclusive)
        {
            _min = minimum;
            _max = maximum;
            Boundary = boundary;
        }
        public RangeAttribute(ulong minimum, ulong maximum, RangeBoundary boundary = RangeBoundary.Inclusive)
        {
            _min = minimum;
            _max = maximum;
            Boundary = boundary;
        }

        public override string ErrorCode { get; set; } = "RangeValidationError";
        public string ErrorMessage { get; set; } = "The {0} field must be within the specified range: {1} to {2}.";

        // Type-specific range checks
        private bool IsValid(long value) => IsValidRange(value, _min.AsInt64, _max.AsInt64);
        private bool IsValid(ulong value) => IsValidRange(value, _min.AsUInt64, _max.AsUInt64);
        private bool IsValid(int value) => IsValidRange(value, (int)_min.AsInt64, (int)_max.AsInt64);
        private bool IsValid(uint value) => IsValidRange(value, (uint)_min.AsUInt64, (uint)_max.AsUInt64);
        private bool IsValid(short value) => IsValidRange(value, (short)_min.AsInt64, (short)_max.AsInt64);
        private bool IsValid(ushort value) => IsValidRange(value, (ushort)_min.AsUInt64, (ushort)_max.AsUInt64);
        private bool IsValid(byte value) => IsValidRange(value, (byte)_min.AsUInt64, (byte)_max.AsUInt64);
        private bool IsValid(sbyte value) => IsValidRange(value, (sbyte)_min.AsInt64, (sbyte)_max.AsInt64);
        private bool IsValid(double value) => IsValidRange(value, _min.AsDouble, _max.AsDouble);
        private bool IsValid(float value) => IsValidRange(value, _min.AsFloat, _max.AsFloat);
        private bool IsValid(decimal value) => IsValidRange(value, _min.AsDecimal, _max.AsDecimal);

        // IValidationAttribute implementations
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, byte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _min, _max);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, sbyte value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _min, _max);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, short value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _min, _max);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, ushort value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _min, _max);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, int value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _min, _max);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, uint value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _min, _max);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, long value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _min, _max);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, ulong value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _min, _max);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, float value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _min, _max);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, double value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _min, _max);
        }
        public AttributeResult Validate(IServiceProvider serviceProvider, string propertyName, decimal value)
        {
            return IsValid(value) ? AttributeResult.Success() : AttributeResult.Fail(ErrorMessage, propertyName, _min, _max);
        }

        // Type-specific range logic
        private bool IsValidRange(long value, long min, long max)
        {
            return Boundary switch
            {
                RangeBoundary.Inclusive => value >= min && value <= max,
                RangeBoundary.Exclusive => value > min && value < max,
                RangeBoundary.InclusiveMinExclusiveMax => value >= min && value < max,
                RangeBoundary.ExclusiveMinInclusiveMax => value > min && value <= max,
                _ => value >= min && value <= max
            };
        }
        private bool IsValidRange(ulong value, ulong min, ulong max)
        {
            return Boundary switch
            {
                RangeBoundary.Inclusive => value >= min && value <= max,
                RangeBoundary.Exclusive => value > min && value < max,
                RangeBoundary.InclusiveMinExclusiveMax => value >= min && value < max,
                RangeBoundary.ExclusiveMinInclusiveMax => value > min && value <= max,
                _ => value >= min && value <= max
            };
        }
        private bool IsValidRange(int value, int min, int max)
        {
            return Boundary switch
            {
                RangeBoundary.Inclusive => value >= min && value <= max,
                RangeBoundary.Exclusive => value > min && value < max,
                RangeBoundary.InclusiveMinExclusiveMax => value >= min && value < max,
                RangeBoundary.ExclusiveMinInclusiveMax => value > min && value <= max,
                _ => value >= min && value <= max
            };
        }
        private bool IsValidRange(uint value, uint min, uint max)
        {
            return Boundary switch
            {
                RangeBoundary.Inclusive => value >= min && value <= max,
                RangeBoundary.Exclusive => value > min && value < max,
                RangeBoundary.InclusiveMinExclusiveMax => value >= min && value < max,
                RangeBoundary.ExclusiveMinInclusiveMax => value > min && value <= max,
                _ => value >= min && value <= max
            };
        }
        private bool IsValidRange(short value, short min, short max)
        {
            return Boundary switch
            {
                RangeBoundary.Inclusive => value >= min && value <= max,
                RangeBoundary.Exclusive => value > min && value < max,
                RangeBoundary.InclusiveMinExclusiveMax => value >= min && value < max,
                RangeBoundary.ExclusiveMinInclusiveMax => value > min && value <= max,
                _ => value >= min && value <= max
            };
        }
        private bool IsValidRange(ushort value, ushort min, ushort max)
        {
            return Boundary switch
            {
                RangeBoundary.Inclusive => value >= min && value <= max,
                RangeBoundary.Exclusive => value > min && value < max,
                RangeBoundary.InclusiveMinExclusiveMax => value >= min && value < max,
                RangeBoundary.ExclusiveMinInclusiveMax => value > min && value <= max,
                _ => value >= min && value <= max
            };
        }
        private bool IsValidRange(byte value, byte min, byte max)
        {
            return Boundary switch
            {
                RangeBoundary.Inclusive => value >= min && value <= max,
                RangeBoundary.Exclusive => value > min && value < max,
                RangeBoundary.InclusiveMinExclusiveMax => value >= min && value < max,
                RangeBoundary.ExclusiveMinInclusiveMax => value > min && value <= max,
                _ => value >= min && value <= max
            };
        }
        private bool IsValidRange(sbyte value, sbyte min, sbyte max)
        {
            return Boundary switch
            {
                RangeBoundary.Inclusive => value >= min && value <= max,
                RangeBoundary.Exclusive => value > min && value < max,
                RangeBoundary.InclusiveMinExclusiveMax => value >= min && value < max,
                RangeBoundary.ExclusiveMinInclusiveMax => value > min && value <= max,
                _ => value >= min && value <= max
            };
        }
        private bool IsValidRange(double value, double min, double max)
        {
            return Boundary switch
            {
                RangeBoundary.Inclusive => value >= min && value <= max,
                RangeBoundary.Exclusive => value > min && value < max,
                RangeBoundary.InclusiveMinExclusiveMax => value >= min && value < max,
                RangeBoundary.ExclusiveMinInclusiveMax => value > min && value <= max,
                _ => value >= min && value <= max
            };
        }
        private bool IsValidRange(float value, float min, float max)
        {
            return Boundary switch
            {
                RangeBoundary.Inclusive => value >= min && value <= max,
                RangeBoundary.Exclusive => value > min && value < max,
                RangeBoundary.InclusiveMinExclusiveMax => value >= min && value < max,
                RangeBoundary.ExclusiveMinInclusiveMax => value > min && value <= max,
                _ => value >= min && value <= max
            };
        }
        private bool IsValidRange(decimal value, decimal min, decimal max)
        {
            return Boundary switch
            {
                RangeBoundary.Inclusive => value >= min && value <= max,
                RangeBoundary.Exclusive => value > min && value < max,
                RangeBoundary.InclusiveMinExclusiveMax => value >= min && value < max,
                RangeBoundary.ExclusiveMinInclusiveMax => value > min && value <= max,
                _ => value >= min && value <= max
            };
        }
    }
}