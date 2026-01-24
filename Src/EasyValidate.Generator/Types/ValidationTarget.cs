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
    internal IReadOnlyCollection<ConstructorTarget> Constructors { get; private set; } = [];

    internal ValidationTarget WithMembers(IEnumerable<Member> members)
    {
        return new ValidationTarget(Symbol)
        {
            Symbol = Symbol,
            Members = [.. members],
            Methods = Methods,
            Constructors = Constructors,
        };
    }
    internal ValidationTarget WithMethods(IEnumerable<MethodTarget> methods)
    {
        return new ValidationTarget(Symbol)
        {
            Symbol = Symbol,
            Members = Members,
            Methods = [.. methods],
            Constructors = Constructors,
        };
    }
    internal ValidationTarget WithConstructors(IEnumerable<ConstructorTarget> constructors)
    {
        return new ValidationTarget(Symbol)
        {
            Symbol = Symbol,
            Members = Members,
            Methods = Methods,
            Constructors = [.. constructors],
        };
    }
    internal void SetAwaitableMembers(IEnumerable<string> awaitableMembers)
    {
        _awaitableMembers = [.. awaitableMembers];
    }

    internal bool NeedGeneration => Members.Count > 0 || Methods.Count > 0 || Constructors.Count > 0;
}

internal class MethodTarget(IMethodSymbol symbol, List<Member> parameters, string? methodName = null, string accessModifier = "public")
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
    
    /// <summary>
    /// Custom method name for the generated validation method. If null, uses the original method name.
    /// </summary>
    internal string? CustomMethodName { get; } = methodName;
    
    /// <summary>
    /// Access modifier for the generated validation method. Defaults to "public".
    /// </summary>
    internal string AccessModifier { get; } = accessModifier;

    internal void SetAwaitableMembers(IEnumerable<string> awaitableMembers)
    {
       _awaitableMembers = [.. awaitableMembers];
    }

}

internal class ConstructorTarget(IMethodSymbol symbol, List<Member> parameters, string methodName = "Create", string accessModifier = "public")
{
    internal IMethodSymbol Symbol { get; } = symbol;

    private List<string> _awaitableMembers = [];
    internal IReadOnlyCollection<string> AwaitableMembers => _awaitableMembers;
    /// <summary>
    /// All parameters of the constructor. Use NeedsValidation flag to check if validation is required.
    /// </summary>
    internal IReadOnlyCollection<Member> Parameters { get; } = parameters;
    /// <summary>
    /// Parameters that require validation (have validation attributes or nested config).
    /// </summary>
    internal IEnumerable<Member> ValidatedParameters => Parameters.Where(p => p.NeedsValidation);
    
    /// <summary>
    /// Name of the generated factory method. Defaults to "Create".
    /// </summary>
    internal string MethodName { get; } = methodName;
    
    /// <summary>
    /// Access modifier for the generated factory method. Defaults to "public".
    /// </summary>
    internal string AccessModifier { get; } = accessModifier;

    internal void SetAwaitableMembers(IEnumerable<string> awaitableMembers)
    {
       _awaitableMembers = [.. awaitableMembers];
    }

}
