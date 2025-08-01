using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Numerics.Positive;

public partial class PositiveModel
 : IValidate
{
    [Positive]
    public int Count { get; set; }

    [Optional, Positive]
    public decimal? Price { get; set; }

    [Positive]
    public double Rating { get; set; }

    [Positive]
    public float Score { get; set; }
}

public partial class PositiveNestedModel
 : IValidate
{
    [Positive]
    public int MainValue { get; set; }

    public PositiveModel? Details { get; set; }
}
