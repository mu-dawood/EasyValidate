using EasyValidate.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace EasyValidate.Fixers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(InterfaceImplementationCodeFixProvider)), Shared]
    public class InterfaceImplementationCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            [ErrorIds.ValidateAttributeUsageAsyncInterface, ErrorIds.ValidateAttributeUsageSyncInterface];

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

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

            if (diagnostic.Id == ErrorIds.ValidateAttributeUsageAsyncInterface)
            {
                var action = CodeAction.Create(
                    title: "Add IAsyncValidate interface",
                    createChangedDocument: c => AddInterfaceAsync(context.Document, classNode, "IAsyncValidate", c),
                    equivalenceKey: "AddIAsyncValidate");
                context.RegisterCodeFix(action, diagnostic);
            }
            else if (diagnostic.Id == ErrorIds.ValidateAttributeUsageSyncInterface)
            {
                var action = CodeAction.Create(
                    title: "Add IValidate interface",
                    createChangedDocument: c => AddInterfaceAsync(context.Document, classNode, "IValidate", c),
                    equivalenceKey: "AddIValidate");
                context.RegisterCodeFix(action, diagnostic);
            }
        }

        private static async Task<Document> AddInterfaceAsync(
            Document document,
            ClassDeclarationSyntax classDeclaration,
            string interfaceName,
            CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null) return document;

            // Add using if needed
            if (root is CompilationUnitSyntax compilationUnit &&
                !compilationUnit.Usings.Any(u => u.Name?.ToString() == "EasyValidate.Core.Abstraction"))
            {
                var newUsing = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("EasyValidate.Core.Abstraction"))
                                            .WithTrailingTrivia(SyntaxFactory.ElasticCarriageReturnLineFeed);

                compilationUnit = compilationUnit.WithUsings(compilationUnit.Usings.Add(newUsing));
                root = compilationUnit;

                // Re-locate classDeclaration inside the updated root
                classDeclaration = root.DescendantNodes()
                                       .OfType<ClassDeclarationSyntax>()
                                       .First(c => c.Identifier.Text == classDeclaration.Identifier.Text);
            }

            // If interface already present, just return
            var interfaces = classDeclaration.BaseList?.Types.Select(t => t.ToString()).ToList() ?? [];
            if (interfaces.Contains(interfaceName))
                return document;

            ClassDeclarationSyntax? newClassDeclaration;

            if (classDeclaration.BaseList == null)
            {
                var colon = SyntaxFactory.Token(SyntaxKind.ColonToken)
                    .WithLeadingTrivia(SyntaxFactory.Space)
                    .WithTrailingTrivia(SyntaxFactory.Space);
                var newBaseList = SyntaxFactory.BaseList(colon,
                     SyntaxFactory.SeparatedList<BaseTypeSyntax>([SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName(interfaceName))])
                 );
                newClassDeclaration = classDeclaration.WithBaseList(newBaseList);
            }
            else
            {
                var newType = SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName(interfaceName))
                                            .WithLeadingTrivia(SyntaxFactory.Space);
                var separated = classDeclaration.BaseList.Types.Add(newType);
                var newBaseList = classDeclaration.BaseList.WithTypes(separated);
                newClassDeclaration = classDeclaration
                      .WithBaseList(newBaseList)
                      .WithAdditionalAnnotations(Microsoft.CodeAnalysis.Formatting.Formatter.Annotation);

            }
            var newRoot = root.ReplaceNode(classDeclaration, newClassDeclaration);
            return document.WithSyntaxRoot(newRoot);
        }
    }
}
