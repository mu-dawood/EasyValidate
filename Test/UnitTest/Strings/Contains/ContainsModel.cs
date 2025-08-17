using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Strings.Contains;

public partial class ContainsModel
 : IValidate, IGenerate
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
 : IValidate, IGenerate
{
    [Optional, Contains("main")]
    public string? MainProperty { get; set; }
    public ContainsModel? Details { get; set; }
}
