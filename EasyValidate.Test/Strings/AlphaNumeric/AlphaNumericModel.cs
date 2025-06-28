using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.Strings.AlphaNumeric;

public partial class AlphaNumericModel
{
    [AlphaNumeric]
    public string Username { get; set; } = string.Empty;

    [AlphaNumeric]
    public string ProductCode { get; set; } = string.Empty;

    [Optional, AlphaNumeric]
    public string? OptionalCode { get; set; }
}

public partial class AlphaNumericNestedModel
{
    [AlphaNumeric]
    public string MainCode { get; set; } = string.Empty;

    public AlphaNumericModel? Details { get; set; }
}
