using System.Collections.Generic;
using EasyValidate.Generator.Types;

namespace EasyValidate.Generator.Analyzers;

internal abstract class ChainAnalyzer
{
    internal abstract (bool valid, List<OrderInfo>? order) Analyze(AnalyserContext context, string chainName, IReadOnlyList<AttributeInfo> chain);
}


internal class OrderInfo
{
    internal OrderInfo(AttributeInfo info, InputAndOutputTypes types)
    {
        Info = info;
        Types = types;
    }
    internal AttributeInfo Info { get; }
    internal InputAndOutputTypes Types { get; }
}





