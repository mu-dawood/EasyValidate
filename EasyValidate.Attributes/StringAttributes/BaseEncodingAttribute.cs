using System;
using System.Linq;
using EasyValidate.Abstractions;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Specifies the supported encoding base types for string validation.
    /// </summary>
    public enum BaseType
    {
        /// <summary>
        /// Base16 encoding (hexadecimal).
        /// </summary>
        Base16,
        /// <summary>
        /// Base32 encoding.
        /// </summary>
        Base32,
        /// <summary>
        /// Base58 encoding.
        /// </summary>
        Base58,
        /// <summary>
        /// Base62 encoding.
        /// </summary>
        Base62,
        /// <summary>
        /// Base64 encoding.
        /// </summary>
        Base64,
        /// <summary>
        /// Base85 encoding.
        /// </summary>
        Base85
    }


    /// <summary>
    /// Validates that a string is a valid Base16, Base32, Base58, Base62, Base64, or Base85 encoded value.
    /// </summary>
    /// <example>
    /// <code>
    /// public partial class Model
    /// {
    ///     [BaseEncoding(BaseEncodingAttribute.BaseType.Base64)]
    ///     public string EncodedData { get; set; } // Valid: "SGVsbG8gV29ybGQ=", Invalid: "Hello World"
    ///     
    ///     [BaseEncoding(BaseEncodingAttribute.BaseType.Base16)]
    ///     public string HexData { get; set; } // Valid: "48656C6C6F", Invalid: "Hello"
    /// }
    /// </code>
    /// </example>
    public class BaseEncodingAttribute(BaseType encodingType) : StringValidationAttributeBase
    {

        /// <summary>
        /// Gets the encoding type that this attribute validates against.
        /// </summary>
        public BaseType EncodingType { get; } = encodingType;


        /// <inheritdoc/>
        public override string ErrorCode { get; set; } = $"{encodingType}ValidationError";


        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string value)
        {
            bool isValid = !string.IsNullOrWhiteSpace(value) && IsBaseEncoded(value!);
            return isValid ? AttributeResult.Success() : AttributeResult.Fail("The {0} field must be a valid base-encoded string.", propertyName);
        }

        private bool IsBaseEncoded(string value)
        {
            return EncodingType switch
            {
                BaseType.Base16 => IsBase16String(value),
                BaseType.Base32 => IsBase32String(value),
                BaseType.Base58 => IsBase58String(value),
                BaseType.Base62 => IsBase62String(value),
                BaseType.Base64 => IsBase64String(value),
                BaseType.Base85 => IsBase85String(value),
                _ => false,
            };
        }

        private static bool IsBase16String(string value)
        {
            var trimmed = value.Trim();
            if (trimmed.Length == 0 || trimmed.Length % 2 != 0)
                return false;
            return trimmed.All(c => (c >= '0' && c <= '9') || (c >= 'A' && c <= 'F') || (c >= 'a' && c <= 'f'));
        }

        private static bool IsBase32String(string value)
        {
            var trimmed = value.Trim().Replace("=", "");
            if (trimmed.Length == 0)
                return false;
            return trimmed.All(c => (c >= 'A' && c <= 'Z') || (c >= '2' && c <= '7'));
        }

        private static bool IsBase58String(string value)
        {
            const string alphabet = "123456789ABCDEFGHJKLMNPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz";
            return value.All(c => alphabet.Contains(c));
        }

        private static bool IsBase62String(string value)
        {
            const string alphabet = "0123456789ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz";
            return value.All(c => alphabet.Contains(c));
        }

        private static bool IsBase64String(string value)
        {
            var trimmed = value.Trim();
            if (trimmed.Length == 0 || trimmed.Length % 4 != 0)
                return false;
            try
            {
                Convert.FromBase64String(trimmed);
                return true;
            }
            catch
            {
                return false;
            }
        }

        private static bool IsBase85String(string value)
        {
            var trimmed = value.Trim();
            if (trimmed.Length == 0 || trimmed.Length % 5 != 0)
                return false;
            return trimmed.All(c => c >= 33 && c <= 117);
        }
    }
}
