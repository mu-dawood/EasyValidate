using EasyValidate.Core.Abstraction;
using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.Dates.FutureDate;

public partial class FutureDateModel
{
    [Optional, FutureDate]
    public DateTime EventDate { get; set; }

    [Optional, FutureDate]
    public DateTime? DeadlineDate { get; set; }

    [FutureDate]
    public DateTime ScheduledDate { get; set; }
}

public partial class FutureDateNestedModel
{
    [FutureDate]
    public DateTime MainDate { get; set; }

    public FutureDateModel? Details { get; set; }
}
