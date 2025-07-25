using EasyValidate.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;

namespace EasyValidate.Fixers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ChainReorderingAttributeUsageCodeFixProvider)), Shared]
    public class ChainReorderingAttributeUsageCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            [ErrorIds.ChainNeedsReordering];

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null) return;

            var diagnostic = context.Diagnostics.FirstOrDefault(diag => diag.Id == ErrorIds.ChainNeedsReordering);
            if (diagnostic == null) return;

            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var token = root.FindToken(diagnosticSpan.Start);
            var attributeNode = token.Parent?.AncestorsAndSelf().OfType<AttributeSyntax>().FirstOrDefault();
            if (attributeNode == null) return;

            var memberNode = attributeNode.AncestorsAndSelf().OfType<MemberDeclarationSyntax>().FirstOrDefault();
            if (memberNode == null) return;

            // For EASY010, the message is:
            // "Validation chain '{2}' for member '{0}' is incompatible but can be reordered. Suggested order: {1}."
            // Extract chain name and suggested order from the message string
            var diagnosticMessage = diagnostic.GetMessage();
            // The message may start with (chainName) or not.
            // Example: "(chain1) Validation for member 'Foo' is incompatible but can be reordered. Suggested order: Bar -> Baz."
            string? chainName = null;
            string? suggestedOrder = null;
            // Regex: ^(\(([^)]+)\)\s*)?Validation for member '[^']+' is incompatible but can be reordered. Suggested order: ([^.]*)
            var match = System.Text.RegularExpressions.Regex.Match(
                diagnosticMessage,
                @"^(\(([^)]+)\)\s*)?Validation for member '[^']+' is incompatible but can be reordered. Suggested order: ([^.]*)");
            if (match.Success)
            {
                chainName = match.Groups[2].Success ? match.Groups[2].Value : null;
                suggestedOrder = match.Groups[3].Value.Trim();
            }
            // Only treat as no chain if chainName is null or exactly "default"
            if (string.IsNullOrEmpty(chainName) || chainName == "default")
            {
                chainName = null;
            }

            // Find all attribute lists on this member
            var attributeLists = memberNode.AttributeLists;
            AttributeListSyntax? targetAttributeList = null;
            if (!string.IsNullOrEmpty(chainName))
            {
                foreach (var attrList in attributeLists)
                {
                    foreach (var attr in attrList.Attributes)
                    {
                        // Look for an attribute with a named argument Chain = "chainName"
                        if (attr.ArgumentList != null &&
                            attr.ArgumentList.Arguments.Any(arg =>
                                arg.NameEquals != null &&
                                arg.NameEquals.Name.Identifier.Text == "Chain" &&
                                arg.Expression is LiteralExpressionSyntax literal &&
                                literal.Token.ValueText == chainName))
                        {
                            targetAttributeList = attrList;
                            break;
                        }
                    }
                    if (targetAttributeList != null) break;
                }
            }
            // Fallback: if not found, use the attribute list containing the attribute at the diagnostic location
            targetAttributeList ??= (AttributeListSyntax?)attributeNode.Parent;
            if (targetAttributeList == null) return;
            var allAttributes = targetAttributeList.Attributes.ToList();

            // If we have a suggested order, reorder attributes accordingly
            List<AttributeSyntax> sortedAttributes = allAttributes;
            if (!string.IsNullOrEmpty(suggestedOrder))
            {
                var orderNames = (suggestedOrder ?? string.Empty).Split(new[] { "->" }, StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim()).ToList();
                sortedAttributes = orderNames
                    .Select(name =>
                    {
                        var simpleName = name.EndsWith("Attribute") ? name.Substring(0, name.Length - 9) : name;
                        return allAttributes.FirstOrDefault(attr =>
                        {
                            var attrName = attr.Name.ToString();
                            if (attrName.EndsWith("Attribute")) attrName = attrName.Substring(0, attrName.Length - 9);
                            return string.Equals(attrName, simpleName, StringComparison.OrdinalIgnoreCase);
                        });
                    })
                    .Where(attr => attr != null)
                    .ToList()!;
                // Add any attributes not in the suggested order at the end
                sortedAttributes.AddRange(allAttributes.Except(sortedAttributes));
            }

            var newAttributeList = targetAttributeList.WithAttributes(SyntaxFactory.SeparatedList(sortedAttributes));
            var newMemberNode = memberNode.ReplaceNode(targetAttributeList, newAttributeList);
            var newRoot = root.ReplaceNode(memberNode, newMemberNode);
            context.RegisterCodeFix(
                Microsoft.CodeAnalysis.CodeActions.CodeAction.Create(
                    title: !string.IsNullOrEmpty(chainName)
                        ? $"Reorder validation attributes for chain '{chainName}'"
                        : "Reorder validation attributes",
                    createChangedDocument: _ => Task.FromResult(context.Document.WithSyntaxRoot(newRoot)),
                    equivalenceKey: !string.IsNullOrEmpty(chainName)
                        ? $"ReorderValidationAttributes_{chainName}"
                        : "ReorderValidationAttributes"),
                diagnostic);
        }
    }
}
