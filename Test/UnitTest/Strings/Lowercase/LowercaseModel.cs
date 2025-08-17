using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Strings.Lowercase;

public partial class LowercaseModel
 : IValidate, IGenerate
{
    [Lowercase]
    public string Username { get; set; } = string.Empty;

    [Optional, Lowercase]
    public string? Email { get; set; }

    [Lowercase]
    public string Slug { get; set; } = string.Empty;
}

public partial class LowercaseNestedModel
 : IValidate, IGenerate
{
    [Lowercase]
    public string MainSlug { get; set; } = string.Empty;

    public LowercaseModel? Details { get; set; }
}
