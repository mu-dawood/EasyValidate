using EasyValidate.Core.Attributes;

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
{
    [PastDate]
    public DateTime MainDate { get; set; }

    public PastDateModel? Details { get; set; }
}
