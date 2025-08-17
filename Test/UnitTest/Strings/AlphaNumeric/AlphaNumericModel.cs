using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Strings.AlphaNumeric;

public partial class AlphaNumericModel
 : IValidate, IGenerate
{
    [AlphaNumeric]
    public string Username { get; set; } = string.Empty;

    [AlphaNumeric]
    public string ProductCode { get; set; } = string.Empty;

    [Optional, AlphaNumeric]
    public string? OptionalCode { get; set; }
}

public partial class AlphaNumericNestedModel
 : IValidate, IGenerate
{
    [AlphaNumeric]
    public string MainCode { get; set; } = string.Empty;

    public AlphaNumericModel? Details { get; set; }
}
