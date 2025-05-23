using System;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Base attribute for numeric validation attributes. Provides a contract for validating numeric properties.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public abstract class NumericValidationAttributeBase : ValidationAttributeBase
    {
        /// <summary>
        /// Validates the numeric property value (nullable byte).
        /// </summary>
        public virtual AttributeResult Validate(string propertyName, byte? value)
        {
            if (value == null)
                return new AttributeResult { IsValid = false, Message = "The field {0} cannot be null.", MessageArgs = [propertyName] };
            return ValidateNumber(propertyName, value.Value);
        }

        /// <summary>
        /// Validates the numeric property value (nullable sbyte).
        /// </summary>
        public virtual AttributeResult Validate(string propertyName, sbyte? value)
        {
            if (value == null)
                return new AttributeResult { IsValid = false, Message = "The field {0} cannot be null.", MessageArgs = [propertyName] };
            return ValidateNumber(propertyName, value.Value);
        }

        /// <summary>
        /// Validates the numeric property value (nullable short).
        /// </summary>
        public virtual AttributeResult Validate(string propertyName, short? value)
        {
            if (value == null)
                return new AttributeResult { IsValid = false, Message = "The field {0} cannot be null.", MessageArgs = [propertyName] };
            return ValidateNumber(propertyName, value.Value);
        }

        /// <summary>
        /// Validates the numeric property value (nullable ushort).
        /// </summary>
        public virtual AttributeResult Validate(string propertyName, ushort? value)
        {
            if (value == null)
                return new AttributeResult { IsValid = false, Message = "The field {0} cannot be null.", MessageArgs = [propertyName] };
            return ValidateNumber(propertyName, value.Value);
        }

        /// <summary>
        /// Validates the numeric property value (nullable int).
        /// </summary>
        public virtual AttributeResult Validate(string propertyName, int? value)
        {
            if (value == null)
                return new AttributeResult { IsValid = false, Message = "The field {0} cannot be null.", MessageArgs = [propertyName] };
            return ValidateNumber(propertyName, value.Value);
        }

        /// <summary>
        /// Validates the numeric property value (nullable uint).
        /// </summary>
        public virtual AttributeResult Validate(string propertyName, uint? value)
        {
            if (value == null)
                return new AttributeResult { IsValid = false, Message = "The field {0} cannot be null.", MessageArgs = [propertyName] };
            return ValidateNumber(propertyName, value.Value);
        }

        /// <summary>
        /// Validates the numeric property value (nullable long).
        /// </summary>
        public virtual AttributeResult Validate(string propertyName, long? value)
        {
            if (value == null)
                return new AttributeResult { IsValid = false, Message = "The field {0} cannot be null.", MessageArgs = [propertyName] };
            return ValidateNumber(propertyName, value.Value);
        }

        /// <summary>
        /// Validates the numeric property value (nullable ulong).
        /// </summary>
        public virtual AttributeResult Validate(string propertyName, ulong? value)
        {
            if (value == null)
                return new AttributeResult { IsValid = false, Message = "The field {0} cannot be null.", MessageArgs = [propertyName] };
            return ValidateNumber(propertyName, (decimal)value.Value);
        }

        /// <summary>
        /// Validates the numeric property value (nullable float).
        /// </summary>
        public virtual AttributeResult Validate(string propertyName, float? value)
        {
            if (value == null)
                return new AttributeResult { IsValid = false, Message = "The field {0} cannot be null.", MessageArgs = [propertyName] };
            return ValidateNumber(propertyName, (decimal)value.Value);
        }

        /// <summary>
        /// Validates the numeric property value (nullable double).
        /// </summary>
        public virtual AttributeResult Validate(string propertyName, double? value)
        {
            if (value == null)
                return new AttributeResult { IsValid = false, Message = "The field {0} cannot be null.", MessageArgs = [propertyName] };
            return ValidateNumber(propertyName, (decimal)value.Value);
        }

        /// <summary>
        /// Validates the numeric property value (nullable decimal).
        /// </summary>
        public virtual AttributeResult Validate(string propertyName, decimal? value)
        {
            if (value == null)
                return new AttributeResult { IsValid = false, Message = "The field {0} cannot be null.", MessageArgs = [propertyName] };
            return ValidateNumber(propertyName, value.Value);
        }

        public abstract AttributeResult ValidateNumber(string propertyName, decimal value);
    }
}
