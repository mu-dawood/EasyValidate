using System;
using System.Net;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string is a valid IPv4 or IPv6 address.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [IpAddress]
    ///     public string ServerIp { get; set; } // Valid: "192.168.1.1", "::1", "2001:db8::1", Invalid: "256.1.1.1", "invalid"
    ///     
    ///     [IpAddress]
    ///     public string ClientAddress { get; set; } // Valid: "127.0.0.1", "10.0.0.1", Invalid: "300.0.0.1"
    /// }
    /// </code>
    /// </example>
    public class IpAddressAttribute : StringValidationAttributeBase
    {
        private static readonly Lazy<IpAddressAttribute> _instance = new(() => new IpAddressAttribute());
        public static IpAddressAttribute Instance => _instance.Value;
        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "IpAddressValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(object obj, string propertyName, string value, out string output)
        {
            bool valid = IsValidIpAddress(value);
            output = value;
            return valid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be a valid IP address.", propertyName);
        }

        private static bool IsValidIpAddress(string value)
        {
            return IPAddress.TryParse(value, out _);
        }
    }
}
