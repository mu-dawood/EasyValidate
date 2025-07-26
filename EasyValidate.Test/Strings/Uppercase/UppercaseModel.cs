using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Test.Strings.Uppercase;

public partial class UppercaseModel
 : IValidate
{
    [Uppercase]
    public string Title { get; set; } = string.Empty;
    
    [Optional, Uppercase]
    public string? Code { get; set; }
    
    [Uppercase]
    public string Category { get; set; } = string.Empty;
}

public partial class UppercaseNestedModel
 : IValidate
{
    [Uppercase]
    public string MainCode { get; set; } = string.Empty;
    
    public UppercaseModel? Details { get; set; }
}
