using EasyValidate.Attributes;

namespace EasyValidate.Test.SingleTests
{
    public partial class SingleModel
    {
        [Single<int>(2)]
        public IEnumerable<int>? Collection { get; set; }
    }
}
