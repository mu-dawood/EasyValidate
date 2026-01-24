

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using EasyValidate.Handlers;
using EasyValidate.Handlers.Methods;
using EasyValidate.Handlers.Constructors;
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
                    predicate: static (node, _) => node.IsClassStructOrRecord(),
                    transform: static (ctx, _) => ctx.SemanticModel.GetDeclaredSymbol((TypeDeclarationSyntax)ctx.Node)
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
        /// <param name="typeSymbol">The class symbol to generate validation for.</param>
        /// <param name="compilation">The current compilation context.</param>
        /// <param name="context">The source production context.</param>
        /// <param name="projectDir">The project directory, if available.</param>
        private void GenerateValidationClass(INamedTypeSymbol typeSymbol, Compilation compilation, SourceProductionContext context, string? projectDir)
        {
            DebuggerUtil.Log($"Generating validation class for: {typeSymbol.Name}");
            var argumentHandler = new AttributeArgumentHandler();
            Dictionary<string, string> instanceNames = [];
            var finalizer = new MembersFinalizer(context, typeSymbol, argumentHandler, instanceNames);
            try
            {
                // Skip classes that have no properties with validation attributes
                // Check if the class has any properties with attributes derived from IValidationAttribute
                var members = typeSymbol.GetMembers().OrderBy(m => m is IFieldSymbol ? 0 : 1).ToList(); // Order properties first, then fields
                var target = new ValidationTarget(typeSymbol);
                var infos = finalizer.Finalize(members, compilation);
                if (infos.Count > 0)
                    target = target.WithMembers(infos);

                List<MethodTarget> methodTargets = [];
                List<ConstructorTarget> constructorTargets = [];
                foreach (var member in typeSymbol.GetMembers())
                {
                    // get parameters for methods to make validation for it
                    if (member is IMethodSymbol method)
                    {
                        if (method.MethodKind == MethodKind.Ordinary || method.MethodKind == MethodKind.Constructor)
                        {
                            var parameters = method.Parameters;
                            if (parameters.Length > 0)
                            {
                                // Process method parameters - include all params for pass-through
                                var parameterInfos = finalizer.Finalize(parameters, compilation, includeAllMembers: true);
                                // Only create target if at least one parameter needs validation
                                if (parameterInfos.Any(p => p.NeedsValidation))
                                {
                                    // Check for GeneratedMethodAttribute
                                    var generatedMethodAttr = method.GetAttributes()
                                        .FirstOrDefault(a => a.AttributeClass.InheritsFrom("EasyValidate.Abstractions.GeneratedMethodAttribute"));

                                    string? customMethodName = null;
                                    string accessModifier = "public";

                                    if (generatedMethodAttr != null)
                                    {
                                        customMethodName = generatedMethodAttr.NamedArguments
                                            .FirstOrDefault(a => a.Key == "MethodName").Value.Value as string;

                                        var accessModifierValue = generatedMethodAttr.NamedArguments
                                            .FirstOrDefault(a => a.Key == "AccessModifier").Value.Value;
                                        if (accessModifierValue != null)
                                        {
                                            accessModifier = ConvertAccessModifier((int)accessModifierValue);
                                        }
                                    }
                                    if (method.MethodKind == MethodKind.Constructor)
                                    {
                                        constructorTargets.Add(new ConstructorTarget(method, parameterInfos, customMethodName ?? "Create", accessModifier));
                                        continue;
                                    }
                                    methodTargets.Add(new MethodTarget(method, parameterInfos, customMethodName, accessModifier));
                                    if (method.DeclaredAccessibility == Accessibility.Public || method.DeclaredAccessibility == Accessibility.ProtectedOrInternal)
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
                }
                if (methodTargets.Count > 0)
                    target = target.WithMethods(methodTargets);

                if (constructorTargets.Count > 0)
                    target = target.WithConstructors(constructorTargets);

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
                .Add(new ParameterValidationMethodHandler(compilation))
                .Add(new ConstructorsRootValidatedHandler())
                .Add(new ConstructorParameterValidationMethodHandler(compilation));

                var sb = chain.Handle(new HandlerParams(target, context, typeSymbol));

                var hintName = typeSymbol.GetMirroredHintName(projectDir);
                context.AddSource(hintName, SourceText.From(sb.ToString(), Encoding.UTF8));
            }
            catch (Exception ex)
            {
                context.ReportDiagnostic(Diagnostic.Create(
                    new DiagnosticDescriptor("EVGEN001", "Validation Class Generation Error", "Error generating validation class for {0}: {1}, Track: {2}", "EasyValidate", DiagnosticSeverity.Error, true),
                   typeSymbol.Locations.First(), typeSymbol.Name, ex.Message, ex.StackTrace));
                DebuggerUtil.Log($"Error generating validation class for {typeSymbol.Name}: {ex.Message}");
                return;
            }
            DebuggerUtil.Log($"Successfully generated validation class for: {typeSymbol.Name}");
        }

        /// <summary>
        /// Converts the AccessModifier enum value to its C# keyword string.
        /// </summary>
        private static string ConvertAccessModifier(int value)
        {
            return value switch
            {
                0 => "public",           // Public
                1 => "internal",         // Internal
                2 => "protected",        // Protected
                3 => "private",          // Private
                4 => "protected internal", // ProtectedInternal
                5 => "private protected",  // PrivateProtected
                _ => "public"
            };
        }
    }
}