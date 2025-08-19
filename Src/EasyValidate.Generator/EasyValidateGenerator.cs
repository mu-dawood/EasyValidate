

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyValidate.Handlers;
using EasyValidate.Handlers.Methods;
using EasyValidate.Generator.Helpers;
using EasyValidate.Generator.Types;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;

namespace EasyValidate.Generator
{
    /// <summary>
    /// Roslyn source generator for EasyValidate.
    /// Scans classes implementing <c>IGenerate</c> and generates validation logic and helper methods for annotated members and parameters.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This generator automatically produces validation classes and methods for types decorated with EasyValidate attributes.
    /// It supports member and parameter validation, method overloads, and integrates with the EasyValidate analyzer and code fix ecosystem.
    /// </para>
    /// <para>
    /// <b>Key features:</b>
    /// <list type="bullet">
    /// <item>Generates validation logic for properties, fields, and method parameters</item>
    /// <item>Supports chain validation and reusable instances</item>
    /// <item>Handles error reporting and diagnostic integration</item>
    /// </list>
    /// </para>
    /// </remarks>
    [Generator]
    public class EasyValidateGenerator : IIncrementalGenerator
    {
        private readonly DiagnosticDescriptor PuplicMethodConfusionRule = new(
            ErrorIds.PublicMethodCanCauseConfusion,
            "Public Method Can Cause Confusion",
            "Public method '{0}' with validation attributes can cause confusion in validation processing. Consider making it private or internal.",
            "Usage",
            DiagnosticSeverity.Warning,
            isEnabledByDefault: true,
            description: "Public methods with validation attributes can lead to unexpected behavior in validation processing."
        );

        /// <summary>
        /// Initializes the EasyValidate source generator and registers syntax providers and output actions.
        /// </summary>
        /// <param name="context">The generator initialization context.</param>
        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            DebuggerUtil.AttachDebugger();
            DebuggerUtil.Log("Initializing EasyValidateGenerator...");
            DebuggerUtil.Log("Starting Initialize method.");

            var compilationProvider = context.CompilationProvider;
            var projectDirProvider = context.AnalyzerConfigOptionsProvider.Select((opts, _) =>
            {
                if (!opts.GlobalOptions.TryGetValue("build_property.projectdir", out var root) &&
                    !opts.GlobalOptions.TryGetValue("build_property.msbuildprojectdirectory", out root))
                    root = null;
                return root;
            });
            var candidates = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (node, _) => node is ClassDeclarationSyntax,
                    transform: static (ctx, _) => ctx.SemanticModel.GetDeclaredSymbol((ClassDeclarationSyntax)ctx.Node)
                )
                .Where(static symbol => symbol != null)
                .Combine(compilationProvider)
                 .Combine(projectDirProvider);



            context.RegisterSourceOutput(candidates, (spc, pair) =>
            {
                var ((classSymbol, compilation), projectDir) = pair;

                if (classSymbol is not INamedTypeSymbol classSymbolNonNull)
                    return;
                if (!classSymbolNonNull.ImplementsIGenerate())
                    return;

                DebuggerUtil.Log($"Processing class: {classSymbolNonNull.Name}");
                GenerateValidationClass(classSymbolNonNull, compilation, spc, projectDir);
            });

            DebuggerUtil.Log("Finished setting up syntax provider and compilation provider.");
            DebuggerUtil.Log("Finished Initialize method.");
        }

        /// <summary>
        /// Generates the validation class and methods for a given type, adding the source to the compilation.
        /// </summary>
        /// <param name="classSymbol">The class symbol to generate validation for.</param>
        /// <param name="compilation">The current compilation context.</param>
        /// <param name="context">The source production context.</param>
        /// <param name="projectDir">The project directory, if available.</param>
        private void GenerateValidationClass(INamedTypeSymbol classSymbol, Compilation compilation, SourceProductionContext context, string? projectDir)
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
                    // get parameters for methods to make validation for it
                    if (member is IMethodSymbol method && method.MethodKind == MethodKind.Ordinary)
                    {
                        var methodParameters = method.Parameters;
                        if (methodParameters.Length > 0)
                        {
                            // Process method parameters instead of all class members
                            var parameterInfos = finalizer.Finalize(methodParameters, compilation);
                            if (parameterInfos.Count > 0)
                            {
                                methodTargets.Add(new MethodTarget(method, parameterInfos));
                                if (method.DeclaredAccessibility == Accessibility.Public && method.DeclaredAccessibility == Accessibility.ProtectedOrInternal)
                                {
                                    // Register diagnostic for public methods with validation attributes
                                    context.ReportDiagnostic(Diagnostic.Create(
                                        PuplicMethodConfusionRule,
                                        method.Locations.FirstOrDefault(),
                                        method.Name
                                    ));
                                }
                            }
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

                var hintName = classSymbol.GetMirroredHintName(projectDir);
                context.AddSource(hintName, SourceText.From(sb.ToString(), Encoding.UTF8));
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