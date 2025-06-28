using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.Strings.EndsWith;

public partial class EndsWithModel
{
    [Optional, EndsWith(".com")]
    public string? Domain { get; set; }
    
    [Optional, EndsWith("_test")]
    public string? TestFile { get; set; }
    
    [Optional, EndsWith("!")]
    public string? Exclamation { get; set; }
    
    [Optional, EndsWith("@domain.org")]
    public string? Email { get; set; }
    
    [Optional, EndsWith(" ")]
    public string? EndsWithSpace { get; set; }
}

public partial class EndsWithNestedModel
{
    [Optional, EndsWith("_main")]
    public string? MainProperty { get; set; }
    
    public EndsWithModel? Details { get; set; }
}
