using EasyValidate.Attributes;

namespace EasyValidate.Test.LengthTests
{
    public partial class LengthModel
    {
        [Length<int>(5)]
        public IEnumerable<int> FixedLengthCollection { get; set; } = new List<int>();
    }
}
