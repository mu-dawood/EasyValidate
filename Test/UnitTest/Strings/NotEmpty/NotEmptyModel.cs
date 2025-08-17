using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Strings.NotEmpty;

public partial class NotEmptyModel
 : IValidate, IGenerate
{
    [NotNull, NotEmpty]
    public string? Name { get; set; } = string.Empty;

    [NotEmpty]
    public string Email { get; set; } = string.Empty;

    [NotNull, NotEmpty]
    public string? Description { get; set; }
}

public partial class NotEmptyNestedModel
 : IValidate, IGenerate
{
    [NotEmpty]
    public string Title { get; set; } = string.Empty;

    public NotEmptyModel? Details { get; set; }
}
