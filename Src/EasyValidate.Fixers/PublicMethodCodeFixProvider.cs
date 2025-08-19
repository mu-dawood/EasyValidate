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
    /// Code fix provider for diagnostics related to public method usage in validation attributes.
    /// </summary>
    /// <remarks>
    /// This provider automatically suggests making public methods private or protected when required by validation attribute usage rules.
    /// It registers code fixes to change the method's accessibility modifier accordingly.
    /// </remarks>
    /// <example>
    /// <code>
    /// public bool ValidateFoo() { ... }
    /// // Diagnostic: Method should not be public.
    /// // After fix: private bool ValidateFoo() { ... }
    /// </code>
    /// </example>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(PublicMethodCodeFixProvider)), Shared]
    public class PublicMethodCodeFixProvider : CodeFixProvider
    {
        /// <summary>
        /// The diagnostic IDs that this code fix provider can fix (public method usage).
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            [ErrorIds.PublicMethodCanCauseConfusion];

        /// <summary>
        /// Gets the fix-all provider for batch fixing.
        /// </summary>
        /// <returns>The batch fix-all provider.</returns>
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        /// <summary>
        /// Registers code fixes for diagnostics related to public method usage in validation attributes.
        /// </summary>
        /// <param name="context">The code fix context.</param>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null) return;

            var diagnostic = context.Diagnostics.FirstOrDefault(diag => diag.Id == ErrorIds.PublicMethodCanCauseConfusion);
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
