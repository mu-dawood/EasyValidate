using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Test.Strings.Alpha;

public partial class AlphaModel
 : IValidate
{
    [Alpha]
    public string FirstName { get; set; } = string.Empty;

    [Optional, Alpha]
    public string? LastName { get; set; }

    [Alpha]
    public string MiddleName { get; set; } = string.Empty;
}

public partial class AlphaNestedModel
 : IValidate
{
    [Alpha]
    public string Name { get; set; } = string.Empty;

    public AlphaModel? NestedModel { get; set; }
}
