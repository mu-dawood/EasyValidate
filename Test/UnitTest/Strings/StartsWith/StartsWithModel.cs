using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Strings.StartsWith;

public partial class StartsWithModel
 : IValidate, IGenerate
{
    [NotNull, StartsWith("Hello")]
    public string? Greeting { get; set; }

    [Optional, StartsWith("https://")]
    public string? SecureUrl { get; set; }

    [Optional, StartsWith("Mr.")]
    public string? FormalTitle { get; set; }

    [Optional, StartsWith("0")]
    public string? NumberString { get; set; }

    [Optional, StartsWith(" ")]
    public string? StartsWithSpace { get; set; }
}

public partial class StartsWithNestedModel
 : IValidate, IGenerate
{
    [Optional, StartsWith("Main")]
    public string? MainProperty { get; set; }

    public StartsWithModel? Details { get; set; }
}
