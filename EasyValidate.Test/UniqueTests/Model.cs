using EasyValidate.Attributes;

namespace EasyValidate.Test.UniqueTests
{
    public partial class UniqueModel
    {
        [Unique<string>("Value1")]
        public IEnumerable<string> StringCollection { get; set; } = [];

        [Unique<int>(10)]
        public IEnumerable<int> IntCollection { get; set; } = [];

        [Unique<double>(3.14)]
        public IEnumerable<double> DoubleCollection { get; set; } = [];
    }
}
