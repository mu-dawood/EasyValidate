using EasyValidate.Generator;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;

namespace EasyValidate.Fixers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PublicMethodCodeFixProvider)), Shared]
    public class PublicMethodCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            [ErrorIds.ValidateAttributeUsagePublicMethod];

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null) return;

            var diagnostic = context.Diagnostics.FirstOrDefault(diag => diag.Id == ErrorIds.ValidateAttributeUsagePublicMethod);
            if (diagnostic == null) return;

            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var token = root.FindToken(diagnosticSpan.Start);
            var methodNode = token.Parent?.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().FirstOrDefault();
            if (methodNode == null) return;

            // Only offer fixes if method is public
            if (methodNode.Modifiers.Any(m => m.IsKind(SyntaxKind.PublicKeyword)))
            {
                var nonPublicModifiers = new[] {
                    (SyntaxKind.PrivateKeyword, "private"),
                    (SyntaxKind.ProtectedKeyword, "protected")
                };
                foreach (var (modifierKind, title) in nonPublicModifiers)
                {

                    var newModifiers = methodNode.Modifiers
                        .Where(m => !m.IsKind(SyntaxKind.PublicKeyword))
                        .ToList();
                    newModifiers.Insert(0, SyntaxFactory.Token(modifierKind));
                    var newMethod = methodNode
                        .WithModifiers(SyntaxFactory.TokenList(newModifiers))
                        .WithLeadingTrivia(methodNode.GetLeadingTrivia())
                        .WithTrailingTrivia(methodNode.GetTrailingTrivia());
                    var newRoot = root.ReplaceNode(methodNode, newMethod);
                    context.RegisterCodeFix(
                        Microsoft.CodeAnalysis.CodeActions.CodeAction.Create(
                            title: $"Make method {title}",
                            createChangedDocument: _ => Task.FromResult(context.Document.WithSyntaxRoot(newRoot)),
                            equivalenceKey: $"MakeMethod{title.Replace(" ", "")}"),
                        diagnostic);

                }
            }
        }
    }
}
