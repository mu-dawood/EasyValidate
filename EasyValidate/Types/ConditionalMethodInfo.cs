namespace EasyValidate.Generator.Types;

internal class ConditionalMethodInfo(string methodName, bool isAsync)
{
    internal string MethodName { get; } = methodName;
    internal bool IsAsync { get; } = isAsync;

}
