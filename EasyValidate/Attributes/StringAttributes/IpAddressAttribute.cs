using System;
using System.Net;
using System.Net.Sockets;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string is a valid IPv4 or IPv6 address.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class IpAddressAttribute : StringValidationAttributeBase
    {
        public override string ErrorCode => "IpAddressValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (!string.IsNullOrEmpty(value) && !IsValidIpAddress(value!))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = "The field {0} must be a valid IPv4 or IPv6 address.",
                    MessageArgs = [propertyName]
                };
            }
            return new AttributeResult { IsValid = true };
        }

        private static bool IsValidIpAddress(string value)
        {
            return IPAddress.TryParse(value, out _);
        }
    }
}
