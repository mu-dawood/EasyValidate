using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Strings.Hex
{
    public partial class HexModel
 : IValidate, IGenerate
    {
        [Hex]
        public string ColorCode { get; set; } = string.Empty;

        [Hex]
        public string HashValue { get; set; } = string.Empty;

        [Optional, Hex]
        public string? OptionalHex { get; set; }
    }

    public partial class HexNestedModel
 : IValidate, IGenerate
    {
        [Hex]
        public string PrimaryHex { get; set; } = string.Empty;

        public HexModel? NestedHex { get; set; }
    }
}
