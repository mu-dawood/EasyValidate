using System;
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
    /// Code fix provider for EASY001 and EASY002 diagnostics related to validation attribute inheritance and AttributeUsage configuration.
    /// </summary>
    /// <remarks>
    /// This provider automatically adds inheritance from System.Attribute and proper AttributeUsage attributes to validation attribute classes.
    /// It registers multiple code fixes for different valid AttributeUsage configurations and ensures required usings are present.
    /// </remarks>
    /// <example>
    /// <code>
    /// public class MyValidationAttribute : IValidationAttribute { ... }
    /// // Diagnostic: Must inherit from System.Attribute.
    /// // After fix: public class MyValidationAttribute : Attribute, IValidationAttribute { ... }
    /// </code>
    /// <code>
    /// public class MyValidationAttribute : Attribute { ... }
    /// // Diagnostic: Must have proper AttributeUsage.
    /// // After fix: [AttributeUsage(AttributeTargets.Property, AllowMultiple = true)]
    /// public class MyValidationAttribute : Attribute { ... }
    /// </code>
    /// </example>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ValidationAttributeAnalyzerCodeFixProvider)), Shared]
    public class ValidationAttributeAnalyzerCodeFixProvider : CodeFixProvider
    {
        /// <summary>
        /// The diagnostic IDs that this code fix provider can fix (EASY001, EASY002).
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            [ErrorIds.AttributeMustInheritFromSystemAttribute, ErrorIds.AttributeMustHaveProperUsage];

        /// <summary>
        /// Gets the fix-all provider for batch fixing.
        /// </summary>
        /// <returns>The batch fix-all provider.</returns>
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        /// <summary>
        /// Registers code fixes for diagnostics related to validation attribute inheritance and AttributeUsage configuration.
        /// </summary>
        /// <param name="context">The code fix context.</param>
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
                    case ErrorIds.AttributeMustInheritFromSystemAttribute:
                        RegisterAttributeInheritanceFix(context, root, classDeclaration, diagnostic);
                        break;
                    case ErrorIds.AttributeMustHaveProperUsage:
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

            var parameterOnlyAction = CodeAction.Create(
                title: "Add [AttributeUsage(AttributeTargets.Parameter, AllowMultiple = true)]",
                createChangedDocument: ct => AddAttributeUsage(context.Document, root, classDeclaration, "Parameter", ct),
                equivalenceKey: "AddAttributeUsageParameter");

            var propertyAndFieldAction = CodeAction.Create(
                title: "Add [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field, AllowMultiple = true)]",
                createChangedDocument: ct => AddAttributeUsage(context.Document, root, classDeclaration, "Property | AttributeTargets.Field", ct),
                equivalenceKey: "AddAttributeUsagePropertyAndField");

            var propertyAndParameterAction = CodeAction.Create(
                title: "Add [AttributeUsage(AttributeTargets.Property | AttributeTargets.Parameter, AllowMultiple = true)]",
                createChangedDocument: ct => AddAttributeUsage(context.Document, root, classDeclaration, "Property | AttributeTargets.Parameter", ct),
                equivalenceKey: "AddAttributeUsagePropertyAndParameter");

            var fieldAndParameterAction = CodeAction.Create(
                title: "Add [AttributeUsage(AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]",
                createChangedDocument: ct => AddAttributeUsage(context.Document, root, classDeclaration, "Field | AttributeTargets.Parameter", ct),
                equivalenceKey: "AddAttributeUsageFieldAndParameter");

            var allTargetsAction = CodeAction.Create(
                title: "Add [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = true)]",
                createChangedDocument: ct => AddAttributeUsage(context.Document, root, classDeclaration, "Property | AttributeTargets.Field | AttributeTargets.Parameter", ct),
                equivalenceKey: "AddAttributeUsageAll");

            context.RegisterCodeFix(propertyOnlyAction, diagnostic);
            context.RegisterCodeFix(fieldOnlyAction, diagnostic);
            context.RegisterCodeFix(parameterOnlyAction, diagnostic);
            context.RegisterCodeFix(propertyAndFieldAction, diagnostic);
            context.RegisterCodeFix(propertyAndParameterAction, diagnostic);
            context.RegisterCodeFix(fieldAndParameterAction, diagnostic);
            context.RegisterCodeFix(allTargetsAction, diagnostic);
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
            // Create AttributeTargets expression based on the targets string
            ExpressionSyntax targetsExpression = CreateTargetsExpression(targets);

            // Create the attribute arguments
            var arguments = SyntaxFactory.SeparatedList(
            [
                // First argument: AttributeTargets
                SyntaxFactory.AttributeArgument(targetsExpression),
                // Named argument: AllowMultiple = true
                SyntaxFactory.AttributeArgument(
                    SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression))
                    .WithNameEquals(SyntaxFactory.NameEquals(SyntaxFactory.IdentifierName("AllowMultiple")))
            ]);

            return SyntaxFactory.Attribute(
                SyntaxFactory.IdentifierName("AttributeUsage"),
                SyntaxFactory.AttributeArgumentList(arguments));
        }

        private ExpressionSyntax CreateTargetsExpression(string targets)
        {
            // Split targets by " | AttributeTargets." to get individual target names
            var targetParts = targets.Split(new[] { " | AttributeTargets." }, StringSplitOptions.RemoveEmptyEntries);

            if (targetParts.Length == 1)
            {
                // Single target case
                return SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName("AttributeTargets"),
                    SyntaxFactory.IdentifierName(targetParts[0]));
            }

            // Multiple targets case - build binary OR expressions
            ExpressionSyntax result = SyntaxFactory.MemberAccessExpression(
                SyntaxKind.SimpleMemberAccessExpression,
                SyntaxFactory.IdentifierName("AttributeTargets"),
                SyntaxFactory.IdentifierName(targetParts[0]));

            for (int i = 1; i < targetParts.Length; i++)
            {
                var rightTarget = SyntaxFactory.MemberAccessExpression(
                    SyntaxKind.SimpleMemberAccessExpression,
                    SyntaxFactory.IdentifierName("AttributeTargets"),
                    SyntaxFactory.IdentifierName(targetParts[i]));

                result = SyntaxFactory.BinaryExpression(
                    SyntaxKind.BitwiseOrExpression,
                    result,
                    rightTarget);
            }

            return result;
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
