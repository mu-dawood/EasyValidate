using EasyValidate.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;
using System.Composition;

namespace EasyValidate.Fixers
{
    /// <summary>
    /// Code fix provider for ValidationAttributeAnalyzer (EASY001, EASY002).
    /// Provides automatic fixes for validation attribute inheritance and AttributeUsage issues.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ValidationAttributeAnalyzerCodeFixProvider)), Shared]
    public class ValidationAttributeAnalyzerCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            [ErrorIds.MustInheritFromAttribute, ErrorIds.MustHaveProperAttributeUsage];

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null) return;

            foreach (var diagnostic in context.Diagnostics.Where(d => FixableDiagnosticIds.Contains(d.Id)))
            {
                var diagnosticSpan = diagnostic.Location.SourceSpan;
                var classDeclaration = root.FindToken(diagnosticSpan.Start)
                    .Parent?.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().FirstOrDefault();

                if (classDeclaration == null) continue;

                switch (diagnostic.Id)
                {
                    case ErrorIds.MustInheritFromAttribute:
                        RegisterAttributeInheritanceFix(context, root, classDeclaration, diagnostic);
                        break;
                    case ErrorIds.MustHaveProperAttributeUsage:
                        RegisterAttributeUsageFix(context, root, classDeclaration, diagnostic);
                        break;
                }
            }
        }

        private void RegisterAttributeInheritanceFix(
            CodeFixContext context,
            SyntaxNode root,
            ClassDeclarationSyntax classDeclaration,
            Diagnostic diagnostic)
        {
            var action = CodeAction.Create(
                title: "Inherit from System.Attribute",
                createChangedDocument: ct => AddSystemAttributeInheritance(context.Document, root, classDeclaration, ct),
                equivalenceKey: "InheritFromSystemAttribute");

            context.RegisterCodeFix(action, diagnostic);
        }

        private void RegisterAttributeUsageFix(
            CodeFixContext context,
            SyntaxNode root,
            ClassDeclarationSyntax classDeclaration,
            Diagnostic diagnostic)
        {
            // Offer multiple options for AttributeUsage
            var propertyOnlyAction = CodeAction.Create(
                title: "Add [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]",
                createChangedDocument: ct => AddAttributeUsage(context.Document, root, classDeclaration, "Property", ct),
                equivalenceKey: "AddAttributeUsageProperty");

            var fieldOnlyAction = CodeAction.Create(
                title: "Add [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]",
                createChangedDocument: ct => AddAttributeUsage(context.Document, root, classDeclaration, "Field", ct),
                equivalenceKey: "AddAttributeUsageField");

            var propertyAndFieldAction = CodeAction.Create(
                title: "Add [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]",
                createChangedDocument: ct => AddAttributeUsage(context.Document, root, classDeclaration, "Property | AttributeTargets.Field", ct),
                equivalenceKey: "AddAttributeUsagePropertyAndField");

            context.RegisterCodeFix(propertyOnlyAction, diagnostic);
            context.RegisterCodeFix(fieldOnlyAction, diagnostic);
            context.RegisterCodeFix(propertyAndFieldAction, diagnostic);
        }

        private async Task<Document> AddSystemAttributeInheritance(
            Document document,
            SyntaxNode root,
            ClassDeclarationSyntax classDeclaration,
            CancellationToken cancellationToken)
        {
            // Check if the class already has a base type
            BaseListSyntax? baseList = classDeclaration.BaseList;

            if (baseList == null)
            {
                // No base list exists, create one with System.Attribute
                var attributeType = SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName("Attribute"));
                var newBaseList = SyntaxFactory.BaseList(SyntaxFactory.SingletonSeparatedList<BaseTypeSyntax>(attributeType));

                var newClassDeclaration = classDeclaration.WithBaseList(newBaseList);
                var newRoot = root.ReplaceNode(classDeclaration, newClassDeclaration);

                return await AddUsingIfNeeded(document.WithSyntaxRoot(newRoot), "System", cancellationToken);
            }
            else
            {
                // Base list exists, need to insert Attribute as the first base type
                var attributeType = SyntaxFactory.SimpleBaseType(SyntaxFactory.IdentifierName("Attribute"));
                var newTypes = baseList.Types.Insert(0, attributeType);
                var newBaseList = baseList.WithTypes(newTypes);

                var newClassDeclaration = classDeclaration.WithBaseList(newBaseList);
                var newRoot = root.ReplaceNode(classDeclaration, newClassDeclaration);

                return await AddUsingIfNeeded(document.WithSyntaxRoot(newRoot), "System", cancellationToken);
            }
        }

        private async Task<Document> AddAttributeUsage(
            Document document,
            SyntaxNode root,
            ClassDeclarationSyntax classDeclaration,
            string targets,
            CancellationToken cancellationToken)
        {
            // Create the AttributeUsage attribute
            var attributeUsageAttribute = CreateAttributeUsageAttribute(targets);

            // Add the attribute to the class
            var attributeList = SyntaxFactory.SingletonList(
                SyntaxFactory.AttributeList(
                    SyntaxFactory.SingletonSeparatedList(attributeUsageAttribute)));

            var newClassDeclaration = classDeclaration.AddAttributeLists(attributeList.ToArray());
            var newRoot = root.ReplaceNode(classDeclaration, newClassDeclaration);

            return await AddUsingIfNeeded(document.WithSyntaxRoot(newRoot), "System", cancellationToken);
        }

        private AttributeSyntax CreateAttributeUsageAttribute(string targets)
        {
            // Create AttributeTargets.Property, AttributeTargets.Field, or both
            ExpressionSyntax targetsExpression;

            if (targets.Contains("|"))
            {
                // Property | Field case
                var left = SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName("AttributeTargets"),
                    SyntaxFactory.IdentifierName("Property"));

                var right = SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName("AttributeTargets"),
                    SyntaxFactory.IdentifierName("Field"));

                targetsExpression = SyntaxFactory.BinaryExpression(
                    SyntaxKind.BitwiseOrExpression, left, right);
            }
            else
            {
                // Single target case
                targetsExpression = SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName("AttributeTargets"),
                    SyntaxFactory.IdentifierName(targets));
            }

            // Create the attribute arguments
            var arguments = SyntaxFactory.SeparatedList(
            [
                // First argument: AttributeTargets
                SyntaxFactory.AttributeArgument(targetsExpression),
                // Named argument: AllowMultiple = false
                SyntaxFactory.AttributeArgument(
                    SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression))
                    .WithNameEquals(SyntaxFactory.NameEquals(SyntaxFactory.IdentifierName("AllowMultiple")))
            ]);

            return SyntaxFactory.Attribute(
                SyntaxFactory.IdentifierName("AttributeUsage"),
                SyntaxFactory.AttributeArgumentList(arguments));
        }

        private async Task<Document> AddUsingIfNeeded(Document document, string namespaceName, CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken);
            if (root == null) return document;

            var compilationUnit = root as CompilationUnitSyntax;
            if (compilationUnit == null) return document;

            // Check if using directive already exists
            var existingUsing = compilationUnit.Usings
                .FirstOrDefault(u => u.Name?.ToString() == namespaceName);

            if (existingUsing != null) return document;

            // Add the using directive
            var usingDirective = SyntaxFactory.UsingDirective(SyntaxFactory.IdentifierName(namespaceName))
                .WithTrailingTrivia(SyntaxFactory.CarriageReturnLineFeed);

            var newUsings = compilationUnit.Usings.Add(usingDirective);
            var newCompilationUnit = compilationUnit.WithUsings(newUsings);

            return document.WithSyntaxRoot(newCompilationUnit);
        }
    }
}
