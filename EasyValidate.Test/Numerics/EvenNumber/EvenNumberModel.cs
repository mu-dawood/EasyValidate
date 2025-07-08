using EasyValidate.Core.Attributes;

namespace EasyValidate.Test.Numerics.EvenNumber;

public partial class EvenNumberModel
{
    [EvenNumber]
    public int IntValue { get; set; }
    
    [EvenNumber]
    public long LongValue { get; set; }
    
    [EvenNumber]
    public short ShortValue { get; set; }
    
    [EvenNumber]
    public byte ByteValue { get; set; }
    
    [Optional, EvenNumber]
    public int? NullableInt { get; set; }
}

public partial class EvenNumberNestedModel
{
    [EvenNumber]
    public int MainValue { get; set; }
    
    public EvenNumberModel? Details { get; set; }
}
