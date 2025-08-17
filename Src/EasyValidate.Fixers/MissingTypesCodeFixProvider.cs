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
using Microsoft.CodeAnalysis.Formatting;

namespace EasyValidate.Fixers
{
    /// <summary>
    /// Code fix provider for EASY011 diagnostics related to missing required types (interfaces/classes) in validation attribute usage.
    /// </summary>
    /// <remarks>
    /// This provider automatically adds missing interface and class implementations to the target class based on diagnostic messages.
    /// It parses the diagnostic message, adds necessary usings, and updates the class declaration to implement the required types.
    /// </remarks>
    /// <example>
    /// <code>
    /// public class MyValidator // Diagnostic: missing IGenerate
    /// // After fix: public class MyValidator : IGenerate
    /// </code>
    /// </example>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(MissingTypesCodeFixProvider)), Shared]
    public class MissingTypesCodeFixProvider : CodeFixProvider
    {
        /// <summary>
        /// The diagnostic IDs that this code fix provider can fix (EASY011).
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            [ErrorIds.ValidateAttributeUsageMissingType];

        /// <summary>
        /// Gets the fix-all provider for batch fixing.
        /// </summary>
        /// <returns>The batch fix-all provider.</returns>
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        /// <summary>
        /// Registers code fixes for diagnostics related to missing required types in validation attribute usage.
        /// </summary>
        /// <param name="context">The code fix context.</param>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null) return;

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var token = root.FindToken(diagnosticSpan.Start);
            var node = token.Parent;
            var classNode = node?.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            if (classNode == null) return;

            if (diagnostic.Id == ErrorIds.ValidateAttributeUsageMissingType)
            {
                var message = diagnostic.GetMessage();
                var types = ExtractTypesFromMessage(message);
                var action = CodeAction.Create(
                    title: "Implement required types (interfaces/classes)",
                    createChangedDocument: c => AddTypesAsync(context.Document, classNode, types, c),
                    equivalenceKey: "AddMissingTypes");
                context.RegisterCodeFix(action, diagnostic);
            }
        }

        private static async Task<Document> AddTypesAsync(
            Document document,
            ClassDeclarationSyntax classDeclaration,
            TypeInfo[] types,
            CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null) return document;
            // Add usings for all collected namespaces
            List<BaseTypeSyntax> classes = [];
            List<BaseTypeSyntax> interfaces = [];

            if (root is CompilationUnitSyntax compilationUnit)
            {
                // Collect all namespaces needed for types
                foreach (var item in types)
                {
                    var type = item.TypeName;
                    if (type.Contains('.'))
                    {
                        var ns = type.Substring(0, type.LastIndexOf('.'));
                        type = type.Substring(type.LastIndexOf('.') + 1);
                        if (!compilationUnit.Usings.Any(u => u.Name?.ToString() == ns))
                        {
                            var newUsing = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName(ns))
                                .WithTrailingTrivia(SyntaxFactory.ElasticCarriageReturnLineFeed);
                            compilationUnit = compilationUnit.WithUsings(compilationUnit.Usings.Add(newUsing));
                        }
                    }
                    var baseType = SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName(type))
                          .WithLeadingTrivia(SyntaxFactory.Space);
                    if (item.TypeKind == "class")
                        classes.Add(baseType);
                    else if (item.TypeKind == "interface")
                        interfaces.Add(baseType);
                }
                root = compilationUnit;
                classDeclaration = root.DescendantNodes()
                                     .OfType<ClassDeclarationSyntax>()
                                     .FirstOrDefault(c => c.Identifier.Text == classDeclaration.Identifier.Text) ?? classDeclaration;
                ClassDeclarationSyntax newClassDeclaration;
                if (classDeclaration.BaseList == null || classDeclaration.BaseList.Types.Count == 0)
                {
                    var baseList = SyntaxFactory.BaseList(SyntaxFactory.Token(SyntaxKind.ColonToken), SyntaxFactory.SeparatedList(classes.Concat(interfaces)))
                    .WithAdditionalAnnotations(Formatter.Annotation);

                    // ensure identifier ends with a space (avoids newline before colon)
                    var newId = classDeclaration.Identifier.WithTrailingTrivia(SyntaxFactory.Space);
                    var updated = classDeclaration.WithIdentifier(newId).WithBaseList(baseList);
                    newClassDeclaration = updated;
                }
                else
                {
                    var separated = classDeclaration.BaseList.Types.AddRange(classes.Concat(interfaces));
                    var newBaseList = classDeclaration.BaseList.WithTypes(separated);
                    newClassDeclaration = classDeclaration
                        .WithBaseList(newBaseList)
                        .WithAdditionalAnnotations(Microsoft.CodeAnalysis.Formatting.Formatter.Annotation);
                }
                var newRoot = root.ReplaceNode(classDeclaration, newClassDeclaration);
                return document.WithSyntaxRoot(newRoot);

            }
            else if (root is ClassDeclarationSyntax)
            {
                foreach (var item in types)
                {
                    var type = item.TypeName;
                    var baseType = SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName(type))
                          .WithLeadingTrivia(SyntaxFactory.Space);
                    if (item.TypeKind == "class")
                        classes.Add(baseType);
                    else if (item.TypeKind == "interface")
                        interfaces.Add(baseType);
                }
                // If the root is already a class, we can directly modify it
                // This is useful for cases where the class is not in a compilation unit
                var colon = SyntaxFactory.Token(SyntaxKind.ColonToken)
                    .WithLeadingTrivia(SyntaxFactory.Space)
                    .WithTrailingTrivia(SyntaxFactory.Space);
                var newBaseList = SyntaxFactory.BaseList(colon,
                    SyntaxFactory.SeparatedList<BaseTypeSyntax>(classes.Concat(interfaces)));
                var newClassDeclaration = classDeclaration.WithBaseList(newBaseList)
                    .WithAdditionalAnnotations(Microsoft.CodeAnalysis.Formatting.Formatter.Annotation);
                var newRoot = root.ReplaceNode(classDeclaration, newClassDeclaration);
                return document.WithSyntaxRoot(newRoot);

            }
            else
                return document;
        }


        private static TypeInfo[] ExtractTypesFromMessage(string message)
        {
            // Extract types from the diagnostic message (after colon and inside brackets)
            var startIdx = message.IndexOf('[');
            var endIdx = message.IndexOf(']');
            if (startIdx < 0 || endIdx < 0 || endIdx <= startIdx) return [];
            var typesPart = message.Substring(startIdx + 1, endIdx - startIdx - 1).Trim();
            return [.. typesPart.Split(',').Select(t => {
                var parts = t.Trim().Split(':');
                if (parts.Length == 2)
                    return new TypeInfo(parts[0].Trim(), parts[1].Trim());
                return new TypeInfo("", t.Trim());
            }).Where(x => !string.IsNullOrEmpty(x.TypeKind) && !string.IsNullOrEmpty(x.TypeName))];
        }
        private record struct TypeInfo(string TypeKind, string TypeName);
    }

}
