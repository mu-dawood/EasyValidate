using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.Strings.Alpha;

public partial class AlphaModel
{
    [Alpha]
    public string FirstName { get; set; } = string.Empty;

    [Optional, Alpha]
    public string? LastName { get; set; }

    [Alpha]
    public string MiddleName { get; set; } = string.Empty;
}

public partial class AlphaNestedModel
{
    [Alpha]
    public string Name { get; set; } = string.Empty;

    public AlphaModel? NestedModel { get; set; }
}
