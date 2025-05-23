using System;
using EasyValidate.Abstraction.Attributes;

namespace EasyValidate.ConsoleTest;

public partial class Dto
{
    [NotNull]
    public string Name { get; set; } = string.Empty;
    [Range(0, 100)]
    public int Age { get; set; }

}



public class XXX : ValidationAttributeBase
{
    public override string ErrorCode => throw new NotImplementedException();

    public string Validate(string propertyName, object? value)
    {
        throw new NotImplementedException();
    }
}
