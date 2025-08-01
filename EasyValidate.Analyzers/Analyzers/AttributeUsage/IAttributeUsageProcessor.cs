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
    (bool passed, List<OrderInfo>? order) Process(SymbolAnalysisContext context, string chainName, IReadOnlyList<AttributeInfo> chain, ITypeSymbol memberType, string memberName);

    ICollection<DiagnosticDescriptor> DiagnosticDescriptors { get; }
}

public class OrderInfo
{
    internal OrderInfo(AttributeInfo info, InputAndOutputTypes types)
    {
        Info = info;
        Types = types;
    }
    public AttributeInfo Info { get; }
    public InputAndOutputTypes Types { get; }
}





