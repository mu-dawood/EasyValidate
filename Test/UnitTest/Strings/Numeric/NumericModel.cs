using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Strings.Numeric;

public partial class NumericModel
 : IValidate, IGenerate
{
    [Numeric]
    public string Amount { get; set; } = string.Empty;

    [Numeric]
    public string Percentage { get; set; } = string.Empty;

    [Optional, Numeric]
    public string? OptionalValue { get; set; }
}

public partial class NumericNestedModel
 : IValidate, IGenerate
{
    [Numeric]
    public string MainValue { get; set; } = string.Empty;

    public NumericModel? Details { get; set; }
}
