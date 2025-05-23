using EasyValidate.Attributes;

namespace EasyValidate.Test.NotContainTests
{
    public partial class TestModel
    {
        [NotContain<int>(2)]
        public List<int> Values { get; set; } = [];

        [NotContain<char>('e')]
        public string Text { get; set; } = string.Empty;
    }
}
