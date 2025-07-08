using BenchmarkDotNet.Attributes;
using FluentValidation;
using FluentValidation.Validators;
using EasyValidate.Core.Attributes;
using BenchmarkDotNet.Jobs;

namespace EasyValidate.Benchmarks
{
    /// <summary>
    /// Comprehensive benchmark suite comparing validation performance across
    /// DataAnnotations, FluentValidation, and EasyValidate for all major attribute types.
    /// 
    /// This benchmark suite provides:
    /// 1. Fair comparisons using equivalent validation logic across frameworks
    /// 2. Comprehensive coverage of 80+ EasyValidate attributes
    /// 3. Both valid and invalid test cases for each validation type
    /// 4. Memory allocation analysis using MemoryDiagnoser
    /// 5. Organized results by validation category (General, String, Numeric, Collection, Date)
    /// 
    /// Performance characteristics:
    /// - EasyValidate: Fastest (compile-time generation, minimal allocations)
    /// - DataAnnotations: Moderate (reflection overhead, boxing)
    /// - FluentValidation: Slowest (object allocation, pipeline complexity)
    /// 
    /// Run with: dotnet run -c Release -- --filter "*AttributeSetup*"
    /// </summary>
    [SimpleJob(RuntimeMoniker.Net90, launchCount: 1, warmupCount: 1, iterationCount: 1)]
    [MinColumn, MaxColumn, MeanColumn]
    [MemoryDiagnoser]
    public partial class AttributeSetupBenchmarks
    {
        // #region DataAnnotations Attributes
        // Removed all DataAnnotations attributes for EasyValidate-only benchmarks
        // #endregion

        #region EasyValidate Attributes
        // General Validation Attributes
        private NotNullAttribute _notNullAttribute = null!;
        private NotDefaultAttribute _notDefaultAttribute = null!;
        private EqualToAttribute _equalToAttribute = null!;
        private NotEqualToAttribute _notEqualToAttribute = null!;
        private OptionalAttribute _optionalAttribute = null!;

        // FluentValidation direct validators for EqualTo
        private InlineValidator<string> _fluentEqualToValidator = null!;

        // String Validation Attributes
        private AlphaAttribute _alphaAttribute = null!;
        private AlphaNumericAttribute _alphaNumericAttribute = null!;
        private AsciiAttribute _asciiAttribute = null!;
        private BaseEncodingAttribute _baseEncodingAttribute = null!;
        private ColorAttribute _colorAttribute = null!;
        private CommonPrintableAttribute _commonPrintableAttribute = null!;
        private ContainsAttribute _containsAttribute = null!;
        private Core.Attributes.CreditCardAttribute _easyCreditCardAttribute = null!;
        private Core.Attributes.EmailAddressAttribute _easyEmailAttribute = null!;
        private EndsWithAttribute _endsWithAttribute = null!;
        private FileExtensionAttribute _fileExtensionAttribute = null!;
        private FirstLetterUpperAttribute _firstLetterUpperAttribute = null!;
        private GuidAttribute _guidAttribute = null!;
        private HexAttribute _hexAttribute = null!;
        private ISBNAttribute _isbnAttribute = null!;
        private IpAddressAttribute _ipAddressAttribute = null!;
        private LowercaseAttribute _lowercaseAttribute = null!;
        private MacAddressAttribute _macAddressAttribute = null!;
        private MatchesAttribute _matchesAttribute = null!;
        private NoWhitespaceAttribute _noWhitespaceAttribute = null!;
        private NotContainsAttribute _notContainsAttribute = null!;
        private NotEmptyAttribute _notEmptyAttribute = null!;
        private NotOneOfAttribute _notOneOfAttribute = null!;
        private NumericAttribute _numericAttribute = null!;
        private OneOfAttribute _oneOfAttribute = null!;
        private Core.Attributes.PhoneAttribute _easyPhoneAttribute = null!;
        private StartsWithAttribute _startsWithAttribute = null!;
        private UppercaseAttribute _uppercaseAttribute = null!;
        private Core.Attributes.UrlAttribute _easyUrlAttribute = null!;
        private ValidEnumAttribute _validEnumAttribute = null!;

        // Numeric Validation Attributes
        private DivisibleByAttribute _divisibleByAttribute = null!;
        private EvenNumberAttribute _evenNumberAttribute = null!;
        private FibonacciAttribute _fibonacciAttribute = null!;
        private GreaterThanAttribute _greaterThanAttribute = null!;
        private GreaterThanOrEqualToAttribute _greaterThanOrEqualToAttribute = null!;
        private LessThanAttribute _lessThanAttribute = null!;
        private LessThanOrEqualToAttribute _lessThanOrEqualToAttribute = null!;
        private MaxDigitsAttribute _maxDigitsAttribute = null!;
        private MinDigitsAttribute _minDigitsAttribute = null!;
        private MultipleOfAttribute _multipleOfAttribute = null!;
        private NegativeAttribute _negativeAttribute = null!;
        private OddNumberAttribute _oddNumberAttribute = null!;
        private PositiveAttribute _positiveAttribute = null!;
        private PowerOfAttribute _powerOfAttribute = null!;
        private PrimeAttribute _primeAttribute = null!;
        private Core.Attributes.RangeAttribute _easyRangeAttribute = null!;

        // Collection Validation Attributes
        private ContainsElementAttribute _containsElementAttribute = null!;
        private HasElementsAttribute _hasElementsAttribute = null!;
        private Core.Attributes.LengthAttribute _easyLengthAttribute = null!;
        private Core.Attributes.MaxLengthAttribute _easyMaxLengthAttribute = null!;
        private Core.Attributes.MinLengthAttribute _easyMinLengthAttribute = null!;
        private NoNullElementsAttribute _noNullElementsAttribute = null!;
        private NotContainElementAttribute _notContainElementAttribute = null!;
        private SingleAttribute _singleAttribute = null!;
        private SingleOrNoneAttribute _singleOrNoneAttribute = null!;
        private UniqueElementsAttribute _uniqueElementsAttribute = null!;

        // Date & Time Validation Attributes  
        private AgeRangeAttribute _easyAgeRangeAttribute = null!;
        private DateRangeAttribute _dateRangeAttribute = null!;
        private DayAttribute _dayAttribute = null!;
        private DayOfWeekAttribute _dayOfWeekAttribute = null!;
        private FutureDateAttribute _futureDateAttribute = null!;
        private LeapYearAttribute _leapYearAttribute = null!;
        private MaxAgeAttribute _maxAgeAttribute = null!;
        private MinAgeAttribute _minAgeAttribute = null!;
        private MonthAttribute _monthAttribute = null!;
        private NotInFutureAttribute _notInFutureAttribute = null!;
        private NotInPastAttribute _notInPastAttribute = null!;
        private NotLeapYearAttribute _notLeapYearAttribute = null!;
        private NotTodayDateAttribute _notTodayDateAttribute = null!;
        private NotUTCAttribute _notUTCAttribute = null!;
        private PastDateAttribute _pastDateAttribute = null!;
        private QuarterAttribute _quarterAttribute = null!;
        private TimeRangeAttribute _timeRangeAttribute = null!;
        private TodayAttribute _todayAttribute = null!;
        private UTCAttribute _utcAttribute = null!;
        private YearAttribute _yearAttribute = null!;
        #endregion

        // #region FluentValidation Direct Validators (removed)
        // #endregion

        #region Test Values
        // String test values
        private string _validName = "John Doe";
        private string _invalidName = "";
        private string _validEmail = "john@example.com";
        private string _invalidEmail = "invalid-email";
        private string _validUrl = "https://example.com";
        private string _invalidUrl = "not-a-url";
        private string _validPhone = "+1-555-0123";
        private string _invalidPhone = "123";
        private string _validCreditCard = "4111111111111111";
        private string _invalidCreditCard = "1234567890";
        private string _validGuid = "550e8400-e29b-41d4-a716-446655440000";
        private string _invalidGuid = "not-a-guid";
        private string _validAlpha = "OnlyLetters";
        private string _invalidAlpha = "Letters123";
        private string _validAlphaNumeric = "Letters123";
        private string _invalidAlphaNumeric = "Letters!@#";
        private string _validNumeric = "12345";
        private string _invalidNumeric = "123abc";
        private string _validUppercase = "UPPERCASE";
        private string _invalidUppercase = "lowercase";
        private string _validLowercase = "lowercase";
        private string _invalidLowercase = "UPPERCASE";
        private string _validHex = "1A2B3C";
        private string _invalidHex = "GHIJK";
        private string _validColor = "#FF5733";
        private string _invalidColor = "notacolor";
        private string _validIpAddress = "192.168.1.1";
        private string _invalidIpAddress = "999.999.999.999";
        private string _validMacAddress = "00:1B:44:11:3A:B7";
        private string _invalidMacAddress = "invalid-mac";
        private string _validIsbn = "978-3-16-148410-0";
        private string _invalidIsbn = "123-456-789";
        private string _validAscii = "Hello World";
        private string _invalidAscii = "Hello 世界";
        private string _validFileExtension = "document.txt";
        private string _invalidFileExtension = "document.exe";
        private string _validFirstLetterUpper = "Hello";
        private string _invalidFirstLetterUpper = "hello";
        private string _validBase64 = "SGVsbG8gV29ybGQ=";
        private string _invalidBase64 = "Invalid!@#";
        private string _stringWithWhitespace = "Hello World";
        private string _stringWithoutWhitespace = "HelloWorld";

        // Numeric test values
        private int _validAge = 25;
        private int _invalidAge = 10;
        private decimal _validSalary = 75000;
        private decimal _invalidSalary = 10000;
        private int _positiveNumber = 42;
        private int _negativeNumber = -5;
        private int _zeroNumber = 0;
        private int _evenNumber = 8;
        private int _oddNumber = 7;
        private int _primeNumber = 17;
        private int _nonPrimeNumber = 15;
        private int _multipleOfFive = 25;
        private int _notMultipleOfFive = 23;
        private int _powerOfTwo = 16;
        private int _notPowerOfTwo = 15;
        private int _fibonacciNumber = 13;
        private int _nonFibonacciNumber = 14;
        private int _twoDigitNumber = 42;
        private int _oneDigitNumber = 5;
        private int _threeDigitNumber = 123;

        // Collection test values
        private int[] _validArray = { 1, 2, 3, 4, 5 };
        private int[] _invalidArray = Array.Empty<int>();
        private int[] _exactLengthArray = { 1, 2, 3, 4, 5 };
        private int[] _shortArray = { 1, 2 };
        private int[] _longArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        private List<int> _uniqueList = new() { 1, 2, 3, 4, 5 };
        private List<int> _duplicateList = new() { 1, 2, 2, 3, 4 };
        private List<int> _singleElementList = new() { 42 };
        private List<int> _multipleElementsList = new() { 1, 2, 3 };
        private List<int> _emptyList = new();
        private List<int> _listContainingFive = new() { 1, 2, 5, 3, 4 };
        private List<int> _listNotContainingFive = new() { 1, 2, 3, 4 };
        private List<string?> _nullElementsList = new() { "a", null, "b" };
        private List<string> _noNullElementsList = new() { "a", "b", "c" };

        // Date test values
        private DateTime _futureDate = DateTime.Now.AddDays(30);
        private DateTime _pastDate = DateTime.Now.AddDays(-30);
        private DateTime _today = DateTime.Today;
        private DateTime _validBirthDate = DateTime.Now.AddYears(-30);
        private DateTime _invalidBirthDate = DateTime.Now.AddYears(-5);
        private DateTime _leapYearDate = new(2024, 2, 29);
        private DateTime _nonLeapYearDate = new(2023, 2, 28);
        private DateTime _mondayDate = new(2024, 7, 8); // Monday
        private DateTime _tuesdayDate = new(2024, 7, 9); // Tuesday
        private DateTime _juneDate = new(2024, 6, 15);
        private DateTime _februaryDate = new(2024, 2, 15);
        private DateTime _q2Date = new(2024, 5, 15); // Q2
        private DateTime _q3Date = new(2024, 8, 15); // Q3
        private DateTime _2024Date = new(2024, 6, 15);
        private DateTime _2023Date = new(2023, 6, 15);
        private DateTime _day15Date = new(2024, 6, 15);
        private DateTime _day20Date = new(2024, 6, 20);
        private DateTime _utcDate = DateTime.UtcNow;
        private DateTime _localDate = DateTime.Now;
        private DateTime _validWorkTimeDate = new(2024, 6, 15, 10, 30, 0); // 10:30 AM
        private DateTime _invalidWorkTimeDate = new(2024, 6, 15, 20, 30, 0); // 8:30 PM
        #endregion

        [GlobalSetup]
        public void Setup()
        {
            // DataAnnotations setup removed for EasyValidate-only benchmarks

            // Initialize EasyValidate General attributes
            _notNullAttribute = new NotNullAttribute();
            _notDefaultAttribute = new NotDefaultAttribute();
            _equalToAttribute = new EqualToAttribute("test");
            _notEqualToAttribute = new NotEqualToAttribute("test");
            _optionalAttribute = new OptionalAttribute();

            // Initialize FluentValidation direct validator for EqualTo
            _fluentEqualToValidator = new InlineValidator<string>();
            _fluentEqualToValidator.RuleFor(x => x).Equal("test");

            // Initialize EasyValidate String attributes
            _alphaAttribute = new AlphaAttribute();
            _alphaNumericAttribute = new AlphaNumericAttribute();
            _asciiAttribute = new AsciiAttribute();
            _baseEncodingAttribute = new BaseEncodingAttribute(BaseEncodingAttribute.BaseType.Base64);
            _colorAttribute = new ColorAttribute();
            _commonPrintableAttribute = new CommonPrintableAttribute();
            _containsAttribute = new ContainsAttribute("test");
            _easyCreditCardAttribute = new Core.Attributes.CreditCardAttribute();
            _easyEmailAttribute = new Core.Attributes.EmailAddressAttribute();
            _endsWithAttribute = new EndsWithAttribute("test");
            _fileExtensionAttribute = new FileExtensionAttribute(".txt");
            _firstLetterUpperAttribute = new FirstLetterUpperAttribute();
            _guidAttribute = new GuidAttribute();
            _hexAttribute = new HexAttribute();
            _isbnAttribute = new ISBNAttribute();
            _ipAddressAttribute = new IpAddressAttribute();
            _lowercaseAttribute = new LowercaseAttribute();
            _macAddressAttribute = new MacAddressAttribute();
            _matchesAttribute = new MatchesAttribute(@"^\+\d{1,3}-\d{3}-\d{4}$");
            _noWhitespaceAttribute = new NoWhitespaceAttribute();
            _notContainsAttribute = new NotContainsAttribute("forbidden");
            _notEmptyAttribute = new NotEmptyAttribute();
            _notOneOfAttribute = new NotOneOfAttribute(new[] { "bad", "invalid" });
            _numericAttribute = new NumericAttribute();
            _oneOfAttribute = new OneOfAttribute(new[] { "valid", "good" });
            _easyPhoneAttribute = new Core.Attributes.PhoneAttribute();
            _startsWithAttribute = new StartsWithAttribute("test");
            _uppercaseAttribute = new UppercaseAttribute();
            _easyUrlAttribute = new Core.Attributes.UrlAttribute();
            _validEnumAttribute = new ValidEnumAttribute(typeof(DayOfWeek));

            // Initialize EasyValidate Numeric attributes
            _divisibleByAttribute = new DivisibleByAttribute(2);
            _evenNumberAttribute = new EvenNumberAttribute();
            _fibonacciAttribute = new FibonacciAttribute();
            _greaterThanAttribute = new GreaterThanAttribute(0);
            _greaterThanOrEqualToAttribute = new GreaterThanOrEqualToAttribute(1);
            _lessThanAttribute = new LessThanAttribute(100);
            _lessThanOrEqualToAttribute = new LessThanOrEqualToAttribute(99);
            _maxDigitsAttribute = new MaxDigitsAttribute(10);
            _minDigitsAttribute = new MinDigitsAttribute(1);
            _multipleOfAttribute = new MultipleOfAttribute(5);
            _negativeAttribute = new NegativeAttribute();
            _oddNumberAttribute = new OddNumberAttribute();
            _positiveAttribute = new PositiveAttribute();
            _powerOfAttribute = new PowerOfAttribute(2);
            _primeAttribute = new PrimeAttribute();
            _easyRangeAttribute = new Core.Attributes.RangeAttribute(18, 120);

            // Initialize EasyValidate Collection attributes
            _containsElementAttribute = new ContainsElementAttribute(5);
            _hasElementsAttribute = new HasElementsAttribute();
            _easyLengthAttribute = new Core.Attributes.LengthAttribute(5);
            _easyMaxLengthAttribute = new Core.Attributes.MaxLengthAttribute(10);
            _easyMinLengthAttribute = new Core.Attributes.MinLengthAttribute(2);
            _noNullElementsAttribute = new NoNullElementsAttribute();
            _notContainElementAttribute = new NotContainElementAttribute(99);
            _singleAttribute = new SingleAttribute(42);
            _singleOrNoneAttribute = new SingleOrNoneAttribute(42);
            _uniqueElementsAttribute = new UniqueElementsAttribute();

            // Initialize EasyValidate Date & Time attributes
            _easyAgeRangeAttribute = new AgeRangeAttribute(18, 65);
            _dateRangeAttribute = new DateRangeAttribute(DateTime.Now.AddYears(-1), DateTime.Now.AddYears(1));
            _dayAttribute = new DayAttribute(15);
            _dayOfWeekAttribute = new DayOfWeekAttribute(DayOfWeek.Monday);
            _futureDateAttribute = new FutureDateAttribute();
            _leapYearAttribute = new LeapYearAttribute();
            _maxAgeAttribute = new MaxAgeAttribute(65);
            _minAgeAttribute = new MinAgeAttribute(18);
            _monthAttribute = new MonthAttribute(6);
            _notInFutureAttribute = new NotInFutureAttribute();
            _notInPastAttribute = new NotInPastAttribute();
            _notLeapYearAttribute = new NotLeapYearAttribute();
            _notTodayDateAttribute = new NotTodayDateAttribute();
            _notUTCAttribute = new NotUTCAttribute();
            _pastDateAttribute = new PastDateAttribute();
            _quarterAttribute = new QuarterAttribute(Quarter.Q2);
            _timeRangeAttribute = new TimeRangeAttribute(TimeSpan.FromHours(9), TimeSpan.FromHours(17));
            _todayAttribute = new TodayAttribute();
            _utcAttribute = new UTCAttribute();
            _yearAttribute = new YearAttribute(2024);

            // FluentValidation direct inner validators removed
        }


        // ============================
        // GENERAL VALIDATION BENCHMARKS
        // ============================

        #region General
        // ===== Required =====
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool Required_EasyValidate_Valid() => _notNullAttribute.Validate(this, "Name", _validName).IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool Required_EasyValidate_Invalid() => _notNullAttribute.Validate(this, "Name", null).IsValid;
        // DataAnnotations Required benchmarks removed

        // ===== NotDefault =====
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool NotDefault_EasyValidate_Valid() => _notDefaultAttribute.Validate(this, "Name", _validName).IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool NotDefault_EasyValidate_Invalid() => _notDefaultAttribute.Validate(this, "Name", "").IsValid;

        // ===== Optional =====
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool Optional_EasyValidate_Valid() => _optionalAttribute.Validate(this, "Name", null).IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool Optional_EasyValidate_Invalid() => _optionalAttribute.Validate(this, "Name", "").IsValid;

        // ===== EqualTo =====
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool EqualTo_EasyValidate_Valid() => _equalToAttribute.Validate(this, "Value", "test").IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool EqualTo_EasyValidate_Invalid() => _equalToAttribute.Validate(this, "Value", "invalid").IsValid;
        // DataAnnotations does not have an EqualTo equivalent

        // ===== NotEqualTo =====
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool NotEqualTo_EasyValidate_Valid() => _notEqualToAttribute.Validate(this, "Value", "different").IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool NotEqualTo_EasyValidate_Invalid() => _notEqualToAttribute.Validate(this, "Value", "test").IsValid;
        // DataAnnotations does not have a NotEqualTo equivalent
        #endregion

        // ============================
        // STRING VALIDATION BENCHMARKS
        // ============================

        #region String
        // ===== Email =====
        // DataAnnotations Email benchmarks removed
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Email_EasyValidate_Valid() => _easyEmailAttribute.Validate(this, "Email", _validEmail).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Email_EasyValidate_Invalid() => _easyEmailAttribute.Validate(this, "Email", _invalidEmail).IsValid;

        // ===== Alpha =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Alpha_EasyValidate_Valid() => _alphaAttribute.Validate(this, "Alpha", _validAlpha).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Alpha_EasyValidate_Invalid() => _alphaAttribute.Validate(this, "Alpha", _invalidAlpha).IsValid;

        // ===== AlphaNumeric =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool AlphaNumeric_EasyValidate_Valid() => _alphaNumericAttribute.Validate(this, "AlphaNumeric", _validAlphaNumeric).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool AlphaNumeric_EasyValidate_Invalid() => _alphaNumericAttribute.Validate(this, "AlphaNumeric", _invalidAlphaNumeric).IsValid;

        // ===== Numeric =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Numeric_EasyValidate_Valid() => _numericAttribute.Validate(this, "Numeric", _validNumeric, out _).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Numeric_EasyValidate_Invalid() => _numericAttribute.Validate(this, "Numeric", _invalidNumeric, out _).IsValid;

        // ===== Uppercase =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Uppercase_EasyValidate_Valid() => _uppercaseAttribute.Validate(this, "Uppercase", _validUppercase).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Uppercase_EasyValidate_Invalid() => _uppercaseAttribute.Validate(this, "Uppercase", _invalidUppercase).IsValid;

        // ===== Lowercase =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Lowercase_EasyValidate_Valid() => _lowercaseAttribute.Validate(this, "Lowercase", _validLowercase).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Lowercase_EasyValidate_Invalid() => _lowercaseAttribute.Validate(this, "Lowercase", _invalidLowercase).IsValid;

        // ===== Url =====
        // DataAnnotations Url benchmarks removed
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Url_EasyValidate_Valid() => _easyUrlAttribute.Validate(this, "Url", _validUrl, out _).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Url_EasyValidate_Invalid() => _easyUrlAttribute.Validate(this, "Url", _invalidUrl, out _).IsValid;

        // ===== Phone =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Phone_EasyValidate_Valid() => _easyPhoneAttribute.Validate(this, "Phone", _validPhone).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Phone_EasyValidate_Invalid() => _easyPhoneAttribute.Validate(this, "Phone", _invalidPhone).IsValid;

        // ===== CreditCard =====
        // DataAnnotations CreditCard benchmarks removed
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool CreditCard_EasyValidate_Valid() => _easyCreditCardAttribute.Validate(this, "CreditCard", _validCreditCard).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool CreditCard_EasyValidate_Invalid() => _easyCreditCardAttribute.Validate(this, "CreditCard", _invalidCreditCard).IsValid;

        // ===== Guid =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Guid_EasyValidate_Valid() => _guidAttribute.Validate(this, "Guid", _validGuid).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Guid_EasyValidate_Invalid() => _guidAttribute.Validate(this, "Guid", _invalidGuid).IsValid;

        // ===== Hex =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Hex_EasyValidate_Valid() => _hexAttribute.Validate(this, "Hex", _validHex).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Hex_EasyValidate_Invalid() => _hexAttribute.Validate(this, "Hex", _invalidHex).IsValid;

        // ===== Color =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Color_EasyValidate_Valid() => _colorAttribute.Validate(this, "Color", _validColor).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Color_EasyValidate_Invalid() => _colorAttribute.Validate(this, "Color", _invalidColor).IsValid;

        // ===== IpAddress =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool IpAddress_EasyValidate_Valid() => _ipAddressAttribute.Validate(this, "IpAddress", _validIpAddress).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool IpAddress_EasyValidate_Invalid() => _ipAddressAttribute.Validate(this, "IpAddress", _invalidIpAddress).IsValid;

        // ===== MacAddress =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool MacAddress_EasyValidate_Valid() => _macAddressAttribute.Validate(this, "MacAddress", _validMacAddress).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool MacAddress_EasyValidate_Invalid() => _macAddressAttribute.Validate(this, "MacAddress", _invalidMacAddress).IsValid;

        // ===== ISBN =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool ISBN_EasyValidate_Valid() => _isbnAttribute.Validate(this, "ISBN", _validIsbn).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool ISBN_EasyValidate_Invalid() => _isbnAttribute.Validate(this, "ISBN", _invalidIsbn).IsValid;

        // ===== Ascii =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Ascii_EasyValidate_Valid() => _asciiAttribute.Validate(this, "Ascii", _validAscii).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Ascii_EasyValidate_Invalid() => _asciiAttribute.Validate(this, "Ascii", _invalidAscii).IsValid;

        // ===== Contains =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Contains_EasyValidate_Valid() => _containsAttribute.Validate(this, "Contains", "test value").IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Contains_EasyValidate_Invalid() => _containsAttribute.Validate(this, "Contains", "invalid").IsValid;

        // ===== StartsWith =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool StartsWith_EasyValidate_Valid() => _startsWithAttribute.Validate(this, "StartsWith", "test value").IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool StartsWith_EasyValidate_Invalid() => _startsWithAttribute.Validate(this, "StartsWith", "invalid").IsValid;

        // ===== EndsWith =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool EndsWith_EasyValidate_Valid() => _endsWithAttribute.Validate(this, "EndsWith", "value test").IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool EndsWith_EasyValidate_Invalid() => _endsWithAttribute.Validate(this, "EndsWith", "invalid").IsValid;

        // ===== Matches =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Matches_EasyValidate_Valid() => _matchesAttribute.Validate(this, "Matches", _validPhone).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool Matches_EasyValidate_Invalid() => _matchesAttribute.Validate(this, "Matches", _invalidPhone).IsValid;

        // ===== FirstLetterUpper =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool FirstLetterUpper_EasyValidate_Valid() => _firstLetterUpperAttribute.Validate(this, "FirstLetterUpper", _validFirstLetterUpper).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool FirstLetterUpper_EasyValidate_Invalid() => _firstLetterUpperAttribute.Validate(this, "FirstLetterUpper", _invalidFirstLetterUpper).IsValid;

        // ===== NotEmpty =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool NotEmpty_EasyValidate_Valid() => _notEmptyAttribute.Validate(this, "NotEmpty", _validName).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool NotEmpty_EasyValidate_Invalid() => _notEmptyAttribute.Validate(this, "NotEmpty", _invalidName).IsValid;

        // ===== NoWhitespace =====
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool NoWhitespace_EasyValidate_Valid() => _noWhitespaceAttribute.Validate(this, "NoWhitespace", _stringWithoutWhitespace).IsValid;
        [Benchmark]
        [BenchmarkCategory("String")]
        public bool NoWhitespace_EasyValidate_Invalid() => _noWhitespaceAttribute.Validate(this, "NoWhitespace", _stringWithWhitespace).IsValid;
        #endregion

        // ============================
        // NUMERIC VALIDATION BENCHMARKS
        // ============================

        // ============================
        // NUMERIC VALIDATION BENCHMARKS
        // ============================
        #region Numeric
        // ===== Range =====
        // DataAnnotations Range benchmarks removed
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Range_Valid() => _easyRangeAttribute.Validate(this, "Range", _validAge).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Range_Invalid() => _easyRangeAttribute.Validate(this, "Range", _invalidAge).IsValid;

        // ===== GreaterThan =====
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_GreaterThan_Valid() => _greaterThanAttribute.Validate(this, "GreaterThan", _positiveNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_GreaterThan_Invalid() => _greaterThanAttribute.Validate(this, "GreaterThan", _negativeNumber).IsValid;

        // ===== GreaterThanOrEqualTo =====
        // (Removed duplicate GreaterThanOrEqualTo benchmarks)

        // ===== LessThan =====
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_LessThan_Valid() => _lessThanAttribute.Validate(this, "LessThan", _validAge).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_LessThan_Invalid() => _lessThanAttribute.Validate(this, "LessThan", 150).IsValid;

        // ===== LessThanOrEqualTo =====
        // (Removed duplicate LessThanOrEqualTo benchmarks)

        // ===== Positive =====
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Positive_Valid() => _positiveAttribute.Validate(this, "Positive", _positiveNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Positive_Invalid() => _positiveAttribute.Validate(this, "Positive", _negativeNumber).IsValid;

        // ===== Negative =====
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Negative_Valid() => _negativeAttribute.Validate(this, "Negative", _negativeNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Negative_Invalid() => _negativeAttribute.Validate(this, "Negative", _positiveNumber).IsValid;

        // ===== EvenNumber =====
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Even_Valid() => _evenNumberAttribute.Validate(this, "Even", _evenNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Even_Invalid() => _evenNumberAttribute.Validate(this, "Even", _oddNumber).IsValid;

        // ===== OddNumber =====
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Odd_Valid() => _oddNumberAttribute.Validate(this, "Odd", _oddNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Odd_Invalid() => _oddNumberAttribute.Validate(this, "Odd", _evenNumber).IsValid;

        // ===== Prime =====
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Prime_Valid() => _primeAttribute.Validate(this, "Prime", _primeNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Prime_Invalid() => _primeAttribute.Validate(this, "Prime", _nonPrimeNumber).IsValid;

        // ===== MultipleOf =====
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_MultipleOf_Valid() => _multipleOfAttribute.Validate(this, "MultipleOf", _multipleOfFive).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_MultipleOf_Invalid() => _multipleOfAttribute.Validate(this, "MultipleOf", _notMultipleOfFive).IsValid;

        // ===== DivisibleBy =====
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_DivisibleBy_Valid() => _divisibleByAttribute.Validate(this, "DivisibleBy", _evenNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_DivisibleBy_Invalid() => _divisibleByAttribute.Validate(this, "DivisibleBy", _oddNumber).IsValid;

        // ===== PowerOf =====
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_PowerOf_Valid() => _powerOfAttribute.Validate(this, "PowerOf", _powerOfTwo).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_PowerOf_Invalid() => _powerOfAttribute.Validate(this, "PowerOf", _notPowerOfTwo).IsValid;

        // ===== Fibonacci =====
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Fibonacci_Valid() => _fibonacciAttribute.Validate(this, "Fibonacci", _fibonacciNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Fibonacci_Invalid() => _fibonacciAttribute.Validate(this, "Fibonacci", _nonFibonacciNumber).IsValid;

        // ===== MaxDigits =====
        // (Removed duplicate MaxDigits benchmarks)

        // ===== MinDigits =====
        // (Removed duplicate MinDigits benchmarks)
        #endregion

        // ============================
        // COLLECTION VALIDATION BENCHMARKS
        // ============================

        #region Collection
        // ===== StringLength =====
        // Removed DataAnnotations StringLength benchmarks

        // ===== Length =====
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool Length_EasyValidate_Valid() => _easyLengthAttribute.Validate(this, "Length", _exactLengthArray).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool Length_EasyValidate_Invalid() => _easyLengthAttribute.Validate(this, "Length", _shortArray).IsValid;

        // ===== MinLength =====
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool MinLength_EasyValidate_Valid() => _easyMinLengthAttribute.Validate(this, "MinLength", _validArray).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool MinLength_EasyValidate_Invalid() => _easyMinLengthAttribute.Validate(this, "MinLength", _shortArray).IsValid;

        // ===== MaxLength =====
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool MaxLength_EasyValidate_Valid() => _easyMaxLengthAttribute.Validate(this, "MaxLength", _validArray).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool MaxLength_EasyValidate_Invalid() => _easyMaxLengthAttribute.Validate(this, "MaxLength", _longArray).IsValid;

        // ===== HasElements =====
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool HasElements_EasyValidate_Valid() => _hasElementsAttribute.Validate(this, "HasElements", _validArray).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool HasElements_EasyValidate_Invalid() => _hasElementsAttribute.Validate(this, "HasElements", _invalidArray).IsValid;

        // ===== UniqueElements =====
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool UniqueElements_EasyValidate_Valid() => _uniqueElementsAttribute.Validate(this, "UniqueElements", _uniqueList).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool UniqueElements_EasyValidate_Invalid() => _uniqueElementsAttribute.Validate(this, "UniqueElements", _duplicateList).IsValid;

        // ===== NoNullElements =====
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool NoNullElements_EasyValidate_Valid() => _noNullElementsAttribute.Validate(this, "NoNullElements", _noNullElementsList).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool NoNullElements_EasyValidate_Invalid() => _noNullElementsAttribute.Validate(this, "NoNullElements", _nullElementsList).IsValid;

        // ===== ContainsElement =====
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool ContainsElement_EasyValidate_Valid() => _containsElementAttribute.Validate(this, "ContainsElement", _listContainingFive).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool ContainsElement_EasyValidate_Invalid() => _containsElementAttribute.Validate(this, "ContainsElement", _listNotContainingFive).IsValid;

        // ===== Single =====
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool Single_EasyValidate_Valid() => _singleAttribute.Validate(this, "Single", _singleElementList).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool Single_EasyValidate_Invalid() => _singleAttribute.Validate(this, "Single", _multipleElementsList).IsValid;
        #endregion

        // ============================
        // DATE & TIME VALIDATION BENCHMARKS
        // ============================

        #region DateTime
        // ===== FutureDate =====
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool FutureDate_EasyValidate_Valid() => _futureDateAttribute.Validate(this, "FutureDate", _futureDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool FutureDate_EasyValidate_Invalid() => _futureDateAttribute.Validate(this, "FutureDate", _pastDate).IsValid;

        // ===== PastDate =====
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool PastDate_EasyValidate_Valid() => _pastDateAttribute.Validate(this, "PastDate", _pastDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool PastDate_EasyValidate_Invalid() => _pastDateAttribute.Validate(this, "PastDate", _futureDate).IsValid;

        // ===== Today =====
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Today_EasyValidate_Valid() => _todayAttribute.Validate(this, "Today", _today).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Today_EasyValidate_Invalid() => _todayAttribute.Validate(this, "Today", _pastDate).IsValid;

        // ===== LeapYear =====
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool LeapYear_EasyValidate_Valid() => _leapYearAttribute.Validate(this, "LeapYear", _leapYearDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool LeapYear_EasyValidate_Invalid() => _leapYearAttribute.Validate(this, "LeapYear", _nonLeapYearDate).IsValid;

        // ===== AgeRange =====
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool AgeRange_EasyValidate_Valid() => _easyAgeRangeAttribute.Validate(this, "AgeRange", _validBirthDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool AgeRange_EasyValidate_Invalid() => _easyAgeRangeAttribute.Validate(this, "AgeRange", _invalidBirthDate).IsValid;

        // ===== DayOfWeek =====
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool DayOfWeek_EasyValidate_Valid() => _dayOfWeekAttribute.Validate(this, "DayOfWeek", _mondayDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool DayOfWeek_EasyValidate_Invalid() => _dayOfWeekAttribute.Validate(this, "DayOfWeek", _tuesdayDate).IsValid;

        // ===== Month =====
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Month_EasyValidate_Valid() => _monthAttribute.Validate(this, "Month", _juneDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Month_EasyValidate_Invalid() => _monthAttribute.Validate(this, "Month", _februaryDate).IsValid;

        // ===== Year =====
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Year_EasyValidate_Valid() => _yearAttribute.Validate(this, "Year", _2024Date).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Year_EasyValidate_Invalid() => _yearAttribute.Validate(this, "Year", _2023Date).IsValid;

        // ===== Day =====
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Day_EasyValidate_Valid() => _dayAttribute.Validate(this, "Day", _day15Date).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Day_EasyValidate_Invalid() => _dayAttribute.Validate(this, "Day", _day20Date).IsValid;

        // ===== Quarter =====
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Quarter_EasyValidate_Valid() => _quarterAttribute.Validate(this, "Quarter", _q2Date).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Quarter_EasyValidate_Invalid() => _quarterAttribute.Validate(this, "Quarter", _q3Date).IsValid;

        // ===== UTC =====
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool UTC_EasyValidate_Valid() => _utcAttribute.Validate(this, "UTC", _utcDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool UTC_EasyValidate_Invalid() => _utcAttribute.Validate(this, "UTC", _localDate).IsValid;

        // ===== TimeRange =====
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool TimeRange_EasyValidate_Valid() => _timeRangeAttribute.Validate(this, "TimeRange", _validWorkTimeDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool TimeRange_EasyValidate_Invalid() => _timeRangeAttribute.Validate(this, "TimeRange", _invalidWorkTimeDate).IsValid;
        #endregion

        // ============================
        // ADDITIONAL EASYVALIDATE ATTRIBUTES
        // ============================

        // ============================
        // OTHER & EDGE CASE VALIDATION BENCHMARKS
        // ============================
        #region Other
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_BaseEncoding_Valid() => _baseEncodingAttribute.Validate(this, "BaseEncoding", _validBase64).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_BaseEncoding_Invalid() => _baseEncodingAttribute.Validate(this, "BaseEncoding", _invalidBase64).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_CommonPrintable_Valid() => _commonPrintableAttribute.Validate(this, "CommonPrintable", _validAscii).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_CommonPrintable_Invalid() => _commonPrintableAttribute.Validate(this, "CommonPrintable", _invalidAscii).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_FileExtension_Valid() => _fileExtensionAttribute.Validate(this, "FileExtension", _validFileExtension).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_FileExtension_Invalid() => _fileExtensionAttribute.Validate(this, "FileExtension", _invalidFileExtension).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotContains_Valid() => _notContainsAttribute.Validate(this, "NotContains", "good content").IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotContains_Invalid() => _notContainsAttribute.Validate(this, "NotContains", "forbidden content").IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotOneOf_Valid() => _notOneOfAttribute.Validate(this, "NotOneOf", "good").IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotOneOf_Invalid() => _notOneOfAttribute.Validate(this, "NotOneOf", "bad").IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_OneOf_Valid() => _oneOfAttribute.Validate(this, "OneOf", "valid").IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_OneOf_Invalid() => _oneOfAttribute.Validate(this, "OneOf", "invalid").IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_ValidEnum_Valid() => _validEnumAttribute.Validate(this, "ValidEnum", "Monday").IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_ValidEnum_Invalid() => _validEnumAttribute.Validate(this, "ValidEnum", "InvalidDay").IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_GreaterThanOrEqual_Valid() => _greaterThanOrEqualToAttribute.Validate(this, "GreaterThanOrEqual", _positiveNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_GreaterThanOrEqual_Invalid() => _greaterThanOrEqualToAttribute.Validate(this, "GreaterThanOrEqual", _zeroNumber).IsValid;
        // Removed DataAnnotations GreaterThanOrEqual benchmarks
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_LessThanOrEqual_Valid() => _lessThanOrEqualToAttribute.Validate(this, "LessThanOrEqual", _validAge).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_LessThanOrEqual_Invalid() => _lessThanOrEqualToAttribute.Validate(this, "LessThanOrEqual", 150).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MaxDigits_Valid() => _maxDigitsAttribute.Validate(this, "MaxDigits", _twoDigitNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MaxDigits_Invalid() => _maxDigitsAttribute.Validate(this, "MaxDigits", _threeDigitNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MinDigits_Valid() => _minDigitsAttribute.Validate(this, "MinDigits", _twoDigitNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MinDigits_Invalid() => _minDigitsAttribute.Validate(this, "MinDigits", _oneDigitNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotContainElement_Valid() => _notContainElementAttribute.Validate(this, "NotContainElement", _listNotContainingFive).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotContainElement_Invalid() => _notContainElementAttribute.Validate(this, "NotContainElement", new List<int> { 1, 99, 3 }).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_SingleOrNone_Valid() => _singleOrNoneAttribute.Validate(this, "SingleOrNone", _emptyList).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_SingleOrNone_Invalid() => _singleOrNoneAttribute.Validate(this, "SingleOrNone", _multipleElementsList).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_DateRange_Valid() => _dateRangeAttribute.Validate(this, "DateRange", DateTime.Now).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_DateRange_Invalid() => _dateRangeAttribute.Validate(this, "DateRange", DateTime.Now.AddYears(-2)).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MaxAge_Valid() => _maxAgeAttribute.Validate(this, "MaxAge", _validBirthDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MaxAge_Invalid() => _maxAgeAttribute.Validate(this, "MaxAge", DateTime.Now.AddYears(-70)).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MinAge_Valid() => _minAgeAttribute.Validate(this, "MinAge", _validBirthDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MinAge_Invalid() => _minAgeAttribute.Validate(this, "MinAge", _invalidBirthDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotInFuture_Valid() => _notInFutureAttribute.Validate(this, "NotInFuture", _pastDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotInFuture_Invalid() => _notInFutureAttribute.Validate(this, "NotInFuture", _futureDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotInPast_Valid() => _notInPastAttribute.Validate(this, "NotInPast", _futureDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotInPast_Invalid() => _notInPastAttribute.Validate(this, "NotInPast", _pastDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotLeapYear_Valid() => _notLeapYearAttribute.Validate(this, "NotLeapYear", _nonLeapYearDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotLeapYear_Invalid() => _notLeapYearAttribute.Validate(this, "NotLeapYear", _leapYearDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotTodayDate_Valid() => _notTodayDateAttribute.Validate(this, "NotTodayDate", _pastDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotTodayDate_Invalid() => _notTodayDateAttribute.Validate(this, "NotTodayDate", _today).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotUTC_Valid() => _notUTCAttribute.Validate(this, "NotUTC", _localDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotUTC_Invalid() => _notUTCAttribute.Validate(this, "NotUTC", _utcDate).IsValid;
        #endregion
    }

    // Simple test model for FluentValidation
    public class TestModel
    {
        public string Name { get; set; } = "";
        public string Email { get; set; } = "";
        public int Age { get; set; }
        public string Phone { get; set; } = "";
        public decimal Salary { get; set; }
    }
}
