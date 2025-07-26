namespace EasyValidate.Types;

internal class ConditionalMethodInfo(string methodName, bool isAsync)
{
    public string MethodName { get; } = methodName;
    public bool IsAsync { get; } = isAsync;

}
