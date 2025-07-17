using BenchmarkDotNet.Attributes;
using EasyValidate.Core.Attributes;

namespace EasyValidate.Benchmarks
{
    [MemoryDiagnoser]
    public class GeneralBenchmarks
    {
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
        public bool Required_EasyValidate_Valid() => _notNullAttribute.Validate(this, "Name", _validName).IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool Required_EasyValidate_Invalid() => _notNullAttribute.Validate(this, "Name", _invalidName).IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool NotDefault_EasyValidate_Valid() => _notDefaultAttribute.Validate(this, "Name", _validName).IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool NotDefault_EasyValidate_Invalid() => _notDefaultAttribute.Validate(this, "Name", "").IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool Optional_EasyValidate_Valid() => _optionalAttribute.Validate(this, "Name", null).IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool Optional_EasyValidate_Invalid() => _optionalAttribute.Validate(this, "Name", "").IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool EqualTo_EasyValidate_Valid() => _equalToAttribute.Validate(this, "Value", _validValue).IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool EqualTo_EasyValidate_Invalid() => _equalToAttribute.Validate(this, "Value", _invalidValue).IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool NotEqualTo_EasyValidate_Valid() => _notEqualToAttribute.Validate(this, "Value", "different").IsValid;
        [Benchmark]
        [BenchmarkCategory("General")]
        public bool NotEqualTo_EasyValidate_Invalid() => _notEqualToAttribute.Validate(this, "Value", "test").IsValid;
    }
}
