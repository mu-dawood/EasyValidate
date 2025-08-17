using System.Collections.Generic;
using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;

namespace EasyValidate.Generator.Analyzers.ChainAnalyzers;
/// <summary>
/// Analyzer that detects duplicate validation attributes with the same Chain value on a single member.
/// </summary>
/// <docs-explanation>
/// Validation attributes can be grouped into chains using the Chain property. Using multiple attributes 
/// with the same Chain value on the same property or field creates ambiguity in the validation process 
/// and can lead to unexpected behavior. Each chain should have a unique name within the scope of a member.
/// </docs-explanation>
/// <docs-good-example>
/// public class User
/// {
///     [Required(Chain = "primary")]
///     [MinLength(3, Chain = "primary")]
///     [MaxLength(20, Chain = "secondary")]
///     public string Name { get; set; }
/// }
/// </docs-good-example>
/// <docs-bad-example>
/// public class User
/// {
///     [Required(Chain = "primary")]
///     [MinLength(3, Chain = "primary")]
///     [MaxLength(20, Chain = "primary")] // Duplicate chain name
///     public string Name { get; set; }
/// }
/// </docs-bad-example>
/// <docs-fixes>
/// Rename one of the chains to a unique name|Remove duplicate attributes|Combine attributes into a single chain with proper ordering
/// </docs-fixes>
internal class DuplicateAttributeInChainProcessor : ChainAnalyzer
{
    private static readonly DiagnosticDescriptor Rule = new(
        id: ErrorIds.DuplicateChainName,
        title: "Duplicate Chain Names in Validation Attributes",
        messageFormat: "Multiple validation attributes with the same chain name '{0}' found on member '{1}'. Chain names must be unique within a member.",
        category: "Usage",
        defaultSeverity: DiagnosticSeverity.Error,
        isEnabledByDefault: true,
        description: "Validation attributes with duplicate chain names create ambiguity in validation processing.");



    internal override (bool valid, List<OrderInfo>? order) Analyze(AnalyserContext context, string chainName, IReadOnlyList<AttributeInfo> chain)
    {
        // Detect duplicate chain names in the queue
        var processed = new HashSet<string>();
        bool hasDuplicates = false;
        foreach (var attribute in chain)
        {
            var name = attribute.FullName;
            if (processed.Contains(name))
            {
                ReportDuplicates(context, attribute, chainName);
                hasDuplicates = true;
                continue; // Skip already processed attributes
            }
            processed.Add(name);
        }
        return (!hasDuplicates, order: null); // No specific order needed for duplicates
    }



    private static void ReportDuplicates(AnalyserContext context, AttributeInfo attribute, string chainName)
    {
        var diagnostic = Diagnostic.Create(
            Rule,
            attribute.Location,
            $"{attribute.Attribute.AttributeClass?.Name} with chain '{chainName}'",
            context.Member.Name);

        context.Context.ReportDiagnostic(diagnostic);
    }


}
