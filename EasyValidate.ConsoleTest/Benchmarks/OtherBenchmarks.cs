using BenchmarkDotNet.Attributes;
using EasyValidate.Core.Abstraction;
using EasyValidate.Core.Attributes;

namespace EasyValidate.Benchmarks
{
    [MemoryDiagnoser]
    public class OtherBenchmarks
    {
        public OtherBenchmarks()
        {
            serviceProvider = new DefaultServiceProvider();
        }
        private readonly IServiceProvider serviceProvider;
        private BaseEncodingAttribute _baseEncodingAttribute = null!;
        private CommonPrintableAttribute _commonPrintableAttribute = null!;
        private FileExtensionAttribute _fileExtensionAttribute = null!;
        private NotContainsAttribute _notContainsAttribute = null!;
        private NotOneOfAttribute _notOneOfAttribute = null!;
        private OneOfAttribute _oneOfAttribute = null!;
        private ValidEnumAttribute _validEnumAttribute = null!;
        private GreaterThanOrEqualToAttribute _greaterThanOrEqualToAttribute = null!;
        private LessThanOrEqualToAttribute _lessThanOrEqualToAttribute = null!;
        private MaxDigitsAttribute _maxDigitsAttribute = null!;
        private MinDigitsAttribute _minDigitsAttribute = null!;
        private NotContainElementAttribute _notContainElementAttribute = null!;
        private SingleOrNoneAttribute _singleOrNoneAttribute = null!;
        private DateRangeAttribute _dateRangeAttribute = null!;
        private MaxAgeAttribute _maxAgeAttribute = null!;
        private MinAgeAttribute _minAgeAttribute = null!;
        private NotInFutureAttribute _notInFutureAttribute = null!;
        private NotInPastAttribute _notInPastAttribute = null!;
        private NotLeapYearAttribute _notLeapYearAttribute = null!;
        private NotTodayDateAttribute _notTodayDateAttribute = null!;
        private NotUTCAttribute _notUTCAttribute = null!;
        private readonly List<int> _listNotContainingFive = new() { 1, 2, 3, 4 };
        private readonly List<int> _multipleElementsList = new() { 1, 2, 3 };
        private readonly List<int> _emptyList = new();
        private readonly string _validBase64 = "SGVsbG8gV29ybGQ=";
        private readonly string _invalidBase64 = "Invalid!@#";
        private readonly string _validAscii = "Hello World";
        private readonly string _invalidAscii = "Hello 世界";
        private readonly string _validFileExtension = "document.txt";
        private readonly string _invalidFileExtension = "document.exe";
        private readonly int _twoDigitNumber = 42;
        private readonly int _threeDigitNumber = 123;
        private readonly int _oneDigitNumber = 5;
        private readonly int _positiveNumber = 42;
        private readonly int _zeroNumber = 0;
        private readonly int _validAge = 25;
        private readonly int _invalidAge = 10;
        private readonly DateTime _today = System.DateTime.Today;
        private readonly DateTime _pastDate = System.DateTime.Now.AddDays(-30);
        private readonly DateTime _futureDate = System.DateTime.Now.AddDays(30);
        private readonly DateTime _validBirthDate = System.DateTime.Now.AddYears(-30);
        private readonly DateTime _invalidBirthDate = System.DateTime.Now.AddYears(-5);
        private readonly DateTime _leapYearDate = new(2024, 2, 29);
        private readonly DateTime _nonLeapYearDate = new(2023, 2, 28);
        private readonly DateTime _localDate = System.DateTime.Now;
        private readonly DateTime _utcDate = System.DateTime.UtcNow;

        [GlobalSetup]
        public void Setup()
        {
            _baseEncodingAttribute = new BaseEncodingAttribute(BaseEncodingAttribute.BaseType.Base64);
            _commonPrintableAttribute = new CommonPrintableAttribute();
            _fileExtensionAttribute = new FileExtensionAttribute(".txt");
            _notContainsAttribute = new NotContainsAttribute("forbidden");
            _notOneOfAttribute = new NotOneOfAttribute(new[] { "bad", "invalid" });
            _oneOfAttribute = new OneOfAttribute(new[] { "valid", "good" });
            _validEnumAttribute = new ValidEnumAttribute(typeof(DayOfWeek));
            _greaterThanOrEqualToAttribute = new GreaterThanOrEqualToAttribute(1);
            _lessThanOrEqualToAttribute = new LessThanOrEqualToAttribute(99);
            _maxDigitsAttribute = new MaxDigitsAttribute(10);
            _minDigitsAttribute = new MinDigitsAttribute(1);
            _notContainElementAttribute = new NotContainElementAttribute(99);
            _singleOrNoneAttribute = new SingleOrNoneAttribute(42);
            _dateRangeAttribute = new DateRangeAttribute(System.DateTime.Now.AddYears(-1), System.DateTime.Now.AddYears(1));
            _maxAgeAttribute = new MaxAgeAttribute(65);
            _minAgeAttribute = new MinAgeAttribute(18);
            _notInFutureAttribute = new NotInFutureAttribute();
            _notInPastAttribute = new NotInPastAttribute();
            _notLeapYearAttribute = new NotLeapYearAttribute();
            _notTodayDateAttribute = new NotTodayDateAttribute();
            _notUTCAttribute = new NotUTCAttribute();
        }

        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_BaseEncoding_Valid() => _baseEncodingAttribute.Validate(serviceProvider, "BaseEncoding", _validBase64).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_BaseEncoding_Invalid() => _baseEncodingAttribute.Validate(serviceProvider, "BaseEncoding", _invalidBase64).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_CommonPrintable_Valid() => _commonPrintableAttribute.Validate(serviceProvider, "CommonPrintable", _validAscii).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_CommonPrintable_Invalid() => _commonPrintableAttribute.Validate(serviceProvider, "CommonPrintable", _invalidAscii).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_FileExtension_Valid() => _fileExtensionAttribute.Validate(serviceProvider, "FileExtension", _validFileExtension).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_FileExtension_Invalid() => _fileExtensionAttribute.Validate(serviceProvider, "FileExtension", _invalidFileExtension).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotContains_Valid() => _notContainsAttribute.Validate(serviceProvider, "NotContains", "good content").IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotContains_Invalid() => _notContainsAttribute.Validate(serviceProvider, "NotContains", "forbidden content").IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotOneOf_Valid() => _notOneOfAttribute.Validate(serviceProvider, "NotOneOf", "good").IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotOneOf_Invalid() => _notOneOfAttribute.Validate(serviceProvider, "NotOneOf", "bad").IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_OneOf_Valid() => _oneOfAttribute.Validate(serviceProvider, "OneOf", "valid").IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_OneOf_Invalid() => _oneOfAttribute.Validate(serviceProvider, "OneOf", "invalid").IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_ValidEnum_Valid() => _validEnumAttribute.Validate(serviceProvider, "ValidEnum", "Monday").IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_ValidEnum_Invalid() => _validEnumAttribute.Validate(serviceProvider, "ValidEnum", "InvalidDay").IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_GreaterThanOrEqual_Valid() => _greaterThanOrEqualToAttribute.Validate(serviceProvider, "GreaterThanOrEqual", _positiveNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_GreaterThanOrEqual_Invalid() => _greaterThanOrEqualToAttribute.Validate(serviceProvider, "GreaterThanOrEqual", _zeroNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_LessThanOrEqual_Valid() => _lessThanOrEqualToAttribute.Validate(serviceProvider, "LessThanOrEqual", _validAge).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_LessThanOrEqual_Invalid() => _lessThanOrEqualToAttribute.Validate(serviceProvider, "LessThanOrEqual", 150).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MaxDigits_Valid() => _maxDigitsAttribute.Validate(serviceProvider, "MaxDigits", _twoDigitNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MaxDigits_Invalid() => _maxDigitsAttribute.Validate(serviceProvider, "MaxDigits", _threeDigitNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MinDigits_Valid() => _minDigitsAttribute.Validate(serviceProvider, "MinDigits", _twoDigitNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MinDigits_Invalid() => _minDigitsAttribute.Validate(serviceProvider, "MinDigits", _oneDigitNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotContainElement_Valid() => _notContainElementAttribute.Validate(serviceProvider, "NotContainElement", _listNotContainingFive).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotContainElement_Invalid() => _notContainElementAttribute.Validate(serviceProvider, "NotContainElement", new List<int> { 1, 99, 3 }).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_SingleOrNone_Valid() => _singleOrNoneAttribute.Validate(serviceProvider, "SingleOrNone", _emptyList).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_SingleOrNone_Invalid() => _singleOrNoneAttribute.Validate(serviceProvider, "SingleOrNone", _multipleElementsList).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_DateRange_Valid() => _dateRangeAttribute.Validate(serviceProvider, "DateRange", System.DateTime.Now).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_DateRange_Invalid() => _dateRangeAttribute.Validate(serviceProvider, "DateRange", System.DateTime.Now.AddYears(-2)).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MaxAge_Valid() => _maxAgeAttribute.Validate(serviceProvider, "MaxAge", _validBirthDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MaxAge_Invalid() => _maxAgeAttribute.Validate(serviceProvider, "MaxAge", System.DateTime.Now.AddYears(-70)).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MinAge_Valid() => _minAgeAttribute.Validate(serviceProvider, "MinAge", _validBirthDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MinAge_Invalid() => _minAgeAttribute.Validate(serviceProvider, "MinAge", _invalidBirthDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotInFuture_Valid() => _notInFutureAttribute.Validate(serviceProvider, "NotInFuture", _pastDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotInFuture_Invalid() => _notInFutureAttribute.Validate(serviceProvider, "NotInFuture", _futureDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotInPast_Valid() => _notInPastAttribute.Validate(serviceProvider, "NotInPast", _futureDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotInPast_Invalid() => _notInPastAttribute.Validate(serviceProvider, "NotInPast", _pastDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotLeapYear_Valid() => _notLeapYearAttribute.Validate(serviceProvider, "NotLeapYear", _nonLeapYearDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotLeapYear_Invalid() => _notLeapYearAttribute.Validate(serviceProvider, "NotLeapYear", _leapYearDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotTodayDate_Valid() => _notTodayDateAttribute.Validate(serviceProvider, "NotTodayDate", _pastDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotTodayDate_Invalid() => _notTodayDateAttribute.Validate(serviceProvider, "NotTodayDate", _today).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotUTC_Valid() => _notUTCAttribute.Validate(serviceProvider, "NotUTC", _localDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_NotUTC_Invalid() => _notUTCAttribute.Validate(serviceProvider, "NotUTC", _utcDate).IsValid;
    }
}
