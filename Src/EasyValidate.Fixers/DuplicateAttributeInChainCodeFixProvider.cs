using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyValidate.Generator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;

namespace EasyValidate.Fixers
{
    /// <summary>
    /// Code fix provider for EASY009 diagnostics related to duplicate validation attributes in a chain.
    /// </summary>
    /// <remarks>
    /// This provider automatically generates and assigns unique chain names to validation attributes that would otherwise conflict within the same member.
    /// It parses the diagnostic message, determines used chain names, and suggests a new unique name for the attribute.
    /// </remarks>
    /// <example>
    /// <code>
    /// [NotEmpty]
    /// [NotEmpty]
    /// public string Foo { get; set; }
    /// // Diagnostic: Multiple validation attributes with the same chain name found.
    /// // After fix: [NotEmpty(Chain = "chain1")][NotEmpty(Chain = "chain2")]
    /// </code>
    /// </example>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(DuplicateAttributeInChainCodeFixProvider)), Shared]
    public class DuplicateAttributeInChainCodeFixProvider : CodeFixProvider
    {
        /// <summary>
        /// The diagnostic IDs that this code fix provider can fix (EASY009).
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            [ErrorIds.DuplicateChainName];

        /// <summary>
        /// Gets the fix-all provider for batch fixing.
        /// </summary>
        /// <returns>The batch fix-all provider.</returns>
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        /// <summary>
        /// Registers code fixes for diagnostics related to duplicate chain names in validation attributes.
        /// </summary>
        /// <param name="context">The code fix context.</param>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null) return;

            var diagnostics = context.Diagnostics.Where(diag => diag.Id == ErrorIds.DuplicateChainName).ToList();
            if (!diagnostics.Any()) return;

            var diagnostic = diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the attribute at the diagnostic location
            var token = root.FindToken(diagnosticSpan.Start);
            var attributeNode = token.Parent?.AncestorsAndSelf().OfType<AttributeSyntax>().FirstOrDefault();

            if (attributeNode == null) return;

            // Find the member (property or field) that contains this attribute
            var memberNode = attributeNode.AncestorsAndSelf().OfType<MemberDeclarationSyntax>().FirstOrDefault();
            if (memberNode == null) return;

            // Get the chain name from the diagnostic message
            var diagnosticMessage = diagnostic.GetMessage();
            var chainName = ExtractChainNameFromMessage(diagnosticMessage);

            // Don't return early for empty chain - this is the default chain case

            // Find all attributes on this member to determine a unique chain name
            var allAttributes = GetAllAttributesOnMember(memberNode);
            var usedChainNames = GetUsedChainNames(allAttributes);

            // Generate a unique chain name
            var newChainName = GenerateUniqueChainName(chainName, usedChainNames);

            var action = CodeAction.Create(
                title: $"Rename chain to '{newChainName}'",
                createChangedDocument: c => RenameChainAsync(context.Document, attributeNode, chainName, newChainName, c),
                equivalenceKey: $"RenameChain_{newChainName}");

            context.RegisterCodeFix(action, diagnostic);
        }

        private static string ExtractChainNameFromMessage(string message)
        {
            // Extract chain name from message like "Multiple validation attributes with the same chain name 'NotEmptyAttribute with chain ''' found..."
            // The format is: "chain name 'AttributeName with chain 'chainValue''"
            var pattern = @"chain name '.*? with chain '(.*?)'";
            var match = System.Text.RegularExpressions.Regex.Match(message, pattern);

            if (match.Success)
            {
                return match.Groups[1].Value;
            }

            // Fallback: return empty string for default chain
            return string.Empty;
        }

        private static List<AttributeSyntax> GetAllAttributesOnMember(MemberDeclarationSyntax memberNode)
        {
            var attributes = new List<AttributeSyntax>();

            foreach (var attributeList in memberNode.AttributeLists)
            {
                attributes.AddRange(attributeList.Attributes);
            }

            return attributes;
        }

        private static HashSet<string> GetUsedChainNames(List<AttributeSyntax> attributes)
        {
            var chainNames = new HashSet<string>();

            foreach (var attribute in attributes)
            {
                var chainValue = GetChainValueFromAttribute(attribute);
                chainNames.Add(chainValue); // Add all chain values (including empty string for default)
            }

            return chainNames;
        }

        private static string GetChainValueFromAttribute(AttributeSyntax attribute)
        {
            var argumentList = attribute.ArgumentList;
            if (argumentList == null) return string.Empty; // Default chain is empty string

            foreach (var argument in argumentList.Arguments)
            {
                if (argument.NameEquals?.Name.Identifier.ValueText == "Chain")
                {
                    if (argument.Expression is LiteralExpressionSyntax literal)
                    {
                        return literal.Token.ValueText;
                    }
                }
            }

            return string.Empty; // Default chain is empty string
        }

        private static string GenerateUniqueChainName(string baseChainName, HashSet<string> usedNames)
        {
            // If base chain name is empty (default chain), use "chain" as base
            var baseName = string.IsNullOrEmpty(baseChainName) ? "chain" : baseChainName;

            var counter = 1;
            string candidateName;

            do
            {
                candidateName = $"{baseName}{counter}";
                counter++;
            }
            while (usedNames.Contains(candidateName));

            return candidateName;
        }

        private static async Task<Document> RenameChainAsync(Document document, AttributeSyntax attributeNode,
            string oldChainName, string newChainName, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null) return document;

            // Find the Chain argument in the attribute
            var argumentList = attributeNode.ArgumentList;

            // Check if there's already a Chain argument
            AttributeArgumentSyntax? chainArgument = null;
            if (argumentList != null)
            {
                foreach (var argument in argumentList.Arguments)
                {
                    if (argument.NameEquals?.Name.Identifier.ValueText == "Chain")
                    {
                        chainArgument = argument;
                        break;
                    }
                }
            }

            SyntaxNode newRoot;

            if (chainArgument != null)
            {
                // Update existing Chain argument
                if (chainArgument.Expression is not LiteralExpressionSyntax) return document;

                var newLiteral = SyntaxFactory.LiteralExpression(
                    SyntaxKind.StringLiteralExpression,
                    SyntaxFactory.Literal(newChainName));

                var newArgument = chainArgument.WithExpression(newLiteral);
                newRoot = root.ReplaceNode(chainArgument, newArgument);
            }
            else
            {
                // Add new Chain argument
                var newChainArgument = SyntaxFactory.AttributeArgument(
                    SyntaxFactory.NameEquals(SyntaxFactory.IdentifierName("Chain")),
                    null,
                    SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(newChainName)));

                if (argumentList == null)
                {
                    // Create new argument list
                    var newArgumentList = SyntaxFactory.AttributeArgumentList(
                        SyntaxFactory.SingletonSeparatedList(newChainArgument));
                    var newAttribute = attributeNode.WithArgumentList(newArgumentList);
                    newRoot = root.ReplaceNode(attributeNode, newAttribute);
                }
                else
                {
                    // Add to existing argument list
                    var newArgumentList = argumentList.AddArguments(newChainArgument);
                    var newAttribute = attributeNode.WithArgumentList(newArgumentList);
                    newRoot = root.ReplaceNode(attributeNode, newAttribute);
                }
            }

            return document.WithSyntaxRoot(newRoot);
        }
    }
}
