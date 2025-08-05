using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Types;

internal class ValidationTarget(INamedTypeSymbol symbol)
{

    internal INamedTypeSymbol Symbol { get; private set; } = symbol;
    internal IReadOnlyCollection<MemberInfo> Members { get; private set; } = [];
    private List<string> _awaitableMembers = [];
    internal IReadOnlyCollection<string> AwaitableMembers => _awaitableMembers;
    internal IReadOnlyCollection<MethodTarget> Methods { get; private set; } = [];

    internal ValidationTarget WithMembers(IEnumerable<MemberInfo> members)
    {
        return new ValidationTarget(Symbol)
        {
            Symbol = Symbol,
            Members = [.. members],
            Methods = Methods,
        };
    }
    internal ValidationTarget WithMethods(IEnumerable<MethodTarget> methods)
    {
        return new ValidationTarget(Symbol)
        {
            Symbol = Symbol,
            Members = Members,
            Methods = [.. methods],
        };
    }
    internal ValidationTarget WithAwaitableMembers(IEnumerable<string> awaitableMembers)
    {
        return new ValidationTarget(Symbol)
        {
            Symbol = Symbol,
            Members = Members,
            Methods = Methods,
            _awaitableMembers = [.. awaitableMembers],
        };
    }

    internal bool NeedGeneration => Members.Count > 0 || Methods.Count > 0;
}

internal class MethodTarget(IMethodSymbol symbol, List<MemberInfo> parameters)
{
    internal IMethodSymbol Symbol { get; } = symbol;

    private List<string> _awaitableMembers = [];
    internal IReadOnlyCollection<string> AwaitableMembers => _awaitableMembers;
    internal IReadOnlyCollection<MemberInfo> Parmters { get; } = parameters;
    internal MethodTarget WithAwaitableMembers(IEnumerable<string> awaitableMembers)
    {
        return new MethodTarget(Symbol, parameters)
        {
            _awaitableMembers = [.. awaitableMembers],
        };
    }

}
