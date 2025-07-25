using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Test.Dates.PastDate;

public partial class PastDateModel
{
    [PastDate]
    public DateOnly EventDate { get; set; }

    [Optional, PastDate]
    public DateTime? OptionalPastDate { get; set; }

    [PastDate]
    public DateOnly BirthDate { get; set; }

    [Optional, PastDate]
    public DateOnly? OptionalDateOnly { get; set; }

    [PastDate]
    public DateTimeOffset CreatedAt { get; set; }
}

public partial class PastDateNestedModel
 : IValidate
{
    [PastDate]
    public DateTime MainDate { get; set; }

    public PastDateModel? Details { get; set; }
}
