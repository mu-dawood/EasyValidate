using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.Strings.Ascii;

public partial class AsciiModel
{
    [Ascii]
    public string Text { get; set; } = string.Empty;
    
    [Optional, Ascii]
    public string? Description { get; set; }
    
    [Ascii]
    public string Content { get; set; } = string.Empty;
}

public partial class AsciiNestedModel
{
    [Ascii]
    public string MainText { get; set; } = string.Empty;
    
    public AsciiModel? Details { get; set; }
}
