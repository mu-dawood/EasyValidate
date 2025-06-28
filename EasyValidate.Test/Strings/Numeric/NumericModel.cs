using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.Strings.Numeric;

public partial class NumericModel
{
    [Numeric]
    public string Amount { get; set; } = string.Empty;

    [Numeric]
    public string Percentage { get; set; } = string.Empty;

    [Optional, Numeric]
    public string? OptionalValue { get; set; }
}

public partial class NumericNestedModel
{
    [Numeric]
    public string MainValue { get; set; } = string.Empty;

    public NumericModel? Details { get; set; }
}
