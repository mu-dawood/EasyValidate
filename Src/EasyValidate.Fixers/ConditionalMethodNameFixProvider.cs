using System.Collections.Immutable;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System;
using System.Text.RegularExpressions;

namespace EasyValidate.Fixers
{
    /// <summary>
    /// Code fix provider for InvalidConditionalMethodName error.
    /// Suggests valid method names for ConditionalMethod attribute value.
    /// </summary>
    /// <summary>
    /// Provides code fixes for InvalidConditionalMethodName diagnostics by suggesting valid method names for the ConditionalMethod attribute.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ConditionalMethodNameFixProvider)), Shared]
    public class ConditionalMethodNameFixProvider : CodeFixProvider
    {
        /// <summary>
        /// The diagnostic IDs that this code fix provider can fix.
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds => [Generator.ErrorIds.InvalidConditionalMethodName];

        /// <summary>
        /// Gets the fix-all provider. Returns null to disable batch fixing for this provider.
        /// </summary>
        /// <returns>Null (batch fixing disabled).</returns>
        public sealed override FixAllProvider? GetFixAllProvider() => null;

        /// <summary>
        /// Registers code fixes for InvalidConditionalMethodName diagnostics by suggesting valid method names.
        /// </summary>
        /// <param name="context">The code fix context.</param>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null) return;

            foreach (var diagnostic in context.Diagnostics.Where(d => FixableDiagnosticIds.Contains(d.Id)))
            {
                var diagnosticSpan = diagnostic.Location.SourceSpan;
                var token = root.FindToken(diagnosticSpan.Start);
                var node = token.Parent;

                var attributeNode = node?.AncestorsAndSelf().OfType<AttributeSyntax>().FirstOrDefault();
                if (attributeNode == null) continue;

                var typeDeclaration = attributeNode.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().FirstOrDefault();
                if (typeDeclaration == null) continue;

                // Find all valid method names in the class that match our signature
                var validMethods = typeDeclaration.Members
                    .OfType<MethodDeclarationSyntax>()
                    .Where(m =>
                        m.ParameterList.Parameters.Count == 1 &&
                        m.ParameterList.Parameters[0].Type?.ToString().Contains("IChainResult") == true &&
                        (m.ReturnType.ToString() == "bool" || m.ReturnType.ToString() == "System.Threading.Tasks.ValueTask<bool>" || m.ReturnType.ToString() == "ValueTask<bool>")
                    )
                    .Select(m => m.Identifier.ValueText)
                    .ToList();

                // Try to suggest the closest valid method name first
                var currentName = attributeNode.ArgumentList?.Arguments
                    .FirstOrDefault(arg => arg.NameEquals?.Name.Identifier.ValueText == "ConditionalMethod")?
                    .Expression as LiteralExpressionSyntax;
                var currentValue = currentName?.Token.ValueText ?? string.Empty;



                var suggested = new HashSet<string>();
                var suggestedName = SuggestAlternative(currentValue, validMethods);
                if (suggestedName != null)
                {
                    suggested.Add(suggestedName);
                }
                foreach (var validMethod in validMethods)
                    suggested.Add(validMethod);

                foreach (var validName in suggested)
                {
                    var methodExists = validMethods.Contains(validName);

                    CodeAction action;
                    if (methodExists)
                    {
                        action = CodeAction.Create(
                            title: $"Change ConditionalMethod to '{validName}'",
                            createChangedDocument: c => ReplaceConditionalMethodNameAsync(context.Document, attributeNode, validName, c),
                            equivalenceKey: $"ChangeConditionalMethod_{validName}");
                    }
                    else
                    {
                        // Create the method using ConditionalMethodImplementationFixProvider.CreateConditionalMethodAsync
                        action = CodeAction.Create(
                            title: $"Change ConditionalMethod to '{validName}' and create method",
                            createChangedDocument: async c =>
                            {
                                var docWithName = await ReplaceConditionalMethodNameAsync(context.Document, attributeNode, validName, c);
                                // Use bool return type by default
                                return await ConditionalMethodImplementationFixProvider.CreateConditionalMethodAsync(
                                    docWithName,
                                    typeDeclaration,
                                    validName,
                                    false,
                                    c);
                            },
                            equivalenceKey: $"ChangeConditionalMethodAndCreate_{validName}");
                    }
                    context.RegisterCodeFix(action, diagnostic);
                }
            }

        }
        private static readonly string[] CommonPrefixes = { "Get", "Set", "Find", "Load", "Save" };
        private static readonly HashSet<string> CSharpKeywords = new HashSet<string>
        {
        "class", "int", "string", "void", "return", "public", "private",
        "protected", "internal", "new", "static", "if", "else", "switch", "case"
        };
        private static string? SuggestAlternative(string invalidName, IEnumerable<string> ignoredNames)
        {
            if (string.IsNullOrWhiteSpace(invalidName))
                return null;

            var ignored = new HashSet<string>(ignoredNames ?? Enumerable.Empty<string>(), StringComparer.OrdinalIgnoreCase);

            // 1) Clean invalid characters
            var cleaned = Regex.Replace(invalidName, @"[^A-Za-z0-9_]", "");
            if (string.IsNullOrEmpty(cleaned))
                return null;

            // 2) If starts with digit, prepend underscore
            if (char.IsDigit(cleaned[0]))
                cleaned = "_" + cleaned;

            // 3) Capitalize first letter → common method style
            var pascal = char.ToUpper(cleaned[0]) + cleaned.Substring(1);
            if (!ignored.Contains(pascal) && !CSharpKeywords.Contains(pascal))
                return pascal;  // ✅ EARLY RETURN

            // 4) Try adding common prefixes
            foreach (var prefix in CommonPrefixes)
            {
                var candidate = prefix + pascal;
                if (!ignored.Contains(candidate) && !CSharpKeywords.Contains(candidate))
                    return candidate; // ✅ EARLY RETURN
            }

            // 5) As fallback, append "Method"
            var fallback = pascal + "Method";
            return ignored.Contains(fallback) ? null : fallback;
        }



        /// <summary>
        /// Replaces the ConditionalMethod attribute value with a new valid method name.
        /// </summary>
        /// <param name="document">The document to update.</param>
        /// <param name="attributeNode">The attribute syntax node.</param>
        /// <param name="newMethodName">The new method name to set.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>The updated document.</returns>
        private static async Task<Document> ReplaceConditionalMethodNameAsync(
            Document document,
            AttributeSyntax attributeNode,
            string newMethodName,
            CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null) return document;

            var argumentList = attributeNode.ArgumentList;
            if (argumentList == null) return document;

            var newArguments = argumentList.Arguments.Select(arg =>
            {
                if (arg.NameEquals?.Name.Identifier.ValueText == "ConditionalMethod")
                {
                    return arg.WithExpression(SyntaxFactory.LiteralExpression(
                        SyntaxKind.StringLiteralExpression,
                        SyntaxFactory.Literal(newMethodName)));
                }
                return arg;
            });

            var newAttribute = attributeNode.WithArgumentList(
                argumentList.WithArguments(SyntaxFactory.SeparatedList(newArguments)));

            var newRoot = root.ReplaceNode(attributeNode, newAttribute);
            return document.WithSyntaxRoot(newRoot);
        }
    }
}
