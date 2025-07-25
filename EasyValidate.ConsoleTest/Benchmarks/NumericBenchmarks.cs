using BenchmarkDotNet.Attributes;
using EasyValidate.Core.Abstraction;
using EasyValidate.Core.Attributes;

namespace EasyValidate.Benchmarks
{
    [MemoryDiagnoser]
    public class NumericBenchmarks
    {
        public NumericBenchmarks()
        {
            serviceProvider = new DefaultServiceProvider();
        }
        private readonly IServiceProvider serviceProvider;
        private RangeAttribute _easyRangeAttribute = null!;
        private GreaterThanAttribute _greaterThanAttribute = null!;
        private LessThanAttribute _lessThanAttribute = null!;
        private PositiveAttribute _positiveAttribute = null!;
        private NegativeAttribute _negativeAttribute = null!;
        private EvenNumberAttribute _evenNumberAttribute = null!;
        private OddNumberAttribute _oddNumberAttribute = null!;
        private PrimeAttribute _primeAttribute = null!;
        private MultipleOfAttribute _multipleOfAttribute = null!;
        private DivisibleByAttribute _divisibleByAttribute = null!;
        private PowerOfAttribute _powerOfAttribute = null!;
        private FibonacciAttribute _fibonacciAttribute = null!;
        private readonly int _validAge = 25;
        private readonly int _invalidAge = 10;
        private readonly int _positiveNumber = 42;
        private readonly int _negativeNumber = -5;
        private readonly int _evenNumber = 8;
        private readonly int _oddNumber = 7;
        private readonly int _primeNumber = 17;
        private readonly int _nonPrimeNumber = 15;
        private readonly int _multipleOfFive = 25;
        private readonly int _notMultipleOfFive = 23;
        private readonly int _powerOfTwo = 16;
        private readonly int _notPowerOfTwo = 15;
        private readonly int _fibonacciNumber = 13;
        private readonly int _nonFibonacciNumber = 14;

        [GlobalSetup]
        public void Setup()
        {
            _easyRangeAttribute = new RangeAttribute(18, 120);
            _greaterThanAttribute = new GreaterThanAttribute(0);
            _lessThanAttribute = new LessThanAttribute(100);
            _positiveAttribute = new PositiveAttribute();
            _negativeAttribute = new NegativeAttribute();
            _evenNumberAttribute = new EvenNumberAttribute();
            _oddNumberAttribute = new OddNumberAttribute();
            _primeAttribute = new PrimeAttribute();
            _multipleOfAttribute = new MultipleOfAttribute(5);
            _divisibleByAttribute = new DivisibleByAttribute(2);
            _powerOfAttribute = new PowerOfAttribute(2);
            _fibonacciAttribute = new FibonacciAttribute();
        }

        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Range_Valid() => _easyRangeAttribute.Validate(serviceProvider, "Range", _validAge).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Range_Invalid() => _easyRangeAttribute.Validate(serviceProvider, "Range", _invalidAge).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_GreaterThan_Valid() => _greaterThanAttribute.Validate(serviceProvider, "GreaterThan", _positiveNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_GreaterThan_Invalid() => _greaterThanAttribute.Validate(serviceProvider, "GreaterThan", _negativeNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_LessThan_Valid() => _lessThanAttribute.Validate(serviceProvider, "LessThan", _validAge).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_LessThan_Invalid() => _lessThanAttribute.Validate(serviceProvider, "LessThan", 150).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Positive_Valid() => _positiveAttribute.Validate(serviceProvider, "Positive", _positiveNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Positive_Invalid() => _positiveAttribute.Validate(serviceProvider, "Positive", _negativeNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Negative_Valid() => _negativeAttribute.Validate(serviceProvider, "Negative", _negativeNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Negative_Invalid() => _negativeAttribute.Validate(serviceProvider, "Negative", _positiveNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Even_Valid() => _evenNumberAttribute.Validate(serviceProvider, "Even", _evenNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Even_Invalid() => _evenNumberAttribute.Validate(serviceProvider, "Even", _oddNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Odd_Valid() => _oddNumberAttribute.Validate(serviceProvider, "Odd", _oddNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Odd_Invalid() => _oddNumberAttribute.Validate(serviceProvider, "Odd", _evenNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Prime_Valid() => _primeAttribute.Validate(serviceProvider, "Prime", _primeNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Prime_Invalid() => _primeAttribute.Validate(serviceProvider, "Prime", _nonPrimeNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_MultipleOf_Valid() => _multipleOfAttribute.Validate(serviceProvider, "MultipleOf", _multipleOfFive).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_MultipleOf_Invalid() => _multipleOfAttribute.Validate(serviceProvider, "MultipleOf", _notMultipleOfFive).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_DivisibleBy_Valid() => _divisibleByAttribute.Validate(serviceProvider, "DivisibleBy", _evenNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_DivisibleBy_Invalid() => _divisibleByAttribute.Validate(serviceProvider, "DivisibleBy", _oddNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_PowerOf_Valid() => _powerOfAttribute.Validate(serviceProvider, "PowerOf", _powerOfTwo).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_PowerOf_Invalid() => _powerOfAttribute.Validate(serviceProvider, "PowerOf", _notPowerOfTwo).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Fibonacci_Valid() => _fibonacciAttribute.Validate(serviceProvider, "Fibonacci", _fibonacciNumber).IsValid;
        [Benchmark]
        [BenchmarkCategory("Numeric")]
        public bool EasyValidate_Fibonacci_Invalid() => _fibonacciAttribute.Validate(serviceProvider, "Fibonacci", _nonFibonacciNumber).IsValid;
    }
}
