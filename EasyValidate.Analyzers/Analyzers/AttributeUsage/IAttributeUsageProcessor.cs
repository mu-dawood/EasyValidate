using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace EasyValidate.Analyzers.Analyzers.AttributeUsage;

internal interface IAttributeUsageProcessor
{
    void Process(SymbolAnalysisContext context, AttributeInfo attribute, ITypeSymbol memberType, string memberName);

    ICollection<DiagnosticDescriptor> DiagnosticDescriptors { get; }
}


internal interface IAttributeUsageChainProcessor
{
    void Process(SymbolAnalysisContext context, string chainName, IReadOnlyList<AttributeInfo> chain, ITypeSymbol memberType, string memberName);

    ICollection<DiagnosticDescriptor> DiagnosticDescriptors { get; }
}





