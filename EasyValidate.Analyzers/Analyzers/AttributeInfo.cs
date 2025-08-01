using System.Collections.Immutable;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EasyValidate.Analyzers.Analyzers;

public record AttributeInfo
{
    public string Name { get; }
    public string FullName { get; }
    public ImmutableArray<InputAndOutputTypes> InputAndOutputTypes { get; }
    public bool IsOptionalNotNullAttribute { get; }
    public INamedTypeSymbol? AttributeClass { get; }
    public string? ConditionalMethodName { get; }
    public Location? Location { get; }
    public ImmutableArray<TypedConstant> ConstructorArguments { get; }
    public ImmutableArray<KeyValuePair<string, TypedConstant>> NamedArguments { get; }

    public AttributeInfo(
        string name,
        string fullName,
        ImmutableArray<InputAndOutputTypes> inputAndOutputTypes,
        bool isOptionalNotNullAttribute,
        INamedTypeSymbol? attributeClass,
        string? conditionalMethodName,
        Location? location,
        ImmutableArray<TypedConstant> constructorArguments,
        ImmutableArray<KeyValuePair<string, TypedConstant>> namedArguments)
    {
        Name = name;
        FullName = fullName;
        InputAndOutputTypes = inputAndOutputTypes;
        IsOptionalNotNullAttribute = isOptionalNotNullAttribute;
        AttributeClass = attributeClass;
        ConditionalMethodName = conditionalMethodName;
        Location = location;
        ConstructorArguments = constructorArguments;
        NamedArguments = namedArguments;
    }

    public (bool areCompatible, InputAndOutputTypes?) CanAccept(SymbolAnalysisContext context, ITypeSymbol type)
    {
        if (IsOptionalNotNullAttribute) return (true, new(type, type, false, true));
        return InputAndOutputTypes.CanAccept(context.Compilation, type);
    }
}