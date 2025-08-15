namespace EasyValidate.Generator.Types;

internal class NestedConfig(bool isCollection, bool isAsync)
{
    internal bool IsCollection { get; } = isCollection;
    internal bool IsAsync { get; } = isAsync;
}
