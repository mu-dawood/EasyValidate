using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EasyValidate.Analyzers.Analyzers;

public record AttributeInfo
{
    public string Name { get; set; } = default!;
    public string FullName { get; set; } = default!;
    public ImmutableArray<InputAndOutputTypes> InputAndOutputTypes { get; set; } = [];
    public bool IsNotNullAttribute { get; set; } = false;

    public INamedTypeSymbol? AttributeClass { get; set; }

    public Location? Location { get; set; }

    public ImmutableArray<TypedConstant> ConstructorArguments { get; set; } = [];
    public ImmutableArray<KeyValuePair<string, TypedConstant>> NamedArguments { get; set; } = [];


    public (bool areCompatible, InputAndOutputTypes?) CanAccept(SymbolAnalysisContext context, ITypeSymbol type)
    {
        if (IsNotNullAttribute) return (true, InputAndOutputTypes.FirstOrDefault());
        return InputAndOutputTypes.CanAccept(context.Compilation, type);
    }
}