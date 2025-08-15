
using EasyValidate.Abstractions;

namespace EasyValidate.ConsoleTest.Video;

public class CutsomAttribute : IValidationAttribute<string>
{
    public string ErrorCode => throw new NotImplementedException();

    public string? ConditionalMethod => throw new NotImplementedException();

    public string Chain => throw new NotImplementedException();
}