using System;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading.Tasks;
using EasyValidate.Generator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EasyValidate.Fixers
{
    /// <summary>
    /// Code fix provider for EASY011 diagnostics related to missing NotNull attribute injection in validation chains.
    /// </summary>
    /// <remarks>
    /// This provider automatically injects a NotNull attribute at the correct position in the attribute chain, optionally with a Chain argument.
    /// It parses the diagnostic message to determine the chain name and insertion position, ensuring proper validation behavior.
    /// </remarks>
    /// <example>
    /// <code>
    /// [ValidationA]
    /// [ValidationB]
    /// public string Foo { get; set; }
    /// // Diagnostic: Validation chain for member 'Foo' is incompatible due to null types. Add NotNull attribute at position 1.
    /// // After fix: [ValidationA][NotNull][ValidationB]
    /// </code>
    /// </example>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(NotNullInjectionAttributeUsageCodeFixProvider)), Shared]
    public class NotNullInjectionAttributeUsageCodeFixProvider : CodeFixProvider
    {
        /// <summary>
        /// The diagnostic IDs that this code fix provider can fix (EASY011).
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            [ErrorIds.NeedsNotNullInjection];

        /// <summary>
        /// Gets the fix-all provider for batch fixing.
        /// </summary>
        /// <returns>The batch fix-all provider.</returns>
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        /// <summary>
        /// Registers code fixes for diagnostics related to missing NotNull attribute injection in validation chains.
        /// </summary>
        /// <param name="context">The code fix context.</param>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null) return;

            var diagnostic = context.Diagnostics.FirstOrDefault(diag => diag.Id == ErrorIds.NeedsNotNullInjection);
            if (diagnostic == null) return;

            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var token = root.FindToken(diagnosticSpan.Start);
            var attributeNode = token.Parent?.AncestorsAndSelf().OfType<AttributeSyntax>().FirstOrDefault();
            if (attributeNode == null) return;

            var memberNode = attributeNode.AncestorsAndSelf().OfType<MemberDeclarationSyntax>().FirstOrDefault();
            if (memberNode == null) return;

            // For EASY011, the message is:
            // "Validation chain '{2}' for member '{0}' is incompatible due to null types. Add NotNull attribute at position {1}."
            // So we extract the chain name and position from the message string (since Diagnostic.Arguments is not accessible)
            var diagnosticMessage = diagnostic.GetMessage();
            // The message may start with (chainName) or not.
            // If present, inject NotNull(Chain = "chainName"); else, inject NotNull.
            string? chainName = null;
            int insertPosition = 0;
            // Regex: ^\(([^)]+)\)\s*Validation for member '.*' is incompatible due to null types. Add NotNull attribute at position (\d+)
            var chainAndPosMatch = System.Text.RegularExpressions.Regex.Match(
                diagnosticMessage,
                @"^\(([^)]+)\)\s*Validation for member '[^']+' is incompatible due to null types. Add NotNull attribute at position (\d+)");
            if (chainAndPosMatch.Success)
            {
                chainName = chainAndPosMatch.Groups[1].Value;
                int.TryParse(chainAndPosMatch.Groups[2].Value, out insertPosition);
            }
            else
            {
                // Fallback: try to extract just the position
                var posMatch = System.Text.RegularExpressions.Regex.Match(diagnosticMessage, @"position (\\d+)");
                if (posMatch.Success)
                    int.TryParse(posMatch.Groups[1].Value, out insertPosition);
            }

            // Find all attribute lists on this member
            var attributeLists = memberNode.AttributeLists;
            AttributeListSyntax? targetAttributeList = null;
            if (!string.IsNullOrEmpty(chainName))
            {
                // Try to find the attribute list that contains the chain (by argument or naming convention)
                foreach (var attrList in attributeLists)
                {
                    foreach (var attr in attrList.Attributes)
                    {
                        // Look for an attribute with an argument matching the chain name
                        if (attr.ArgumentList != null && attr.ArgumentList.Arguments.Any(arg => arg.ToString().Contains(chainName)))
                        {
                            targetAttributeList = attrList;
                            break;
                        }
                    }
                    if (targetAttributeList != null) break;
                }
            }
            // Fallback: if not found, use the attribute list containing the attribute at the diagnostic location
            if (targetAttributeList == null)
                targetAttributeList = (AttributeListSyntax?)attributeNode.Parent;
            if (targetAttributeList == null) return;

            // Find all attributes in the target list
            var attrListAttrs = targetAttributeList.Attributes.ToList();
            // Check if NotNull already exists at the correct position
            var notNullExists = attrListAttrs.ElementAtOrDefault(insertPosition)?.Name.ToString().Contains("NotNull") == true;
            if (notNullExists) return;

            // Create NotNull attribute, with or without Chain argument
            AttributeSyntax notNullAttr;
            if (!string.IsNullOrEmpty(chainName))
            {
                notNullAttr = SyntaxFactory.Attribute(
                    SyntaxFactory.IdentifierName("NotNull"),
                    SyntaxFactory.AttributeArgumentList(
                        SyntaxFactory.SingletonSeparatedList(
                            SyntaxFactory.AttributeArgument(
                                SyntaxFactory.NameEquals("Chain"),
                                null,
                                SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(chainName!))
                            )
                        )
                    )
                );
            }
            else
            {
                notNullAttr = SyntaxFactory.Attribute(SyntaxFactory.IdentifierName("NotNull"));
            }

            // Insert NotNull at the specified position
            int safeInsert = Math.Max(0, Math.Min(insertPosition, attrListAttrs.Count));
            attrListAttrs.Insert(safeInsert, notNullAttr);
            var newAttributeList = targetAttributeList.WithAttributes(SyntaxFactory.SeparatedList(attrListAttrs));

            // Replace the old attribute list with the new one
            var newMemberNode = memberNode.ReplaceNode(targetAttributeList, newAttributeList);
            var newRoot = root.ReplaceNode(memberNode, newMemberNode);
            context.RegisterCodeFix(
                Microsoft.CodeAnalysis.CodeActions.CodeAction.Create(
                    title: !string.IsNullOrEmpty(chainName)
                        ? $"Inject NotNull attribute for chain '{chainName}'"
                        : "Inject NotNull attribute",
                    createChangedDocument: _ => Task.FromResult(context.Document.WithSyntaxRoot(newRoot)),
                    equivalenceKey: !string.IsNullOrEmpty(chainName)
                        ? $"InjectNotNullAttribute_{chainName}"
                        : "InjectNotNullAttribute"),
                diagnostic);
        }
    }
}
