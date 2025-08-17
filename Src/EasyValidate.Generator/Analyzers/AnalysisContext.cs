using System;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Generator.Analyzers;



internal record AnalyserContext
{
    internal SourceProductionContext Context { get; }
    internal INamedTypeSymbol ContainingClass { get; }
    internal Compilation Compilation { get; }
    internal ISymbol Member { get; }


    internal ITypeSymbol GetMemberType()
    {
        return Member switch
        {
            IPropertySymbol property => property.Type,
            IFieldSymbol field => field.Type,
            IParameterSymbol parameter => parameter.Type,
            _ => throw new InvalidOperationException($"Unsupported member type: {Member.GetType().Name}")
        };
    }
    public AnalyserContext(SourceProductionContext context, Compilation compilation, INamedTypeSymbol containingClass, ISymbol member)
    {
        ContainingClass = containingClass;
        Compilation = compilation;
        Context = context;
        Member = member;
    }
}


