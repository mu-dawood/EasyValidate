using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.Strings.StartsWith;

public partial class StartsWithModel
{
    [Optional, StartsWith("Hello")]
    public string? Greeting { get; set; }
    
    [Optional, StartsWith("https://")]
    public string? SecureUrl { get; set; }
    
    [Optional, StartsWith("Mr.")]
    public string? FormalTitle { get; set; }
    
    [Optional, StartsWith("0")]
    public string? NumberString { get; set; }
    
    [Optional, StartsWith(" ")]
    public string? StartsWithSpace { get; set; }
}

public partial class StartsWithNestedModel
{
    [Optional, StartsWith("Main")]
    public string? MainProperty { get; set; }
    
    public StartsWithModel? Details { get; set; }
}
