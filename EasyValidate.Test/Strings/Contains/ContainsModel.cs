using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.Strings.Contains;

public partial class ContainsModel
{
    [NotNull, Contains("test")]
    public string? BasicContains { get; set; }

    [Optional, Contains("CASE")]
    public string? CaseSensitive { get; set; }

    [Optional, Contains("required", Comparison = StringComparison.OrdinalIgnoreCase)] // Case insensitive
    public string? CaseInsensitive { get; set; }

    [Optional, Contains("@")]
    public string? EmailContains { get; set; }

    [Optional, Contains(" ")]
    public string? WhitespaceContains { get; set; }
}

public partial class ContainsNestedModel
{
    [Optional, Contains("main")]
    public string? MainProperty { get; set; }
    public ContainsModel? Details { get; set; }
}
