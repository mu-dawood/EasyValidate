using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.General.NotNull;

public partial class NotNullModel
{
    [NotNull]
    public string? Name { get; set; }
    
    [NotNull]
    public object? Data { get; set; }
    [NotNull]
    public List<string>? Items { get; set; }
}

public partial class NotNullNestedModel
{
    [NotNull]
    public string? MainProperty { get; set; }
    public NotNullModel? Details { get; set; }
}
