using BenchmarkDotNet.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Attributes;

namespace ConsoleTest.Benchmarks
{
    [MemoryDiagnoser]
    public class CollectionBenchmarks
    {
        public CollectionBenchmarks()
        {
        }

        private LengthAttribute _easyLengthAttribute = null!;
        private MinLengthAttribute _easyMinLengthAttribute = null!;
        private MaxLengthAttribute _easyMaxLengthAttribute = null!;
        private HasElementsAttribute _hasElementsAttribute = null!;
        private UniqueElementsAttribute _uniqueElementsAttribute = null!;
        private NoNullElementsAttribute _noNullElementsAttribute = null!;
        private ContainsElementAttribute _containsElementAttribute = null!;
        private SingleAttribute _singleAttribute = null!;
        private readonly int[] _validArray = { 1, 2, 3, 4, 5 };
        private readonly int[] _invalidArray = System.Array.Empty<int>();
        private readonly int[] _exactLengthArray = { 1, 2, 3, 4, 5 };
        private readonly int[] _shortArray = { 1, 2 };
        private readonly int[] _longArray = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10, 11 };
        private readonly List<int> _uniqueList = new() { 1, 2, 3, 4, 5 };
        private readonly List<int> _duplicateList = new() { 1, 2, 2, 3, 4 };
        private readonly List<int> _singleElementList = new() { 42 };
        private readonly List<int> _multipleElementsList = new() { 1, 2, 3 };
        private readonly List<string?> _nullElementsList = new() { "a", null, "b" };
        private readonly List<string> _noNullElementsList = new() { "a", "b", "c" };
        private readonly List<int> _listContainingFive = new() { 1, 2, 5, 3, 4 };
        private readonly List<int> _listNotContainingFive = new() { 1, 2, 3, 4 };

        [GlobalSetup]
        public void Setup()
        {

            _easyLengthAttribute = new LengthAttribute(5);
            _easyMinLengthAttribute = new MinLengthAttribute(2);
            _easyMaxLengthAttribute = new MaxLengthAttribute(10);
            _hasElementsAttribute = new HasElementsAttribute();
            _uniqueElementsAttribute = new UniqueElementsAttribute();
            _noNullElementsAttribute = new NoNullElementsAttribute();
            _containsElementAttribute = new ContainsElementAttribute(5);
            _singleAttribute = new SingleAttribute(42);
        }

        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool Length_EasyValidate_Valid() => _easyLengthAttribute.Validate("Length", _exactLengthArray).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool Length_EasyValidate_Invalid() => _easyLengthAttribute.Validate("Length", _shortArray).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool MinLength_EasyValidate_Valid() => _easyMinLengthAttribute.Validate("MinLength", _validArray).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool MinLength_EasyValidate_Invalid() => _easyMinLengthAttribute.Validate("MinLength", _shortArray).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool MaxLength_EasyValidate_Valid() => _easyMaxLengthAttribute.Validate("MaxLength", _validArray).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool MaxLength_EasyValidate_Invalid() => _easyMaxLengthAttribute.Validate("MaxLength", _longArray).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool HasElements_EasyValidate_Valid() => _hasElementsAttribute.Validate("HasElements", _validArray).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool HasElements_EasyValidate_Invalid() => _hasElementsAttribute.Validate("HasElements", _invalidArray).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool UniqueElements_EasyValidate_Valid() => _uniqueElementsAttribute.Validate("UniqueElements", _uniqueList).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool UniqueElements_EasyValidate_Invalid() => _uniqueElementsAttribute.Validate("UniqueElements", _duplicateList).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool NoNullElements_EasyValidate_Valid() => _noNullElementsAttribute.Validate("NoNullElements", _noNullElementsList).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool NoNullElements_EasyValidate_Invalid() => _noNullElementsAttribute.Validate("NoNullElements", _nullElementsList).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool ContainsElement_EasyValidate_Valid() => _containsElementAttribute.Validate("ContainsElement", _listContainingFive).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool ContainsElement_EasyValidate_Invalid() => _containsElementAttribute.Validate("ContainsElement", _listNotContainingFive).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool Single_EasyValidate_Valid() => _singleAttribute.Validate("Single", _singleElementList).IsValid;
        [Benchmark]
        [BenchmarkCategory("Collection")]
        public bool Single_EasyValidate_Invalid() => _singleAttribute.Validate("Single", _multipleElementsList).IsValid;
    }
}
