using BenchmarkDotNet.Attributes;
using EasyValidate.Core.Attributes;

namespace EasyValidate.Benchmarks
{
    [MemoryDiagnoser]
    public class StringBenchmarks
    {
        private AlphaAttribute _alphaAttribute = null!;
        private AlphaNumericAttribute _alphaNumericAttribute = null!;
        private NumericAttribute _numericAttribute = null!;
        private UppercaseAttribute _uppercaseAttribute = null!;
        private LowercaseAttribute _lowercaseAttribute = null!;
        private EmailAddressAttribute _easyEmailAttribute = null!;
        private UrlAttribute _easyUrlAttribute = null!;
        private PhoneAttribute _easyPhoneAttribute = null!;
        private CreditCardAttribute _easyCreditCardAttribute = null!;
        private GuidAttribute _guidAttribute = null!;
        private HexAttribute _hexAttribute = null!;
        private ColorAttribute _colorAttribute = null!;
        private IpAddressAttribute _ipAddressAttribute = null!;
        private MacAddressAttribute _macAddressAttribute = null!;
        private ISBNAttribute _isbnAttribute = null!;
        private AsciiAttribute _asciiAttribute = null!;
        private ContainsAttribute _containsAttribute = null!;
        private StartsWithAttribute _startsWithAttribute = null!;
        private EndsWithAttribute _endsWithAttribute = null!;
        private MatchesAttribute _matchesAttribute = null!;
        private FirstLetterUpperAttribute _firstLetterUpperAttribute = null!;
        private NotEmptyAttribute _notEmptyAttribute = null!;
        private NoWhitespaceAttribute _noWhitespaceAttribute = null!;
        private readonly string _validName = "John Doe";
        private readonly string _invalidName = "";
        private readonly string _validEmail = "john@example.com";
        private readonly string _invalidEmail = "invalid-email";
        private readonly string _validUrl = "https://example.com";
        private readonly string _invalidUrl = "not-a-url";
        private readonly string _validPhone = "+1-555-0123";
        private readonly string _invalidPhone = "123";
        private readonly string _validCreditCard = "4111111111111111";
        private readonly string _invalidCreditCard = "1234567890";
        private readonly string _validGuid = "550e8400-e29b-41d4-a716-446655440000";
        private readonly string _invalidGuid = "not-a-guid";
        private readonly string _validAlpha = "OnlyLetters";
        private readonly string _invalidAlpha = "Letters123";
        private readonly string _validAlphaNumeric = "Letters123";
        private readonly string _invalidAlphaNumeric = "Letters!@#";
        private readonly string _validNumeric = "12345";
        private readonly string _invalidNumeric = "123abc";
        private readonly string _validUppercase = "UPPERCASE";
        private readonly string _invalidUppercase = "lowercase";
        private readonly string _validLowercase = "lowercase";
        private readonly string _invalidLowercase = "UPPERCASE";
        private readonly string _validHex = "1A2B3C";
        private readonly string _invalidHex = "GHIJK";
        private readonly string _validColor = "#FF5733";
        private readonly string _invalidColor = "notacolor";
        private readonly string _validIpAddress = "192.168.1.1";
        private readonly string _invalidIpAddress = "999.999.999.999";
        private readonly string _validMacAddress = "00:1B:44:11:3A:B7";
        private readonly string _invalidMacAddress = "invalid-mac";
        private readonly string _validIsbn = "978-3-16-148410-0";
        private readonly string _invalidIsbn = "123-456-789";
        private readonly string _validAscii = "Hello World";
        private readonly string _invalidAscii = "Hello 世界";
        private readonly string _stringWithWhitespace = "Hello World";
        private readonly string _stringWithoutWhitespace = "HelloWorld";
        private readonly string _validFirstLetterUpper = "Hello";
        private readonly string _invalidFirstLetterUpper = "hello";

        [GlobalSetup]
        public void Setup()
        {
            _alphaAttribute = new AlphaAttribute();
            _alphaNumericAttribute = new AlphaNumericAttribute();
            _numericAttribute = new NumericAttribute();
            _uppercaseAttribute = new UppercaseAttribute();
            _lowercaseAttribute = new LowercaseAttribute();
            _easyEmailAttribute = new EmailAddressAttribute();
            _easyUrlAttribute = new UrlAttribute();
            _easyPhoneAttribute = new PhoneAttribute();
            _easyCreditCardAttribute = new CreditCardAttribute();
            _guidAttribute = new GuidAttribute();
            _hexAttribute = new HexAttribute();
            _colorAttribute = new ColorAttribute();
            _ipAddressAttribute = new IpAddressAttribute();
            _macAddressAttribute = new MacAddressAttribute();
            _isbnAttribute = new ISBNAttribute();
            _asciiAttribute = new AsciiAttribute();
            _containsAttribute = new ContainsAttribute("test");
            _startsWithAttribute = new StartsWithAttribute("test");
            _endsWithAttribute = new EndsWithAttribute("test");
            _matchesAttribute = new MatchesAttribute(@"^\\+\\d{1,3}-\\d{3}-\\d{4}$");
            _firstLetterUpperAttribute = new FirstLetterUpperAttribute();
            _notEmptyAttribute = new NotEmptyAttribute();
            _noWhitespaceAttribute = new NoWhitespaceAttribute();
        }

        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Email_EasyValidate_Valid() => _easyEmailAttribute.Validate(this, "Email", _validEmail).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Email_EasyValidate_Invalid() => _easyEmailAttribute.Validate(this, "Email", _invalidEmail).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Alpha_EasyValidate_Valid() => _alphaAttribute.Validate(this, "Alpha", _validAlpha).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Alpha_EasyValidate_Invalid() => _alphaAttribute.Validate(this, "Alpha", _invalidAlpha).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool AlphaNumeric_EasyValidate_Valid() => _alphaNumericAttribute.Validate(this, "AlphaNumeric", _validAlphaNumeric).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool AlphaNumeric_EasyValidate_Invalid() => _alphaNumericAttribute.Validate(this, "AlphaNumeric", _invalidAlphaNumeric).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Numeric_EasyValidate_Valid() => _numericAttribute.Validate(this, "Numeric", _validNumeric, out _).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Numeric_EasyValidate_Invalid() => _numericAttribute.Validate(this, "Numeric", _invalidNumeric, out _).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Uppercase_EasyValidate_Valid() => _uppercaseAttribute.Validate(this, "Uppercase", _validUppercase).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Uppercase_EasyValidate_Invalid() => _uppercaseAttribute.Validate(this, "Uppercase", _invalidUppercase).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Lowercase_EasyValidate_Valid() => _lowercaseAttribute.Validate(this, "Lowercase", _validLowercase).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Lowercase_EasyValidate_Invalid() => _lowercaseAttribute.Validate(this, "Lowercase", _invalidLowercase).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Url_EasyValidate_Valid() => _easyUrlAttribute.Validate(this, "Url", _validUrl, out _).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Url_EasyValidate_Invalid() => _easyUrlAttribute.Validate(this, "Url", _invalidUrl, out _).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Phone_EasyValidate_Valid() => _easyPhoneAttribute.Validate(this, "Phone", _validPhone).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Phone_EasyValidate_Invalid() => _easyPhoneAttribute.Validate(this, "Phone", _invalidPhone).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool CreditCard_EasyValidate_Valid() => _easyCreditCardAttribute.Validate(this, "CreditCard", _validCreditCard).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool CreditCard_EasyValidate_Invalid() => _easyCreditCardAttribute.Validate(this, "CreditCard", _invalidCreditCard).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Guid_EasyValidate_Valid() => _guidAttribute.Validate(this, "Guid", _validGuid).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Guid_EasyValidate_Invalid() => _guidAttribute.Validate(this, "Guid", _invalidGuid).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Hex_EasyValidate_Valid() => _hexAttribute.Validate(this, "Hex", _validHex).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Hex_EasyValidate_Invalid() => _hexAttribute.Validate(this, "Hex", _invalidHex).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Color_EasyValidate_Valid() => _colorAttribute.Validate(this, "Color", _validColor).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Color_EasyValidate_Invalid() => _colorAttribute.Validate(this, "Color", _invalidColor).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool IpAddress_EasyValidate_Valid() => _ipAddressAttribute.Validate(this, "IpAddress", _validIpAddress).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool IpAddress_EasyValidate_Invalid() => _ipAddressAttribute.Validate(this, "IpAddress", _invalidIpAddress).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool MacAddress_EasyValidate_Valid() => _macAddressAttribute.Validate(this, "MacAddress", _validMacAddress).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool MacAddress_EasyValidate_Invalid() => _macAddressAttribute.Validate(this, "MacAddress", _invalidMacAddress).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool ISBN_EasyValidate_Valid() => _isbnAttribute.Validate(this, "ISBN", _validIsbn).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool ISBN_EasyValidate_Invalid() => _isbnAttribute.Validate(this, "ISBN", _invalidIsbn).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Ascii_EasyValidate_Valid() => _asciiAttribute.Validate(this, "Ascii", _validAscii).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Ascii_EasyValidate_Invalid() => _asciiAttribute.Validate(this, "Ascii", _invalidAscii).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Contains_EasyValidate_Valid() => _containsAttribute.Validate(this, "Contains", "test value").IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Contains_EasyValidate_Invalid() => _containsAttribute.Validate(this, "Contains", "invalid").IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool StartsWith_EasyValidate_Valid() => _startsWithAttribute.Validate(this, "StartsWith", "test value").IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool StartsWith_EasyValidate_Invalid() => _startsWithAttribute.Validate(this, "StartsWith", "invalid").IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool EndsWith_EasyValidate_Valid() => _endsWithAttribute.Validate(this, "EndsWith", "value test").IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool EndsWith_EasyValidate_Invalid() => _endsWithAttribute.Validate(this, "EndsWith", "invalid").IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Matches_EasyValidate_Valid() => _matchesAttribute.Validate(this, "Matches", _validPhone).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Matches_EasyValidate_Invalid() => _matchesAttribute.Validate(this, "Matches", _invalidPhone).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool FirstLetterUpper_EasyValidate_Valid() => _firstLetterUpperAttribute.Validate(this, "FirstLetterUpper", _validFirstLetterUpper).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool FirstLetterUpper_EasyValidate_Invalid() => _firstLetterUpperAttribute.Validate(this, "FirstLetterUpper", _invalidFirstLetterUpper).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool NotEmpty_EasyValidate_Valid() => _notEmptyAttribute.Validate(this, "NotEmpty", _validName).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool NotEmpty_EasyValidate_Invalid() => _notEmptyAttribute.Validate(this, "NotEmpty", _invalidName).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool NoWhitespace_EasyValidate_Valid() => _noWhitespaceAttribute.Validate(this, "NoWhitespace", _stringWithoutWhitespace).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool NoWhitespace_EasyValidate_Invalid() => _noWhitespaceAttribute.Validate(this, "NoWhitespace", _stringWithWhitespace).IsValid;
    }
}
