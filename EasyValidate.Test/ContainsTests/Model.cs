using EasyValidate.Attributes;

namespace EasyValidate.Test.ContainsTests
{
    public partial class ContainsModel
    {
        [Contains<int>(3)]
        public required List<int> Items { get; set; }
    }
}
