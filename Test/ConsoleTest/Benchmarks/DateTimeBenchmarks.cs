using BenchmarkDotNet.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Attributes;

namespace ConsoleTest.Benchmarks
{
    [MemoryDiagnoser]
    public class DateTimeBenchmarks
    {
        public DateTimeBenchmarks()
        {
        }
        private FutureDateAttribute _futureDateAttribute = null!;
        private PastDateAttribute _pastDateAttribute = null!;
        private TodayAttribute _todayAttribute = null!;
        private LeapYearAttribute _leapYearAttribute = null!;
        private AgeRangeAttribute _easyAgeRangeAttribute = null!;
        private DayOfWeekAttribute _dayOfWeekAttribute = null!;
        private MonthAttribute _monthAttribute = null!;
        private YearAttribute _yearAttribute = null!;
        private DayAttribute _dayAttribute = null!;
        private QuarterAttribute _quarterAttribute = null!;
        private UTCAttribute _utcAttribute = null!;
        private TimeRangeAttribute _timeRangeAttribute = null!;
        private readonly DateTime _futureDate = System.DateTime.Now.AddDays(30);
        private readonly DateTime _pastDate = System.DateTime.Now.AddDays(-30);
        private readonly DateTime _today = System.DateTime.Today;
        private readonly DateTime _validBirthDate = System.DateTime.Now.AddYears(-30);
        private readonly DateTime _invalidBirthDate = System.DateTime.Now.AddYears(-5);
        private readonly DateTime _leapYearDate = new(2024, 2, 29);
        private readonly DateTime _nonLeapYearDate = new(2023, 2, 28);
        private readonly DateTime _mondayDate = new(2024, 7, 8);
        private readonly DateTime _tuesdayDate = new(2024, 7, 9);
        private readonly DateTime _juneDate = new(2024, 6, 15);
        private readonly DateTime _februaryDate = new(2024, 2, 15);
        private readonly DateTime _q2Date = new(2024, 5, 15);
        private readonly DateTime _q3Date = new(2024, 8, 15);
        private readonly DateTime _2024Date = new(2024, 6, 15);
        private readonly DateTime _2023Date = new(2023, 6, 15);
        private readonly DateTime _day15Date = new(2024, 6, 15);
        private readonly DateTime _day20Date = new(2024, 6, 20);
        private readonly DateTime _utcDate = System.DateTime.UtcNow;
        private readonly DateTime _localDate = System.DateTime.Now;
        private readonly DateTime _validWorkTimeDate = new(2024, 6, 15, 10, 30, 0);
        private readonly DateTime _invalidWorkTimeDate = new(2024, 6, 15, 20, 30, 0);

        [GlobalSetup]
        public void Setup()
        {
            _futureDateAttribute = new FutureDateAttribute();
            _pastDateAttribute = new PastDateAttribute();
            _todayAttribute = new TodayAttribute();
            _leapYearAttribute = new LeapYearAttribute();
            _easyAgeRangeAttribute = new AgeRangeAttribute(18, 65);
            _dayOfWeekAttribute = new DayOfWeekAttribute(System.DayOfWeek.Monday);
            _monthAttribute = new MonthAttribute(6);
            _yearAttribute = new YearAttribute(2024);
            _dayAttribute = new DayAttribute(15);
            _quarterAttribute = new QuarterAttribute(Quarter.Q2);
            _utcAttribute = new UTCAttribute();
            _timeRangeAttribute = new TimeRangeAttribute(System.TimeSpan.FromHours(9), System.TimeSpan.FromHours(17));
        }

        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool FutureDate_EasyValidate_Valid() => _futureDateAttribute.Validate("FutureDate", _futureDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool FutureDate_EasyValidate_Invalid() => _futureDateAttribute.Validate("FutureDate", _pastDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool PastDate_EasyValidate_Valid() => _pastDateAttribute.Validate("PastDate", _pastDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool PastDate_EasyValidate_Invalid() => _pastDateAttribute.Validate("PastDate", _futureDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Today_EasyValidate_Valid() => _todayAttribute.Validate("Today", _today).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Today_EasyValidate_Invalid() => _todayAttribute.Validate("Today", _pastDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool LeapYear_EasyValidate_Valid() => _leapYearAttribute.Validate("LeapYear", _leapYearDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool LeapYear_EasyValidate_Invalid() => _leapYearAttribute.Validate("LeapYear", _nonLeapYearDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool AgeRange_EasyValidate_Valid() => _easyAgeRangeAttribute.Validate("AgeRange", _validBirthDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool AgeRange_EasyValidate_Invalid() => _easyAgeRangeAttribute.Validate("AgeRange", _invalidBirthDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool DayOfWeek_EasyValidate_Valid() => _dayOfWeekAttribute.Validate("DayOfWeek", _mondayDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool DayOfWeek_EasyValidate_Invalid() => _dayOfWeekAttribute.Validate("DayOfWeek", _tuesdayDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Month_EasyValidate_Valid() => _monthAttribute.Validate("Month", _juneDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Month_EasyValidate_Invalid() => _monthAttribute.Validate("Month", _februaryDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Year_EasyValidate_Valid() => _yearAttribute.Validate("Year", _2024Date).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Year_EasyValidate_Invalid() => _yearAttribute.Validate("Year", _2023Date).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Day_EasyValidate_Valid() => _dayAttribute.Validate("Day", _day15Date).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Day_EasyValidate_Invalid() => _dayAttribute.Validate("Day", _day20Date).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Quarter_EasyValidate_Valid() => _quarterAttribute.Validate("Quarter", _q2Date).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool Quarter_EasyValidate_Invalid() => _quarterAttribute.Validate("Quarter", _q3Date).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool UTC_EasyValidate_Valid() => _utcAttribute.Validate("UTC", _utcDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool UTC_EasyValidate_Invalid() => _utcAttribute.Validate("UTC", _localDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool TimeRange_EasyValidate_Valid() => _timeRangeAttribute.Validate("TimeRange", _validWorkTimeDate).IsValid;
        [Benchmark]
        [BenchmarkCategory("DateTime")]
        public bool TimeRange_EasyValidate_Invalid() => _timeRangeAttribute.Validate("TimeRange", _invalidWorkTimeDate).IsValid;
    }
}
