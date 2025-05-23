using System;
using System.Linq;
using EasyValidate.Abstraction;

namespace EasyValidate.Attributes
{
    /// <summary>
    /// Validates that a string is a valid Base16, Base32, Base58, Base62, Base64, or Base85 encoded value.
    /// </summary>
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = false)]
    public class BaseEncodingAttribute(BaseEncodingAttribute.BaseType encodingType) : StringValidationAttributeBase
    {
        public enum BaseType
        {
            Base16,
            Base32,
            Base58,
            Base62,
            Base64,
            Base85
        }

        public BaseType EncodingType { get; } = encodingType;

        public override string ErrorCode => $"{EncodingType}ValidationError";

        /// <inheritdoc/>
        public override AttributeResult Validate(string propertyName, string? value)
        {
            if (!string.IsNullOrEmpty(value) && !IsValid(value!, EncodingType))
            {
                return new AttributeResult
                {
                    IsValid = false,
                    Message = $"The field {{0}} must be a valid {EncodingType} encoded string.",
                    MessageArgs = new object?[] { propertyName }
                };
            }
            return new AttributeResult { IsValid = true };
        }

        private static bool IsValid(string value, BaseType type)
        {
            switch (type)
            {
                case BaseType.Base16:
                    return IsBase16String(value);
                case BaseType.Base32:
                    return IsBase32String(value);
                case BaseType.Base58:
                    return IsBase58String(value);
                case BaseType.Base62:
                    return IsBase62String(value);
                case BaseType.Base64:
                    return IsBase64String(value);
                case BaseType.Base85:
                    return IsBase85String(value);
                default:
                    return false;
            }
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
