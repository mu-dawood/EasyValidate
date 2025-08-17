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
    /// Code fix provider for ConditionalMethodAttributeUsage analyzer (EASY008).
    /// Provides automatic fixes for missing or incorrectly defined conditional methods.
    /// </summary>
    /// <summary>
    /// Code fix provider for EASY008 diagnostics related to conditional method usage in validation attributes.
    /// </summary>
    /// <remarks>
    /// This provider automatically creates or corrects conditional methods referenced by validation attributes' ConditionalMethod property.
    /// It offers fixes for missing methods, incorrect signatures, and incorrect return types, supporting both synchronous and asynchronous (ValueTask) patterns.
    /// </remarks>
    /// <example>
    /// <code>
    /// [MyValidation(ConditionalMethod = "ShouldValidateFoo")]
    /// public string Foo { get; set; }
    /// // Diagnostic: Conditional method 'ShouldValidateFoo' does not exist.
    /// // After fix: private bool ShouldValidateFoo(IChainResult result) => true;
    /// </code>
    /// </example>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ConditionalMethodAttributeUsageCodeFixProvider)), Shared]
    public class ConditionalMethodAttributeUsageCodeFixProvider : CodeFixProvider
    {
        /// <summary>
        /// The diagnostic IDs that this code fix provider can fix (EASY008).
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            [ErrorIds.ConditionalMethodError];

        /// <summary>
        /// Gets the fix-all provider for batch fixing.
        /// </summary>
        /// <returns>The batch fix-all provider.</returns>
        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        /// <summary>
        /// Registers code fixes for diagnostics related to conditional method usage.
        /// </summary>
        /// <param name="context">The code fix context.</param>
        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null) return;

            // Debug: Check if we have the right diagnostic
            var diagnostics = context.Diagnostics.Where(diag => diag.Id == ErrorIds.ConditionalMethodError).ToList();
            if (!diagnostics.Any()) return;

            var diagnostic = diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;

            // Find the node at the diagnostic location
            var token = root.FindToken(diagnosticSpan.Start);
            var node = token.Parent;

            // Find the attribute that contains ConditionalMethod, or find the member declaration
            AttributeSyntax? attributeNode = null;
            MemberDeclarationSyntax? memberNode = null;

            // First try to find if we're directly on an attribute
            attributeNode = node?.AncestorsAndSelf().OfType<AttributeSyntax>().FirstOrDefault();

            if (attributeNode == null)
            {
                // If not on attribute, we might be on the member itself, find the member
                memberNode = node?.AncestorsAndSelf().OfType<MemberDeclarationSyntax>()
                    .FirstOrDefault(m => m is PropertyDeclarationSyntax || m is FieldDeclarationSyntax);

                if (memberNode != null)
                {
                    // Find the attribute with ConditionalMethod on this member
                    var attributes = memberNode.AttributeLists.SelectMany(al => al.Attributes);
                    attributeNode = attributes.FirstOrDefault(attr =>
                        attr.ArgumentList?.Arguments.Any(arg =>
                            arg.NameEquals?.Name.Identifier.ValueText == "ConditionalMethod") == true);
                }
            }

            if (attributeNode == null) return;

            // Find the member containing this attribute if we don't have it yet
            if (memberNode == null)
            {
                memberNode = attributeNode.AncestorsAndSelf().OfType<MemberDeclarationSyntax>()
                    .FirstOrDefault(m => m is PropertyDeclarationSyntax || m is FieldDeclarationSyntax);
            }
            if (memberNode == null) return;

            // Find the containing class
            var classNode = attributeNode.AncestorsAndSelf().OfType<ClassDeclarationSyntax>().FirstOrDefault();
            if (classNode == null) return;

            var methodName = ExtractMethodNameFromAttribute(attributeNode);
            if (string.IsNullOrEmpty(methodName)) return;

            var semanticModel = await context.Document.GetSemanticModelAsync(context.CancellationToken).ConfigureAwait(false);
            if (semanticModel == null) return;

            // Register different fixes based on the issue
            var diagnosticMessage = diagnostic.GetMessage();

            if (diagnosticMessage.Contains("does not exist"))
            {
                var boolAction = CodeAction.Create(
                    title: $"Create conditional method '{methodName}' (bool)",
                    createChangedDocument: c => CreateConditionalMethodAsync(context.Document, classNode, methodName, c, useValueTask: false),
                    equivalenceKey: $"CreateMethodBool_{methodName}");

                var valueTaskAction = CodeAction.Create(
                    title: $"Create conditional method '{methodName}' (ValueTask<bool>)",
                    createChangedDocument: c => CreateConditionalMethodAsync(context.Document, classNode, methodName, c, useValueTask: true),
                    equivalenceKey: $"CreateMethodValueTask_{methodName}");

                context.RegisterCodeFix(boolAction, diagnostic);
                context.RegisterCodeFix(valueTaskAction, diagnostic);
            }
            else if (diagnosticMessage.Contains("must accept exactly one parameter") || diagnosticMessage.Contains("must accept a parameter of type IValidationResult"))
            {
                var action = CodeAction.Create(
                    title: $"Fix method '{methodName}' signature (add IChainResult parameter)",
                    createChangedDocument: c => FixMethodSignatureAsync(context.Document, classNode, methodName, c),
                    equivalenceKey: $"FixSignature_{methodName}");

                context.RegisterCodeFix(action, diagnostic);
            }
            else if (diagnosticMessage.Contains("must return bool"))
            {
                // Offer both bool and ValueTask<bool> as code fixes
                var boolAction = CodeAction.Create(
                    title: $"Fix method '{methodName}' return type (change to bool)",
                    createChangedDocument: c => FixMethodReturnTypeAsync(context.Document, classNode, methodName, c, useValueTask: false),
                    equivalenceKey: $"FixReturnTypeBool_{methodName}");

                var valueTaskAction = CodeAction.Create(
                    title: $"Fix method '{methodName}' return type (change to ValueTask<bool>)",
                    createChangedDocument: c => FixMethodReturnTypeAsync(context.Document, classNode, methodName, c, useValueTask: true),
                    equivalenceKey: $"FixReturnTypeValueTask_{methodName}");

                context.RegisterCodeFix(boolAction, diagnostic);
                context.RegisterCodeFix(valueTaskAction, diagnostic);
            }
        }

        private static string ExtractMethodNameFromAttribute(AttributeSyntax attributeNode)
        {
            // Look for ConditionalMethod argument in the attribute
            var argumentList = attributeNode.ArgumentList?.Arguments;
            if (argumentList == null) return string.Empty;

            foreach (var argument in argumentList)
            {
                // Check for named argument: ConditionalMethod = "MethodName"
                if (argument.NameEquals?.Name.Identifier.ValueText == "ConditionalMethod")
                {
                    if (argument.Expression is LiteralExpressionSyntax literal)
                    {
                        return literal.Token.ValueText;
                    }
                }

                // Check for named colon argument: ConditionalMethod: "MethodName" (less common)
                if (argument.NameColon?.Name.Identifier.ValueText == "ConditionalMethod")
                {
                    if (argument.Expression is LiteralExpressionSyntax literal)
                    {
                        return literal.Token.ValueText;
                    }
                }
            }

            return string.Empty;
        }

        private static async Task<Document> CreateConditionalMethodAsync(
            Document document,
            ClassDeclarationSyntax classDeclaration,
            string methodName,
            CancellationToken cancellationToken,
            bool useValueTask)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null) return document;

            MethodDeclarationSyntax methodDeclaration;
            TypeSyntax? parmterType = null;
            TypeSyntax? returnType = null;

            if (useValueTask && root is CompilationUnitSyntax compilationUnit)
            {
                if (!compilationUnit.Usings.Any(u => u.Name?.ToString() == "EasyValidate.Abstractions"))
                    parmterType = SyntaxFactory.IdentifierName("EasyValidate.Abstractions.IChainResult");
                else
                    parmterType = SyntaxFactory.IdentifierName("IChainResult");
                if (useValueTask && !compilationUnit.Usings.Any(u => u.Name?.ToString() == "System.Threading.Tasks"))
                    returnType = SyntaxFactory.ParseTypeName("System.Threading.Tasks.ValueTask<bool>");
            }
            parmterType ??= SyntaxFactory.IdentifierName("EasyValidate.Abstractions.IChainResult");
            returnType ??= useValueTask
                            ? SyntaxFactory.ParseTypeName("System.Threading.Tasks.ValueTask<bool>")
                            : SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword));



            methodDeclaration = SyntaxFactory.MethodDeclaration(
                returnType,
                SyntaxFactory.Identifier(methodName))
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)))
                .WithParameterList(SyntaxFactory.ParameterList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier("result"))
                            .WithType(parmterType))))
                .WithBody(SyntaxFactory.Block(
                    SyntaxFactory.SingletonList<StatementSyntax>(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.ObjectCreationExpression(
                                returnType)
                                .WithArgumentList(
                                    SyntaxFactory.ArgumentList(
                                        SyntaxFactory.SingletonSeparatedList(
                                            SyntaxFactory.Argument(
                                                SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression)))))))))
                .WithLeadingTrivia(
                    SyntaxFactory.Comment("/// <summary>"),
                    SyntaxFactory.EndOfLine("\n"),
                    SyntaxFactory.Comment($"/// Determines whether validation should be performed."),
                    SyntaxFactory.EndOfLine("\n"),
                    SyntaxFactory.Comment("/// </summary>"),
                    SyntaxFactory.EndOfLine("\n"),
                    SyntaxFactory.Comment("/// <param name=\"result\">The current validation result.</param>"),
                    SyntaxFactory.EndOfLine("\n"),
                    SyntaxFactory.Comment("/// <returns>A ValueTask containing true if validation should be performed; otherwise, false.</returns>"),
                    SyntaxFactory.EndOfLine("\n"));



            var newClassDeclaration = classDeclaration.AddMembers(methodDeclaration);
            var newRoot = root.ReplaceNode(classDeclaration, newClassDeclaration);
            return document.WithSyntaxRoot(newRoot);
        }

        private static async Task<Document> FixMethodSignatureAsync(
            Document document,
            ClassDeclarationSyntax classDeclaration,
            string methodName,
            CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null) return document;

            // Find the method with the specified name
            var method = classDeclaration.Members.OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(m => m.Identifier.ValueText == methodName);

            if (method == null) return document;

            // Create a new method with IChainResult parameter
            var newMethod = method.WithParameterList(SyntaxFactory.ParameterList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("result"))
                        .WithType(SyntaxFactory.IdentifierName("EasyValidate.Abstractions.IChainResult")))));

            var newClassDeclaration = classDeclaration.ReplaceNode(method, newMethod);
            var newRoot = root.ReplaceNode(classDeclaration, newClassDeclaration);

            return document.WithSyntaxRoot(newRoot);
        }

        private static async Task<Document> FixMethodReturnTypeAsync(
            Document document,
            ClassDeclarationSyntax classDeclaration,
            string methodName,
            CancellationToken cancellationToken,
            bool useValueTask)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null) return document;

            // Find the method with the specified name
            var method = classDeclaration.Members.OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(m => m.Identifier.ValueText == methodName);

            if (method == null) return document;

            MethodDeclarationSyntax newMethod;
            if (useValueTask)
            {
                // Add using for System.Threading.Tasks if not present
                var compilationUnit = root as CompilationUnitSyntax;
                if (compilationUnit != null &&
                    !compilationUnit.Usings.Any(u => u.Name?.ToString() == "System.Threading.Tasks"))
                {
                    compilationUnit = compilationUnit.AddUsings(
                        SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Threading.Tasks")));
                    root = compilationUnit;
                }
                newMethod = method.WithReturnType(
                    SyntaxFactory.ParseTypeName("ValueTask<bool>"));
            }
            else
            {
                newMethod = method.WithReturnType(
                    SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)));
            }

            var newClassDeclaration = classDeclaration.ReplaceNode(method, newMethod);
            var newRoot = root.ReplaceNode(classDeclaration, newClassDeclaration);

            return document.WithSyntaxRoot(newRoot);
        }


    }
}
