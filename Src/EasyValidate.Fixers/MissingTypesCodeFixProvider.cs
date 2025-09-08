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
    /// This provider automatically adds missing interface and class implementations to the target type (class, record, or struct) based on diagnostic messages.
    /// It parses the diagnostic message, adds necessary usings, and updates the type declaration to implement the required types.
    /// </remarks>
    /// <example>
    /// <code>
    /// public class MyValidator // Diagnostic: missing IGenerate
    /// // After fix: public class MyValidator : IGenerate
    /// 
    /// public record MyRecord // Diagnostic: missing IGenerate
    /// // After fix: public record MyRecord : IGenerate
    /// 
    /// public struct MyStruct // Diagnostic: missing IGenerate
    /// // After fix: public struct MyStruct : IGenerate
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
            var typeDeclaration = node?.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().FirstOrDefault();
            if (typeDeclaration == null) return;

            if (diagnostic.Id == ErrorIds.ValidateAttributeUsageMissingType)
            {
                var message = diagnostic.GetMessage();
                var types = ExtractTypesFromMessage(message);
                var action = CodeAction.Create(
                    title: "Implement required types (interfaces/classes)",
                    createChangedDocument: c => AddTypesAsync(context.Document, typeDeclaration, types, c),
                    equivalenceKey: "AddMissingTypes");
                context.RegisterCodeFix(action, diagnostic);
            }
        }

        private static async Task<Document> AddTypesAsync(
            Document document,
            TypeDeclarationSyntax typeDeclaration,
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
                typeDeclaration = root.DescendantNodes()
                                     .OfType<TypeDeclarationSyntax>()
                                     .FirstOrDefault(c => c.Identifier.Text == typeDeclaration.Identifier.Text) ?? typeDeclaration;
                var oldlist = typeDeclaration.BaseList?.Types ?? [];
                var baseList = SyntaxFactory.BaseList(SyntaxFactory.Token(SyntaxKind.ColonToken), SyntaxFactory.SeparatedList(classes.Concat(oldlist).Concat(interfaces)))
                .WithAdditionalAnnotations(Formatter.Annotation);

                // ensure identifier ends with a space (avoids newline before colon)
                var newId = typeDeclaration.Identifier.WithTrailingTrivia(SyntaxFactory.Space);
                var newTypeDeclaration = typeDeclaration.WithIdentifier(newId).WithBaseList(baseList);

                var newRoot = root.ReplaceNode(typeDeclaration, newTypeDeclaration);
                return document.WithSyntaxRoot(newRoot);

            }
            else if (root is TypeDeclarationSyntax)
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
                // If the root is already a type declaration, we can directly modify it
                // This is useful for cases where the type is not in a compilation unit
                var colon = SyntaxFactory.Token(SyntaxKind.ColonToken)
                    .WithLeadingTrivia(SyntaxFactory.Space)
                    .WithTrailingTrivia(SyntaxFactory.Space);
                var newBaseList = SyntaxFactory.BaseList(colon,
                    SyntaxFactory.SeparatedList(classes.Concat(interfaces)));
                var newTypeDeclaration = typeDeclaration.WithBaseList(newBaseList)
                    .WithAdditionalAnnotations(Microsoft.CodeAnalysis.Formatting.Formatter.Annotation);
                var newRoot = root.ReplaceNode(typeDeclaration, newTypeDeclaration);
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
