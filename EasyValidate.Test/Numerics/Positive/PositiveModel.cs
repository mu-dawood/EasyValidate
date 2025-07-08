using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.Numerics.Positive;

public partial class PositiveModel
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
{
    [Positive]
    public int MainValue { get; set; }
    
    public PositiveModel? Details { get; set; }
}
