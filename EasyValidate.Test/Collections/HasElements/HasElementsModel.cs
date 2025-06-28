using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.Collections.HasElements;

public partial class HasElementsModel
{
    [NotNull, HasElements]
    public List<string>? StringList { get; set; }

    [NotNull, HasElements]
    public IEnumerable<int>? NumberEnumerable { get; set; }

    [NotNull, HasElements]
    public string[]? StringArray { get; set; }

    [NotNull, HasElements]
    public ICollection<object>? ObjectCollection { get; set; }

    [NotNull, HasElements]
    public HashSet<Guid>? GuidHashSet { get; set; }
}

public partial class HasElementsNestedModel
{
    [Optional, HasElements]
    public List<string>? MainList { get; set; }

    public HasElementsModel? Details { get; set; }
}
