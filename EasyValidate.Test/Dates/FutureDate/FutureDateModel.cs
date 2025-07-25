using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Test.Dates.FutureDate;

public partial class FutureDateModel
 : IValidate
{
    [FutureDate]
    public DateTime EventDate { get; set; }

    [Optional, FutureDate]
    public DateTime? DeadlineDate { get; set; }

    [FutureDate]
    public DateTime ScheduledDate { get; set; }
}

public partial class FutureDateNestedModel
 : IValidate
{
    [FutureDate]
    public DateTime MainDate { get; set; }

    public FutureDateModel? Details { get; set; }
}
