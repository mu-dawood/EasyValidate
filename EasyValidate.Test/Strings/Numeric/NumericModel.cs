using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Strings.Numeric;

public partial class NumericModel
 : IValidate
{
    [Numeric]
    public string Amount { get; set; } = string.Empty;

    [Numeric]
    public string Percentage { get; set; } = string.Empty;

    [Optional, Numeric]
    public string? OptionalValue { get; set; }
}

public partial class NumericNestedModel
 : IValidate
{
    [Numeric]
    public string MainValue { get; set; } = string.Empty;

    public NumericModel? Details { get; set; }
}
