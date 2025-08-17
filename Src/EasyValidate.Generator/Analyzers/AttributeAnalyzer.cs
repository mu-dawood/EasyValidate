using System.Collections.Generic;
using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Generator.Analyzers;

internal abstract class AttributeAnalyzer
{
    internal abstract bool Analyze(AnalyserContext context, AttributeInfo attribute);

    internal abstract ICollection<DiagnosticDescriptor> DiagnosticDescriptors { get; }
}
