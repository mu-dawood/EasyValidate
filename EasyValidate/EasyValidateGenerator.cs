using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using Microsoft.CodeAnalysis;
using EasyValidate.Handlers;

namespace EasyValidate
{
    [Generator]
    public class EasyValidateGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            LogDebug("Initializing EasyValidateGenerator...");

            var compilationProvider = context.CompilationProvider;

            var candidates = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (node, _) => node is ClassDeclarationSyntax,
                    transform: static (ctx, _) => ctx.SemanticModel.GetDeclaredSymbol((ClassDeclarationSyntax)ctx.Node) as INamedTypeSymbol
                )
                .Where(static symbol => symbol != null)
                .Combine(compilationProvider);

            context.RegisterSourceOutput(candidates, static (spc, pair) =>
            {
                var (classSymbol, compilation) = pair;

                if (classSymbol is not INamedTypeSymbol classSymbolNonNull)
                    return;

                GenerateValidationClass(classSymbolNonNull, spc);
            });
        }

        private static void GenerateValidationClass(INamedTypeSymbol classSymbol, SourceProductionContext context)
        {
            // Skip classes that have no properties with validation attributes
            if (!classSymbol.GetMembers().OfType<Microsoft.CodeAnalysis.IPropertySymbol>()
                    .Any(p => p.GetAttributes().Any(a => a.AttributeClass != null &&
                        a.AttributeClass.ContainingNamespace.ToDisplayString().StartsWith("EasyValidate.Abstraction.Attributes"))))
            {
                return;
            }

            var sb = new StringBuilder();
            var chain = new ValidationChain()
                .Add(new ValidateAttributeUsageHandler())
                .Add(new UsingImportsHandler())
                .Add(new NamespaceHandler())
                .Add(new ClassDeclarationHandler())
                .Add(new DefaultValidateMethodHandler())
                .Add(new CustomValidateMethodHandler());

            chain.Handle(classSymbol, context, sb);
            context.AddSource($"{classSymbol.Name}_Validation.g.cs", SourceText.From(sb.ToString(), Encoding.UTF8));
        }

        private static void LogDebug(string message)
        {
#if DEBUG
            Debugger.Log(0, "Debug", $"[DEBUG] {message}\n");
#endif
        }
    }
}