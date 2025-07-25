using System.Composition;
using EasyValidate.Analyzers;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeActions;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.CSharp;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using System.Collections.Immutable;

namespace EasyValidate.Fixers
{
    [ExportCodeFixProvider(LanguageNames.CSharp, Name = nameof(ConditionalMethodStrategyCodeFixProvider)), Shared]
    public class ConditionalMethodStrategyCodeFixProvider : CodeFixProvider
    {
        public sealed override ImmutableArray<string> FixableDiagnosticIds =>
            [ErrorIds.ConditionalMethodInvalidStrategyError, ErrorIds.ConditionalMethodMissingError];

        public sealed override FixAllProvider GetFixAllProvider() => WellKnownFixAllProviders.BatchFixer;

        public sealed override async Task RegisterCodeFixesAsync(CodeFixContext context)
        {
            var root = await context.Document.GetSyntaxRootAsync(context.CancellationToken).ConfigureAwait(false);
            if (root == null) return;

            var diagnostic = context.Diagnostics.First();
            var diagnosticSpan = diagnostic.Location.SourceSpan;
            var token = root.FindToken(diagnosticSpan.Start);
            var node = token.Parent;

            var attributeNode = node?.AncestorsAndSelf().OfType<AttributeSyntax>().FirstOrDefault();
            if (attributeNode == null) return;

            var memberNode = attributeNode.AncestorsAndSelf().OfType<MemberDeclarationSyntax>()
                .FirstOrDefault(m => m is PropertyDeclarationSyntax || m is FieldDeclarationSyntax);
            if (memberNode == null) return;

            var propertyName = memberNode is PropertyDeclarationSyntax prop ? prop.Identifier.ValueText :
                memberNode is FieldDeclarationSyntax field && field.Declaration.Variables.Count > 0 ? field.Declaration.Variables[0].Identifier.ValueText : "Property";

            if (diagnostic.Id == ErrorIds.ConditionalMethodMissingError)
            {
                // Add ConditionalMethod property to attribute
                var newMethodName = $"ShouldValidate{propertyName}";
                var newAttribute = AddOrUpdateNamedArgument(attributeNode, "ConditionalMethod", SyntaxFactory.LiteralExpression(SyntaxKind.StringLiteralExpression, SyntaxFactory.Literal(newMethodName)));
                var newRoot = root.ReplaceNode(attributeNode, newAttribute);
                context.RegisterCodeFix(
                    CodeAction.Create(
                        $"Add ConditionalMethod=\"{newMethodName}\" to attribute",
                        c => Task.FromResult(context.Document.WithSyntaxRoot(newRoot)),
                        nameof(ConditionalMethodStrategyCodeFixProvider) + ".AddConditionalMethod"),
                    diagnostic);
            }
            else if (diagnostic.Id == ErrorIds.ConditionalMethodInvalidStrategyError)
            {
                // Register two code fixes:
                // 1. Set Strategy to ExecutionStrategy.ConditionalAndStopChain
                var strategyExprStop = SyntaxFactory.ParseExpression("EasyValidate.Core.Abstraction.ExecutionStrategy.ConditionalAndStopChain");
                var newAttributeStop = AddOrUpdateNamedArgument(attributeNode, "Strategy", strategyExprStop);
                var newRootStop = root.ReplaceNode(attributeNode, newAttributeStop);
                context.RegisterCodeFix(
                    CodeAction.Create(
                        $"Set Strategy=ExecutionStrategy.ConditionalAndStopChain",
                        c => Task.FromResult(context.Document.WithSyntaxRoot(newRootStop)),
                        nameof(ConditionalMethodStrategyCodeFixProvider) + ".SetStrategyStop"),
                    diagnostic);

                // 2. Set Strategy to ExecutionStrategy.ConditionalAndContinue
                var strategyExprContinue = SyntaxFactory.ParseExpression("EasyValidate.Core.Abstraction.ExecutionStrategy.ConditionalAndContinue");
                var newAttributeContinue = AddOrUpdateNamedArgument(attributeNode, "Strategy", strategyExprContinue);
                var newRootContinue = root.ReplaceNode(attributeNode, newAttributeContinue);
                context.RegisterCodeFix(
                    CodeAction.Create(
                        $"Set Strategy=ExecutionStrategy.ConditionalAndContinue",
                        c => Task.FromResult(context.Document.WithSyntaxRoot(newRootContinue)),
                        nameof(ConditionalMethodStrategyCodeFixProvider) + ".SetStrategyContinue"),
                    diagnostic);
            }
        }

        private static AttributeSyntax AddOrUpdateNamedArgument(AttributeSyntax attribute, string name, ExpressionSyntax value)
        {
            var args = attribute.ArgumentList?.Arguments ?? new SeparatedSyntaxList<AttributeArgumentSyntax>();
            var existing = args.FirstOrDefault(a => a.NameEquals?.Name.Identifier.ValueText == name);
            if (existing != null)
            {
                var newArg = existing.WithExpression(value);
                var newArgs = args.Replace(existing, newArg);
                return attribute.WithArgumentList(attribute.ArgumentList?.WithArguments(newArgs));
            }
            else
            {
                var newArg = SyntaxFactory.AttributeArgument(value).WithNameEquals(SyntaxFactory.NameEquals(name));
                var newArgs = args.Add(newArg);
                return attribute.WithArgumentList(attribute.ArgumentList != null ? attribute.ArgumentList.WithArguments(newArgs) : SyntaxFactory.AttributeArgumentList(newArgs));
            }
        }
    }
}
