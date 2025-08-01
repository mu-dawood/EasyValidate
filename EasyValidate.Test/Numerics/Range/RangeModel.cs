using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Numerics.Range;

public partial class RangeModel
 : IValidate
{
    [Range(1, 100)]
    public int Age { get; set; }

    [Optional, Range(0.0, 5.0)]
    public decimal? Rating { get; set; }

    [Range(-10, 10)]
    public double Temperature { get; set; }

    [Range(0, 1000)]
    public float Score { get; set; }
}

public partial class RangeNestedModel
 : IValidate
{
    [Range(1, 50)]
    public int MainValue { get; set; }

    public RangeModel? Details { get; set; }
}
