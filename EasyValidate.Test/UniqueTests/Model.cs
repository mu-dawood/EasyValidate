using EasyValidate.Abstraction.Attributes;

namespace EasyValidate.Test.UniqueTests
{
    public partial class UniqueModel
    {
        [Unique<string>("Value1")]
        public IEnumerable<string> StringCollection { get; set; } = new List<string>();

        [Unique<int>(10)]
        public IEnumerable<int> IntCollection { get; set; } = new List<int>();

        [Unique<double>(3.14)]
        public IEnumerable<double> DoubleCollection { get; set; } = new List<double>();
    }
}
