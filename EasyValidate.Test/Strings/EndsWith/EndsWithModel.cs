using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Test.Strings.EndsWith;

public partial class EndsWithModel
 : IValidate
{
    [NotNull, EndsWith(".com")]
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
 : IValidate
{
    [Optional, EndsWith("_main")]
    public string? MainProperty { get; set; }
    
    public EndsWithModel? Details { get; set; }
}
