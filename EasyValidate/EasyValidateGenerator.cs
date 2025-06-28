using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using EasyValidate.Handlers;

namespace EasyValidate
{
    [Generator]
    public class EasyValidateGenerator : IIncrementalGenerator
    {
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            DebuggerUtil.AttachDebugger();
            DebuggerUtil.Log("Initializing EasyValidateGenerator...");
            DebuggerUtil.Log("Starting Initialize method.");

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

                DebuggerUtil.Log($"Processing class: {classSymbolNonNull.Name}");
                GenerateValidationClass(classSymbolNonNull, compilation, spc);
            });

            DebuggerUtil.Log("Finished setting up syntax provider and compilation provider.");
            DebuggerUtil.Log("Finished Initialize method.");
        }

        private static void GenerateValidationClass(INamedTypeSymbol classSymbol, Compilation compilation, SourceProductionContext context)
        {
            DebuggerUtil.Log($"Generating validation class for: {classSymbol.Name}");

            // Skip classes that have no properties with validation attributes
            // Check if the class has any properties with attributes derived from IValidationAttribute
            var member = classSymbol.GetMembers();
            var hasPropertySymbols = member.OfType<IPropertySymbol>()
                .Any(p => p.GetAttributes().Any(a => a.AttributeClass?.IsValidationAttribute() == true));
            var hasFieldSymbols = member.OfType<IFieldSymbol>()
                .Any(f => f.GetAttributes().Any(a => a.AttributeClass?.IsValidationAttribute() == true));
            if (!hasPropertySymbols && !hasFieldSymbols)
            {
                DebuggerUtil.Log($"Skipping class {classSymbol.Name} as it has no properties with validation attributes derived from IValidationAttribute.");
                return;
            }

            var sb = new StringBuilder();
            var chain = new GeneratorChain()
                .Add(new UsingImportsHandler())
                .Add(new NamespaceHandler())
                .Add(new ClassDeclarationHandler())
                .Add(new ValidateMethodOverlodsHandler())
                .Add(new ValidateMethodHandler(compilation))
                .Add(new PropertyValidationMethodHandler(compilation));

            chain.Handle(classSymbol, context, sb);

            var namespacePath = classSymbol.ContainingNamespace.ToDisplayString().Replace('.', '/');
            var fileName = $"{classSymbol.Name}_Validation.g.cs";
            context.AddSource($"{namespacePath}/{fileName}", SourceText.From(sb.ToString(), Encoding.UTF8));

            DebuggerUtil.Log($"Successfully generated validation class for: {classSymbol.Name}");
        }

    }
}