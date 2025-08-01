namespace EasyValidate.Types;

internal class NestedConfig(bool isCollection, bool isAsync)
{
    public bool IsCollection { get; } = isCollection;
    public bool IsAsync { get; } = isAsync;
}
