using EasyValidate.Attributes;
using EasyValidate.Abstractions;
using EasyValidate.Test.Extensions;

using System.Linq;
namespace EasyValidate.Test.Numerics.Negative;

public partial class NegativeModel
 : IValidate, IGenerate
{
    [Negative]
    public int IntValue { get; set; } = -1;

    [Negative]
    public double DoubleValue { get; set; } = -1;

    [Negative]
    public decimal DecimalValue { get; set; } = -1;

    [Negative]
    public float FloatValue { get; set; } = -1;

    [Negative]
    public long LongValue { get; set; } = -1;

    [Optional, Negative]
    public int? NullableInt { get; set; }
}

public partial class NegativeNestedModel
 : IValidate, IGenerate
{
    [Negative]
    public int MainValue { get; set; }

    public NegativeModel? Details { get; set; }
}
