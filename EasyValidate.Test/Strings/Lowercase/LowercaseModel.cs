using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.Strings.Lowercase;

public partial class LowercaseModel
{
    [Lowercase]
    public string Username { get; set; } = string.Empty;
    
    [Optional, Lowercase]
    public string? Email { get; set; }
    
    [Lowercase]
    public string Slug { get; set; } = string.Empty;
}

public partial class LowercaseNestedModel
{
    [Lowercase]
    public string MainSlug { get; set; } = string.Empty;
    
    public LowercaseModel? Details { get; set; }
}
