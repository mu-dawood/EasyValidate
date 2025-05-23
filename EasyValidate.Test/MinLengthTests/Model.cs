using EasyValidate.Abstraction.Attributes;

namespace EasyValidate.Test.MinLengthTests
{
    public partial class MinLengthModel
    {
        [MinLength<int>(3)]
        public IEnumerable<int> MinLengthCollection { get; set; } = new List<int>();
    }
}
