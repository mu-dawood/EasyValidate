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
    /// Works with classes, records, and structs.
    /// </remarks>
    /// <example>
    /// <code>
    /// [MyValidation(ConditionalMethod = "ShouldValidateFoo")]
    /// public string Foo { get; set; }
    /// // Diagnostic: Conditional method 'ShouldValidateFoo' does not exist.
    /// // After fix: private bool ShouldValidateFoo(IChainResult result) => true;
    /// </code>
    /// </example>

    /// <summary>
    /// Provides code fixes for diagnostics related to conditional method usage in validation attributes.
    /// Supports creating missing methods, fixing method signatures, and correcting return types.
    /// Works with classes, records, and structs.
    /// </summary>
    /// <summary>
    /// Provides code fixes for missing, invalid signature, or invalid return type for conditional methods referenced by validation attributes.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ConditionalMethodImplementationFixProvider)), Shared]
    public class ConditionalMethodImplementationFixProvider : CodeFixProvider
    {
        /// <summary>
        /// The diagnostic IDs that this code fix provider can fix.
        /// </summary>
        /// <summary>
        /// The diagnostic IDs that this code fix provider can fix (excluding InvalidConditionalMethodName).
        /// </summary>
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            [
                ErrorIds.ConditionalMethodIsMissing,
                ErrorIds.ConditionalMethodInvalidParameterLength,
                ErrorIds.ConditionalMethodFirstParameterTypeMismatch,
                ErrorIds.ConditionalMethodReturnTypeMismatch,
            ];

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

            foreach (var diagnostic in context.Diagnostics.Where(d => FixableDiagnosticIds.Contains(d.Id)))
            {
                var diagnosticSpan = diagnostic.Location.SourceSpan;
                var token = root.FindToken(diagnosticSpan.Start);
                var node = token.Parent;

                // Use diagnostic location to find the correct node for each error type
                switch (diagnostic.Id)
                {
                    case var id when id == ErrorIds.ConditionalMethodIsMissing:
                        // Attribute location: find attribute, class, and method name
                        var attributeNode = node?.AncestorsAndSelf().OfType<AttributeSyntax>().FirstOrDefault();
                        if (attributeNode == null) continue;
                        var typeDeclaration = attributeNode.AncestorsAndSelf().OfType<TypeDeclarationSyntax>().FirstOrDefault();
                        if (typeDeclaration == null) continue;
                        var methodName = ExtractMethodNameFromAttribute(attributeNode);
                        if (string.IsNullOrEmpty(methodName)) continue;
                        var boolAction = CodeAction.Create(
                            title: $"Create conditional method '{methodName}' (bool)",
                            createChangedDocument: c => CreateConditionalMethodAsync(context.Document, typeDeclaration, methodName, false, c),
                            equivalenceKey: $"CreateMethodBool_{methodName}");
                        var valueTaskAction = CodeAction.Create(
                            title: $"Create conditional method '{methodName}' (ValueTask<bool>)",
                            createChangedDocument: c => CreateConditionalMethodAsync(context.Document, typeDeclaration, methodName, true, c),
                            equivalenceKey: $"CreateMethodValueTask_{methodName}");
                        context.RegisterCodeFix(boolAction, diagnostic);
                        context.RegisterCodeFix(valueTaskAction, diagnostic);
                        break;
                    case var id when id == ErrorIds.ConditionalMethodInvalidParameterLength:
                        // Method location: find method and class
                        var methodNode = node?.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().FirstOrDefault();
                        if (methodNode == null) continue;
                        var typeDeclaration2 = methodNode.AncestorsAndSelf().GetClassStructOrRecord();
                        if (typeDeclaration2 == null) continue;
                        var methodName2 = methodNode.Identifier.ValueText;
                        var signatureAction = CodeAction.Create(
                            title: $"Fix method '{methodName2}' signature (add IChainResult parameter)",
                            createChangedDocument: c => FixMethodSignatureAsync(context.Document, typeDeclaration2, methodName2, c),
                            equivalenceKey: $"FixSignature_{methodName2}");
                        context.RegisterCodeFix(signatureAction, diagnostic);
                        break;
                    case var id when id == ErrorIds.ConditionalMethodFirstParameterTypeMismatch:
                        // Parameter location: find parameter, method, and class
                        var parameterNode = node?.AncestorsAndSelf().OfType<ParameterSyntax>().FirstOrDefault();
                        var methodNode3 = parameterNode?.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().FirstOrDefault();
                        if (methodNode3 == null) continue;
                        var typeDeclaration3 = methodNode3.AncestorsAndSelf().GetClassStructOrRecord();
                        if (typeDeclaration3 == null) continue;
                        var methodName3 = methodNode3.Identifier.ValueText;
                        var signatureAction2 = CodeAction.Create(
                            title: $"Fix method '{methodName3}' parameter type (set to IChainResult)",
                            createChangedDocument: c => FixMethodSignatureAsync(context.Document, typeDeclaration3, methodName3, c),
                            equivalenceKey: $"FixParameterType_{methodName3}");
                        context.RegisterCodeFix(signatureAction2, diagnostic);
                        break;
                    case var id when id == ErrorIds.ConditionalMethodReturnTypeMismatch:
                        // Return type location: find method and class
                        var returnTypeNode = node?.AncestorsAndSelf().OfType<MethodDeclarationSyntax>().FirstOrDefault();
                        if (returnTypeNode == null) continue;
                        var typeDeclaration4 = returnTypeNode.AncestorsAndSelf().GetClassStructOrRecord();
                        if (typeDeclaration4 == null) continue;
                        var methodName4 = returnTypeNode.Identifier.ValueText;
                        var boolReturnAction = CodeAction.Create(
                            title: $"Fix method '{methodName4}' return type (change to bool)",
                            createChangedDocument: c => FixMethodReturnTypeAsync(context.Document, typeDeclaration4, methodName4, c, useValueTask: false),
                            equivalenceKey: $"FixReturnTypeBool_{methodName4}");
                        var valueTaskReturnAction = CodeAction.Create(
                            title: $"Fix method '{methodName4}' return type (change to ValueTask<bool>)",
                            createChangedDocument: c => FixMethodReturnTypeAsync(context.Document, typeDeclaration4, methodName4, c, useValueTask: true),
                            equivalenceKey: $"FixReturnTypeValueTask_{methodName4}");
                        context.RegisterCodeFix(boolReturnAction, diagnostic);
                        context.RegisterCodeFix(valueTaskReturnAction, diagnostic);
                        break;
                }
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

        /// <summary>
        /// Creates a conditional method with the specified name and signature in the given type.
        /// </summary>
        /// <param name="document">The document to update.</param>
        /// <param name="typeDeclaration">The type declaration to add the method to.</param>
        /// <param name="methodName">The name of the method to create.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <param name="useValueTask">Whether to use ValueTask&lt;bool&gt; as the return type.</param>
        /// <returns>The updated document with the new method added.</returns>
        internal static async Task<Document> CreateConditionalMethodAsync(
                Document document,
                TypeDeclarationSyntax typeDeclaration,
                string methodName,
                bool useValueTask,
                CancellationToken cancellationToken
        )
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null) return document;

            MethodDeclarationSyntax methodDeclaration;
            TypeSyntax parmterType = SyntaxFactory.IdentifierName("EasyValidate.Abstractions.IChainResult");
            TypeSyntax returnType = useValueTask
                            ? SyntaxFactory.ParseTypeName("System.Threading.Tasks.ValueTask<bool>")
                            : SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword));

            if (root is CompilationUnitSyntax compilationUnit)
            {
                if (!compilationUnit.Usings.Any(u => u.Name?.ToString() == "EasyValidate.Abstractions"))
                {
                    var newUsing = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("EasyValidate.Abstractions"))
                        .WithTrailingTrivia(SyntaxFactory.ElasticCarriageReturnLineFeed);
                    compilationUnit = compilationUnit.WithUsings(compilationUnit.Usings.Add(newUsing));
                    parmterType = SyntaxFactory.IdentifierName("IChainResult");
                }
                if (useValueTask && !compilationUnit.Usings.Any(u => u.Name?.ToString() == "System.Threading.Tasks"))
                {
                    var newUsing = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("System.Threading.Tasks"))
                        .WithTrailingTrivia(SyntaxFactory.ElasticCarriageReturnLineFeed);
                    compilationUnit = compilationUnit.WithUsings(compilationUnit.Usings.Add(newUsing));
                    returnType = SyntaxFactory.ParseTypeName("ValueTask<bool>");
                }
                root = compilationUnit;
            }

            StatementSyntax returnStatement;
            if (useValueTask)
            {
                // return new ValueTask<bool>(true);
                returnStatement = SyntaxFactory.ReturnStatement(
                    SyntaxFactory.ObjectCreationExpression(
                        SyntaxFactory.ParseTypeName("ValueTask<bool>"))
                    .WithArgumentList(
                        SyntaxFactory.ArgumentList(
                            SyntaxFactory.SingletonSeparatedList(
                                SyntaxFactory.Argument(
                                    SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression))))));
            }
            else
            {
                // return true;
                returnStatement = SyntaxFactory.ReturnStatement(
                    SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression));
            }

            methodDeclaration = SyntaxFactory.MethodDeclaration(
                returnType,
                SyntaxFactory.Identifier(methodName))
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)))
                .WithParameterList(SyntaxFactory.ParameterList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier("result"))
                            .WithType(parmterType))))
                .WithBody(SyntaxFactory.Block(returnStatement))
                .WithLeadingTrivia(
                    SyntaxFactory.Comment("/// <summary>"),
                    SyntaxFactory.EndOfLine("\n"),
                    SyntaxFactory.Comment($"/// Determines whether validation should be performed."),
                    SyntaxFactory.EndOfLine("\n"),
                    SyntaxFactory.Comment("/// </summary>"),
                    SyntaxFactory.EndOfLine("\n"),
                    SyntaxFactory.Comment("/// <param name=\"result\">The current validation result.</param>"),
                    SyntaxFactory.EndOfLine("\n"),
                    SyntaxFactory.Comment("/// <returns>True if validation should be performed; otherwise, false.</returns>"),
                    SyntaxFactory.EndOfLine("\n"));



            var newTypeDeclaration = typeDeclaration.AddMembers(methodDeclaration);
            var newRoot = root.ReplaceNode(typeDeclaration, newTypeDeclaration);
            return document.WithSyntaxRoot(newRoot);
        }

        private static async Task<Document> FixMethodSignatureAsync(
            Document document,
            TypeDeclarationSyntax typeDeclaration,
            string methodName,
            CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null) return document;

            // Find the method with the specified name
            var method = typeDeclaration.Members.OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(m => m.Identifier.ValueText == methodName);

            if (method == null) return document;

            if (root is CompilationUnitSyntax compilationUnit)
            {
                if (!compilationUnit.Usings.Any(u => u.Name?.ToString() == "EasyValidate.Abstractions"))
                {
                    var newUsing = SyntaxFactory.UsingDirective(SyntaxFactory.ParseName("EasyValidate.Abstractions"))
                        .WithTrailingTrivia(SyntaxFactory.ElasticCarriageReturnLineFeed);
                    compilationUnit = compilationUnit.WithUsings(compilationUnit.Usings.Add(newUsing));
                }
                root = compilationUnit;
            }
            // Create a new method with IChainResult parameter
            var newMethod = method.WithParameterList(SyntaxFactory.ParameterList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("result"))
                        .WithType(SyntaxFactory.IdentifierName("IChainResult")))));

            var newTypeDeclaration = typeDeclaration.ReplaceNode(method, newMethod);
            var newRoot = root.ReplaceNode(typeDeclaration, newTypeDeclaration);

            return document.WithSyntaxRoot(newRoot);
        }

        private static async Task<Document> FixMethodReturnTypeAsync(
            Document document,
            TypeDeclarationSyntax typeDeclaration,
            string methodName,
            CancellationToken cancellationToken,
            bool useValueTask)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null) return document;

            // Find the method with the specified name
            var method = typeDeclaration.Members.OfType<MethodDeclarationSyntax>()
                .FirstOrDefault(m => m.Identifier.ValueText == methodName);

            if (method == null) return document;

            MethodDeclarationSyntax newMethod;
            if (useValueTask)
            {
                // Add using for System.Threading.Tasks if not present
                if (root is CompilationUnitSyntax compilationUnit &&
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

            var newTypeDeclaration = typeDeclaration.ReplaceNode(method, newMethod);
            var newRoot = root.ReplaceNode(typeDeclaration, newTypeDeclaration);

            return document.WithSyntaxRoot(newRoot);
        }


    }
}
