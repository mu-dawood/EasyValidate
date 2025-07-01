using System;
using System.Text.RegularExpressions;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Core.Attributes
{
    /// <summary>
    /// Validates that a string is a valid MAC address.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [MacAddress]
    ///     public string NetworkCard { get; set; } // Valid: "00:1B:44:11:3A:B7", "aa-bb-cc-dd-ee-ff", Invalid: "00:1B:44:11:3A", "invalid"
    ///     
    ///     [MacAddress]
    ///     public string DeviceIdentifier { get; set; } // Valid: "FF:FF:FF:FF:FF:FF", "00-00-00-00-00-00", Invalid: "GG:HH:II:JJ:KK:LL"
    /// }
    /// </code>
    /// </example>
    public class MacAddressAttribute : StringValidationAttributeBase
    {
        private static readonly Lazy<MacAddressAttribute> _instance = new(() => new MacAddressAttribute());
        public static MacAddressAttribute Instance => _instance.Value;
        private static readonly Regex MacAddressRegex = new(
            "^([0-9A-Fa-f]{2}[:-]){5}([0-9A-Fa-f]{2})$",
            RegexOptions.Compiled);

        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = "MacAddressValidationError";

        /// <inheritdoc/>
        public override string ErrorMessage { get; set; } = "The {0} field must be a valid MAC address.";

        /// <inheritdoc/>
        public override AttributeResult<string> Validate(object obj, string propertyName, string value)
        {
            bool valid = MacAddressRegex.IsMatch(value);
            return new AttributeResult<string>(valid, value, propertyName);
        }
    }
}
