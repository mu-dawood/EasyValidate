using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Strings.Ascii;

public partial class AsciiModel
 : IValidate
{
    [Ascii]
    public string Text { get; set; } = string.Empty;

    [Optional, Ascii]
    public string? Description { get; set; }

    [Ascii]
    public string Content { get; set; } = string.Empty;
}

public partial class AsciiNestedModel
 : IValidate
{
    [Ascii]
    public string MainText { get; set; } = string.Empty;

    public AsciiModel? Details { get; set; }
}
