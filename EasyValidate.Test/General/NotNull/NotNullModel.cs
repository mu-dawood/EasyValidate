using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.General.NotNull;

public partial class NotNullModel
 : IValidate
{
    [NotNull]
    public string? Name { get; set; }

    [NotNull]
    public object? Data { get; set; }
    [NotNull]
    public List<string>? Items { get; set; }
}

public partial class NotNullNestedModel
 : IValidate
{
    [NotNull]
    public string? MainProperty { get; set; }
    public NotNullModel? Details { get; set; }
}
