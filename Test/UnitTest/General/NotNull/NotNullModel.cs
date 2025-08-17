using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.General.NotNull;

public partial class NotNullModel
 : IValidate, IGenerate
{
    [NotNull]
    public string? Name { get; set; }

    [NotNull]
    public object? Data { get; set; }
    [NotNull]
    public List<string>? Items { get; set; }
}

public partial class NotNullNestedModel
 : IValidate, IGenerate
{
    [NotNull]
    public string? MainProperty { get; set; }
    public NotNullModel? Details { get; set; }
}
