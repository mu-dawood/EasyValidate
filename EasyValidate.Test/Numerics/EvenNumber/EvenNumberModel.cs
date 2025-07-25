using EasyValidate.Core.Attributes;
using EasyValidate.Core.Abstraction;

namespace EasyValidate.Test.Numerics.EvenNumber;

public partial class EvenNumberModel
 : IValidate
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
 : IValidate
{
    [EvenNumber]
    public int MainValue { get; set; }
    
    public EvenNumberModel? Details { get; set; }
}
