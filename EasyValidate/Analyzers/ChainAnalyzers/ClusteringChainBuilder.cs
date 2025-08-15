using System.Collections.Generic;
using System.Linq;
using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;
namespace EasyValidate.Generator.Analyzers.ChainAnalyzers;

internal static class ClusteringChainBuilder
{
    internal static (bool found, List<AttributeInfo> order) FindValidChainClustering(
        IEnumerable<AttributeInfo> nodes,
        AnalyserContext context,
        ITypeSymbol memberType)
    {
        var copy = nodes.ToList();
        var result = new List<(ITypeSymbol output, AttributeInfo attribute)>();

        for (int i = 0; i < copy.Count; i++)
        {
            var (areCompatible, inputOutput) = copy[i].CanAccept(context, memberType);
            if (areCompatible)
            {
                result.Add((inputOutput!.ResolveOutPutType(memberType), copy[i]));
                copy.RemoveAt(i);
                break; // Found a valid starting point
            }
        }
        if (result.Count == 0)
        {
            // No valid starting point found, return empty result
            return (false, new List<AttributeInfo>());
        }
        var currentOutPut = result.Last().output;
        var nexCandidate = copy.GetAcceptable(context, currentOutPut);
        while (nexCandidate.areCompatible)
        {
            var nextOutPutType = nexCandidate.inputAndOutputTypes!.ResolveOutPutType(currentOutPut);
            result.Add((nextOutPutType, copy[nexCandidate.index]));
            copy.RemoveAt(nexCandidate.index);
            currentOutPut = nextOutPutType;
            nexCandidate = copy.GetAcceptable(context, nextOutPutType);
        }
        return (result.Count == nodes.Count(), result.Select(x => x.attribute).ToList());
    }


    private static (bool areCompatible, int index, InputAndOutputTypes? inputAndOutputTypes) GetAcceptable(this List<AttributeInfo> attributes, AnalyserContext context, ITypeSymbol previousOutputType)
    {
        for (int i = 0; i < attributes.Count; i++)
        {
            var attribute = attributes[i];
            var (areCompatible, inputOutput) = attribute.CanAccept(context, previousOutputType);
            if (areCompatible)
            {
                return (true, i, inputOutput);
            }
        }
        return (false, 0, null);
    }
}