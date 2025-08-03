using System.Collections.Generic;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Types;

internal class ValidationTarget(INamedTypeSymbol symbol)
{

    internal INamedTypeSymbol Symbol { get; private set; } = symbol;
    internal IReadOnlyCollection<MemberInfo> Members { get; private set; } = [];
    internal IReadOnlyCollection<MethodTarget> Methods { get; private set; } = [];

    internal ValidationTarget WithMembers(List<MemberInfo> members)
    {
        return new ValidationTarget(Symbol)
        {
            Symbol = Symbol,
            Members = members,
            Methods = Methods,
        };
    }
    internal ValidationTarget WithMethods(List<MethodTarget> methods)
    {
        return new ValidationTarget(Symbol)
        {
            Symbol = Symbol,
            Members = Members,
            Methods = methods,
        };
    }

    internal bool NeedGeneration => Members.Count > 0 || Methods.Count > 0;
}

internal class MethodTarget(IMethodSymbol symbol, List<MemberInfo> parameters)
{
    internal IMethodSymbol Symbol { get; } = symbol;

    internal IReadOnlyCollection<MemberInfo> Parmters { get; } = parameters;

}
