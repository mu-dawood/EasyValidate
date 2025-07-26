using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Test.Strings.Hex
{
    public partial class HexModel
 : IValidate
    {
        [Hex]
        public string ColorCode { get; set; } = string.Empty;

        [Hex]
        public string HashValue { get; set; } = string.Empty;

        [Optional, Hex]
        public string? OptionalHex { get; set; }
    }

    public partial class HexNestedModel
 : IValidate
    {
        [Hex]
        public string PrimaryHex { get; set; } = string.Empty;

        public HexModel? NestedHex { get; set; }
    }
}
