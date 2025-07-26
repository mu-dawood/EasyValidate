using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Test.Strings.Contains;

public partial class ContainsModel
 : IValidate
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
 : IValidate
{
    [Optional, Contains("main")]
    public string? MainProperty { get; set; }
    public ContainsModel? Details { get; set; }
}
