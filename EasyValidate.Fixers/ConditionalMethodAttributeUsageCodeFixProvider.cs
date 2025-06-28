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
    /// Code fix provider for ConditionalMethodAttributeUsage analyzer (EASY008).
    /// Provides automatic fixes for missing or incorrectly defined conditional methods.
    /// </summary>
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ConditionalMethodAttributeUsageCodeFixProvider)), Shared]
    public class ConditionalMethodAttributeUsageCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            [ErrorIds.ConditionalMethodError];

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

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
                var action = CodeAction.Create(
                    title: $"Create conditional method '{methodName}'",
                    createChangedDocument: c => CreateConditionalMethodAsync(context.Document, classNode, methodName, c),
                    equivalenceKey: $"CreateMethod_{methodName}");

                context.RegisterCodeFix(action, diagnostic);
            }
            else if (diagnosticMessage.Contains("must accept exactly one parameter") || diagnosticMessage.Contains("must accept a parameter of type IValidationResult"))
            {
                var action = CodeAction.Create(
                    title: $"Fix method '{methodName}' signature (add IValidationResult parameter)",
                    createChangedDocument: c => FixMethodSignatureAsync(context.Document, classNode, methodName, c),
                    equivalenceKey: $"FixSignature_{methodName}");

                context.RegisterCodeFix(action, diagnostic);
            }
            else if (diagnosticMessage.Contains("must return bool"))
            {
                var action = CodeAction.Create(
                    title: $"Fix method '{methodName}' return type (change to bool)",
                    createChangedDocument: c => FixMethodReturnTypeAsync(context.Document, classNode, methodName, c),
                    equivalenceKey: $"FixReturnType_{methodName}");

                context.RegisterCodeFix(action, diagnostic);
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
            CancellationToken cancellationToken)
        {
            var root = await document.GetSyntaxRootAsync(cancellationToken).ConfigureAwait(false);
            if (root == null) return document;

            // Create the conditional method with IValidationResult parameter
            var methodDeclaration = SyntaxFactory.MethodDeclaration(
                SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)),
                SyntaxFactory.Identifier(methodName))
                .WithModifiers(SyntaxFactory.TokenList(SyntaxFactory.Token(SyntaxKind.PrivateKeyword)))
                .WithParameterList(SyntaxFactory.ParameterList(
                    SyntaxFactory.SingletonSeparatedList(
                        SyntaxFactory.Parameter(SyntaxFactory.Identifier("result"))
                            .WithType(SyntaxFactory.IdentifierName("EasyValidate.Core.Abstraction.IValidationResult")))))
                .WithBody(SyntaxFactory.Block(
                    SyntaxFactory.SingletonList<StatementSyntax>(
                        SyntaxFactory.ReturnStatement(
                            SyntaxFactory.LiteralExpression(SyntaxKind.TrueLiteralExpression)))))
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

            // Add the method to the class
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

            // Create a new method with IValidationResult parameter
            var newMethod = method.WithParameterList(SyntaxFactory.ParameterList(
                SyntaxFactory.SingletonSeparatedList(
                    SyntaxFactory.Parameter(SyntaxFactory.Identifier("result"))
                        .WithType(SyntaxFactory.IdentifierName("EasyValidate.Core.Abstraction.IValidationResult")))));

            var newClassDeclaration = classDeclaration.ReplaceNode(method, newMethod);
            var newRoot = root.ReplaceNode(classDeclaration, newClassDeclaration);

            return document.WithSyntaxRoot(newRoot);
        }

        private static async Task<Document> FixMethodReturnTypeAsync(
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

            // Create a new method with bool return type
            var newMethod = method.WithReturnType(
                SyntaxFactory.PredefinedType(SyntaxFactory.Token(SyntaxKind.BoolKeyword)));
            
            var newClassDeclaration = classDeclaration.ReplaceNode(method, newMethod);
            var newRoot = root.ReplaceNode(classDeclaration, newClassDeclaration);

            return document.WithSyntaxRoot(newRoot);
        }

       
    }
}
