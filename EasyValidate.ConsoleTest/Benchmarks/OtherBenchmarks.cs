using BenchmarkDotNet.Attributes;
using EasyValidate.Core.Attributes;

namespace EasyValidate.Benchmarks
{
    [MemoryDiagnoser]
    public class OtherBenchmarks
    {
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
        public bool EasyValidate_DateRange_Valid() => _dateRangeAttribute.Validate(this, "DateRange", System.DateTime.Now).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_DateRange_Invalid() => _dateRangeAttribute.Validate(this, "DateRange", System.DateTime.Now.AddYears(-2)).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MaxAge_Valid() => _maxAgeAttribute.Validate(this, "MaxAge", _validBirthDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("Other")]
        public bool EasyValidate_MaxAge_Invalid() => _maxAgeAttribute.Validate(this, "MaxAge", System.DateTime.Now.AddYears(-70)).IsValid;
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
    }
}
