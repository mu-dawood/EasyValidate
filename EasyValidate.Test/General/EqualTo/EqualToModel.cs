using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.General.EqualTo;

public partial class EqualToModel
{
    [EqualTo("ExpectedValue")]
    public string? StringProperty { get; set; }
    
    [EqualTo(42)]
    public int IntProperty { get; set; }
    
    [EqualTo(3.14)]
    public double DoubleProperty { get; set; }
    
    [EqualTo(true)]
    public bool BoolProperty { get; set; }
    
    [EqualTo(null)]
    public string? NullableProperty { get; set; }
}

public partial class EqualToNestedModel
{
    [EqualTo("MainValue")]
    public string? MainProperty { get; set; }
    
    public EqualToModel? Details { get; set; }
}
