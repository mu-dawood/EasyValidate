using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Dates.PastDate;

public partial class PastDateModel : IValidate, IGenerate
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
 : IValidate, IGenerate
{
    [PastDate]
    public DateTime MainDate { get; set; }

    public PastDateModel? Details { get; set; }
}
