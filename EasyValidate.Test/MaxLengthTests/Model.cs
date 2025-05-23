using EasyValidate.Attributes;

namespace EasyValidate.Test.MaxLengthTests
{
    public partial class MaxLengthModel
    {
        [MaxLength<int>(10)]
        public IEnumerable<int> MaxLengthCollection { get; set; } = [];
    }
}
