using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Dates.FutureDate;

public partial class FutureDateModel
 : IValidate, IGenerate
{
    [FutureDate]
    public DateTime EventDate { get; set; }

    [Optional, FutureDate]
    public DateTime? DeadlineDate { get; set; }

    [FutureDate]
    public DateTime ScheduledDate { get; set; }


}

public partial class FutureDateNestedModel
 : IValidate, IGenerate
{
    [FutureDate]
    public DateTime MainDate { get; set; }

    public FutureDateModel? Details { get; set; }
}
