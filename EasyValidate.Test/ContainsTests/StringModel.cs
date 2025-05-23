using EasyValidate.Attributes;

namespace EasyValidate.Test.ContainsTests
{
    public partial class ContainsStringModel
    {
        [Contains<char>('H')]
        public string Text { get; set; } = string.Empty;
    }
}
