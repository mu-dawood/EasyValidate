using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.Strings.Hex
{
    public partial class HexModel
    {
        [Hex]
        public string ColorCode { get; set; } = string.Empty;

        [Hex]
        public string HashValue { get; set; } = string.Empty;

        [Optional, Hex]
        public string? OptionalHex { get; set; }
    }

    public partial class HexNestedModel
    {
        [Hex]
        public string PrimaryHex { get; set; } = string.Empty;

        public HexModel? NestedHex { get; set; }
    }
}
