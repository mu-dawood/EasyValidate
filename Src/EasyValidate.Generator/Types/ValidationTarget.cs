using System.Collections.Generic;
using System.Linq;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Generator.Types;

internal class ValidationTarget(INamedTypeSymbol symbol)
{

    internal INamedTypeSymbol Symbol { get; private set; } = symbol;
    internal IReadOnlyCollection<Member> Members { get; private set; } = [];
    private List<string> _awaitableMembers = [];
    internal IReadOnlyCollection<string> AwaitableMembers => _awaitableMembers;
    internal IReadOnlyCollection<MethodTarget> Methods { get; private set; } = [];

    internal ValidationTarget WithMembers(IEnumerable<Member> members)
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
    internal void SetAwaitableMembers(IEnumerable<string> awaitableMembers)
    {
        _awaitableMembers = [.. awaitableMembers];
    }

    internal bool NeedGeneration => Members.Count > 0 || Methods.Count > 0;
}

internal class MethodTarget(IMethodSymbol symbol, List<Member> parameters)
{
    internal IMethodSymbol Symbol { get; } = symbol;

    private List<string> _awaitableMembers = [];
    internal IReadOnlyCollection<string> AwaitableMembers => _awaitableMembers;
    /// <summary>
    /// All parameters of the method. Use NeedsValidation flag to check if validation is required.
    /// </summary>
    internal IReadOnlyCollection<Member> Parmters { get; } = parameters;
    /// <summary>
    /// Parameters that require validation (have validation attributes or nested config).
    /// </summary>
    internal IEnumerable<Member> ValidatedParameters => Parmters.Where(p => p.NeedsValidation);

    internal void SetAwaitableMembers(IEnumerable<string> awaitableMembers)
    {
       _awaitableMembers = [.. awaitableMembers];
    }

}
