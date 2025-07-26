using BenchmarkDotNet.Attributes;
using EasyValidate.Core.Abstraction;
using EasyValidate.Core.Attributes;

namespace EasyValidate.Benchmarks
{
    [MemoryDiagnoser]
    public class GeneralBenchmarks
    {
        public GeneralBenchmarks()
        {
            serviceProvider = new DefaultServiceProvider();
        }
        private readonly IServiceProvider serviceProvider;
        private NotNullAttribute _notNullAttribute = null!;
        private NotDefaultAttribute _notDefaultAttribute = null!;
        private OptionalAttribute _optionalAttribute = null!;
        private EqualToAttribute _equalToAttribute = null!;
        private NotEqualToAttribute _notEqualToAttribute = null!;
        private readonly string _validName = "John Doe";
        private readonly string? _invalidName = null;
        private readonly string _validValue = "test";
        private readonly string _invalidValue = "invalid";

        [GlobalSetup]
        public void Setup()
        {
            _notNullAttribute = new NotNullAttribute();
            _notDefaultAttribute = new NotDefaultAttribute();
            _optionalAttribute = new OptionalAttribute();
            _equalToAttribute = new EqualToAttribute("test");
            _notEqualToAttribute = new NotEqualToAttribute("test");
        }

        [Benchmark]
        [BenchmarkCategory("General")]
        public bool Required_EasyValidate_Valid() => _notNullAttribute.Validate(serviceProvider, "Name", _validName).IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool Required_EasyValidate_Invalid() => _notNullAttribute.Validate(serviceProvider, "Name", _invalidName).IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool NotDefault_EasyValidate_Valid() => _notDefaultAttribute.Validate(serviceProvider, "Name", _validName).IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool NotDefault_EasyValidate_Invalid() => _notDefaultAttribute.Validate(serviceProvider, "Name", "").IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool Optional_EasyValidate_Valid() => _optionalAttribute.Validate(serviceProvider, "Name", null).IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool Optional_EasyValidate_Invalid() => _optionalAttribute.Validate(serviceProvider, "Name", "").IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool EqualTo_EasyValidate_Valid() => _equalToAttribute.Validate(serviceProvider, "Value", _validValue).IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool EqualTo_EasyValidate_Invalid() => _equalToAttribute.Validate(serviceProvider, "Value", _invalidValue).IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool NotEqualTo_EasyValidate_Valid() => _notEqualToAttribute.Validate(serviceProvider, "Value", "different").IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool NotEqualTo_EasyValidate_Invalid() => _notEqualToAttribute.Validate(serviceProvider, "Value", "test").IsValid;
    }
}
