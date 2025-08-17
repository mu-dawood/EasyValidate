using System.Collections.Generic;
using System.Linq;
using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp;

namespace EasyValidate.Generator.Analyzers.ChainAnalyzers;

internal class ChainIncompatibilityProcessor : ChainAnalyzer
{
    private static readonly DiagnosticDescriptor ReorderableChainError = new(
        ErrorIds.ChainNeedsReordering,
        "Validation chain attributes need reordering",
        "{0}Validation for member '{1}' is incompatible but can be reordered. Suggested order: {2}.",
        "Usage",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "The validation attributes in this chain are not in compatible order but can be rearranged to create a valid type flow.");

    private static readonly DiagnosticDescriptor NotNullInjectableError = new(
        ErrorIds.NeedsNotNullInjection,
        "Validation chain needs NotNull attribute injection",
        "{0}Validation for member '{1}' is incompatible due to null types. Add NotNull attribute at position {2}.",
        "Usage",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "The validation chain has type incompatibilities that can be resolved by injecting NotNull attributes to handle nullable types.");

    internal static readonly DiagnosticDescriptor IncompatibleChainError = new(
        ErrorIds.IncompatibleChainTypes,
        "Validation chain has incompatible types",
        "{0}Validation for member '{1}' has incompatible attribute types that cannot be resolved: {2}",
        "Usage",
        DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "The validation attributes in this chain have fundamental type incompatibilities that cannot be resolved by reordering or adding NotNull attributes.");


    internal override (bool valid, List<OrderInfo>? order) Analyze(AnalyserContext context, string chainName, IReadOnlyList<AttributeInfo> chain)
    {
        if (chain.Count == 0) return (true, []);
        var chainLabel = string.IsNullOrEmpty(chainName) ? chainName : $"({chainName}) ";

        // Sequential processing with optimized early exit
        var currentType = context.GetMemberType();
        List<OrderInfo> properOrders = [];
        for (int i = 0; i < chain.Count; i++)
        {
            var attribute = chain[i];
            var (areCompatible, info) = attribute.CanAccept(context, currentType);
            // If attribute has input type constraint, check compatibility
            if (!areCompatible)
            {
                if (chain.Count > 1)
                {
                    var (foundValidChain, suggestedOrder) = ClusteringChainBuilder.FindValidChainClustering(chain, context, context.GetMemberType());
                    if (foundValidChain)
                    {
                        context.Context.ReportDiagnostic(Diagnostic.Create(
                            ReorderableChainError,
                            attribute.Location,
                            chainLabel,
                            context.Member.Name,
                            string.Join(" -> ", suggestedOrder.Select(a => a.Name))
                        ));
                        return (false, null);
                    }
                }

                if (CanBeFixedByNotNullInjection(attribute, currentType, context.Compilation))
                {
                    context.Context.ReportDiagnostic(Diagnostic.Create(
                        NotNullInjectableError,
                        attribute.Location,
                        chainLabel,
                        context.Member.Name,
                        i.ToString()
                    ));
                    return (false, null);
                }

                // Report as truly incompatible
                context.Context.ReportDiagnostic(Diagnostic.Create(
                    IncompatibleChainError,
                    attribute.Location,
                    chainLabel,
                    context.Member.Name,
                    $"{attribute.Name} expects one of [{string.Join(", ", attribute.InputAndOutputTypes.Select((x) => x.InputType?.ToDisplayString()))}] but got {currentType.ToDisplayString()}"
                ));
                return (false, null);
            }
            else
            {
                properOrders.Add(new(attribute, info!));
                currentType = info!.ResolveOutPutType(currentType);
            }
        }
        return (true, properOrders);
    }





    private static bool CanBeFixedByNotNullInjection(AttributeInfo attributeInfo, ITypeSymbol currentType, Compilation compilation)
    {
        // For value types (e.g., DateTime? -> DateTime)
        if (currentType is INamedTypeSymbol namedCurrent &&
            namedCurrent.IsGenericType &&
            namedCurrent.ConstructedFrom.SpecialType == SpecialType.System_Nullable_T)
        {
            var underlying = namedCurrent.TypeArguments[0];
            return attributeInfo.InputAndOutputTypes.Any(info => SymbolEqualityComparer.Default.Equals(underlying, info.InputType));
        }
        // For reference types (e.g., List<string>? -> IEnumerable<string>)
        if (currentType.NullableAnnotation == NullableAnnotation.Annotated)
        {
            var notNullCurrent = currentType.WithNullableAnnotation(NullableAnnotation.NotAnnotated);
            foreach (var info in attributeInfo.InputAndOutputTypes)
            {
                if (info.InputType.NullableAnnotation == NullableAnnotation.NotAnnotated)
                {
                    if (compilation.ClassifyConversion(notNullCurrent, info.InputType).IsImplicit)
                    {
                        return true;
                    }
                    // Fallback: check for direct type match
                    if (SymbolEqualityComparer.Default.Equals(
                        info.InputType.WithNullableAnnotation(NullableAnnotation.NotAnnotated),
                        notNullCurrent))
                    {
                        return true;
                    }
                }
            }
        }
        return false;
    }




}
