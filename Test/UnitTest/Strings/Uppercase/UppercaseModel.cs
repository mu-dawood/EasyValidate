using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Strings.Uppercase;

public partial class UppercaseModel
 : IValidate, IGenerate
{
    [Uppercase]
    public string Title { get; set; } = string.Empty;

    [Optional, Uppercase]
    public string? Code { get; set; }

    [Uppercase]
    public string Category { get; set; } = string.Empty;
}

public partial class UppercaseNestedModel
 : IValidate, IGenerate
{
    [Uppercase]
    public string MainCode { get; set; } = string.Empty;

    public UppercaseModel? Details { get; set; }
}
