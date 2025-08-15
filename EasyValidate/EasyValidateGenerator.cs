using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using Microsoft.CodeAnalysis;
using EasyValidate.Generator.Handlers;
using System.Collections.Generic;
using System;
using Microsoft.CodeAnalysis.CSharp;
using EasyValidate.Generator.Types;
using EasyValidate.Generator.Handlers.Methods;
using EasyValidate.Generator.Helpers;

namespace EasyValidate.Generator
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
                    transform: static (ctx, _) => ctx.SemanticModel.GetDeclaredSymbol((ClassDeclarationSyntax)ctx.Node)
                )
                .Where(static symbol => symbol != null)
                .Combine(compilationProvider);

            context.RegisterSourceOutput(candidates, static (spc, pair) =>
            {
                var (classSymbol, compilation) = pair;

                if (classSymbol is not INamedTypeSymbol classSymbolNonNull)
                    return;
                if (!classSymbol.ImplementsIGenerate())
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
            var argumentHandler = new AttributeArgumentHandler();
            Dictionary<string, string> instanceNames = [];
            var finalizer = new MembersFinalizer(context, classSymbol, argumentHandler, instanceNames);
            try
            {
                // Skip classes that have no properties with validation attributes
                // Check if the class has any properties with attributes derived from IValidationAttribute
                var members = classSymbol.GetMembers().OrderBy(m => m is IFieldSymbol ? 0 : 1).ToList(); // Order properties first, then fields
                var target = new ValidationTarget(classSymbol);
                var infos = finalizer.Finalize(members, compilation);
                if (infos.Count > 0)
                    target = target.WithMembers(infos);

                List<MethodTarget> methodTargets = [];
                foreach (var member in classSymbol.GetMembers())
                {
                    /// get parameters for methods to make validation for it
                    if (member is IMethodSymbol method && method.MethodKind == MethodKind.Ordinary)
                    {
                        var methodParameters = method.Parameters;
                        if (methodParameters.Length > 0)
                        {
                            // Process method parameters instead of all class members
                            var parameterInfos = finalizer.Finalize(methodParameters, compilation);
                            if (parameterInfos.Count > 0)
                                methodTargets.Add(new MethodTarget(method, parameterInfos));
                        }
                    }

                }
                if (methodTargets.Count > 0)
                    target = target.WithMethods(methodTargets);

                if (!target.NeedGeneration)
                    return;

                var chain = new GeneratorChain(new UsingImportsHandler())
                .Add(new NamespaceHandler())
                .Add(new ClassDeclarationHandler())
                .Add(new ReusableInstancesHandler())
                .Add(new ValidateMethodOverloadsHandler())
                .Add(new RootValidateMethodHandler())
                .Add(new MemberValidationMethodHandler(compilation))
                .Add(new MethodsRootValidatedHandler())
                .Add(new ParameterValidationMethodHandler(compilation));

                var sb = chain.Handle(new HandlerParams(target, context, classSymbol));
                var namespacePath = classSymbol.ContainingNamespace.ToDisplayString().Replace('.', '/');
                var fileName = $"{classSymbol.Name}_Validation.g.cs";
                context.AddSource($"{namespacePath}/{fileName}", SourceText.From(sb.ToString(), Encoding.UTF8));



            }
            catch (Exception ex)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor("EVGEN001", "Validation Class Generation Error", "Error generating validation class for {0}: {1}, Track: {2}", "EasyValidate", DiagnosticSeverity.Error, true),
                   classSymbol.Locations.First(), classSymbol.Name, ex.Message, ex.StackTrace));
                DebuggerUtil.Log($"Error generating validation class for {classSymbol.Name}: {ex.Message}");
                return;
            }
            DebuggerUtil.Log($"Successfully generated validation class for: {classSymbol.Name}");
        }


    }
}