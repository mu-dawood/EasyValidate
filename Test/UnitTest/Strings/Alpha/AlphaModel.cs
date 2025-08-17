using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Strings.Alpha;

public partial class AlphaModel
 : IValidate, IGenerate
{
    [Alpha]
    public string FirstName { get; set; } = string.Empty;

    [Optional, Alpha]
    public string? LastName { get; set; }

    [Alpha]
    public string MiddleName { get; set; } = string.Empty;
}

public partial class AlphaNestedModel
 : IValidate, IGenerate
{
    [Alpha]
    public string Name { get; set; } = string.Empty;

    public AlphaModel? NestedModel { get; set; }
}
