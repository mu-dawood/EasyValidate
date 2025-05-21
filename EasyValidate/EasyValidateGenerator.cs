using System.Linq;
using System.Text;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.CodeAnalysis.Text;
using System.Diagnostics;
using Microsoft.CodeAnalysis;

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

                ValidateAttributeUsage(classSymbolNonNull, compilation, spc);

                var source = GenerateValidationClass(classSymbolNonNull);
                spc.AddSource($"{classSymbolNonNull.Name}_Validation.g.cs", SourceText.From(source, Encoding.UTF8));
            });
        }

        private static string GenerateValidationClass(INamedTypeSymbol classSymbol)
        {
            var namespaceName = classSymbol.ContainingNamespace.IsGlobalNamespace
                ? string.Empty
                : classSymbol.ContainingNamespace.ToDisplayString();

            var sb = new StringBuilder();

            AppendUsings(sb);

            if (!string.IsNullOrEmpty(namespaceName))
            {
                sb.AppendLine($"namespace {namespaceName}");
                sb.AppendLine("{");
            }

            sb.AppendLine($"    public partial class {classSymbol.Name}");
            sb.AppendLine("    {");

            AppendDefaultValidateMethod(sb);
            AppendCustomValidateMethod(sb, classSymbol);

            sb.AppendLine("    }");

            if (!string.IsNullOrEmpty(namespaceName))
            {
                sb.AppendLine("}");
            }

            return sb.ToString();
        }

        private static void AppendUsings(StringBuilder sb)
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using EasyValidate.Abstraction;"); // Added missing using directive
        }

        private static void AppendDefaultValidateMethod(StringBuilder sb)
        {
            sb.AppendLine("        public ValidationResult Validate()");
            sb.AppendLine("        {");
            sb.AppendLine("            return Validate(ValidationResult.GetDefaultFormatter());");
            sb.AppendLine("        }");
        }

        private static void AppendCustomValidateMethod(StringBuilder sb, INamedTypeSymbol cls)
        {
            sb.AppendLine("        public ValidationResult Validate(IFormatter formatter)");
            sb.AppendLine("        {");
            sb.AppendLine("            var result = new ValidationResult(formatter);");

            foreach (var member in cls.GetMembers().OfType<IPropertySymbol>())
            {
                foreach (var attr in member.GetAttributes())
                {
                    var attributeName = attr.AttributeClass?.ToDisplayString();
                    if (string.IsNullOrEmpty(attributeName))
                        continue;

                    var constructorArguments = string.Join(", ", attr.ConstructorArguments.Select(arg =>
                    {
                        return arg.Kind switch
                        {
                            TypedConstantKind.Primitive => arg.Value?.ToString() ?? "null",
                            TypedConstantKind.Enum => $"({arg.Type?.ToDisplayString()}){arg.Value}",
                            TypedConstantKind.Type => $"typeof({arg.Value})",
                            TypedConstantKind.Array => $"new[] {{ {string.Join(", ", arg.Values.Select(v => v.Value?.ToString() ?? "null"))} }}",
                            TypedConstantKind.Error => "null", // Handle error case gracefully
                            _ => "null"
                        };
                    }));

                    var memberName = member.Name;

                    sb.AppendLine($"            result.TryAddError(nameof({memberName}), new {attributeName}({constructorArguments}), (v) => v.Validate(nameof({memberName}), {memberName}));");
                }
            }

            sb.AppendLine("            return result;");
            sb.AppendLine("        }");
        }

        private static void ValidateAttributeUsage(INamedTypeSymbol classSymbol, Compilation compilation, SourceProductionContext context)
        {
            foreach (var member in classSymbol.GetMembers().OfType<IPropertySymbol>())
            {
                foreach (var attr in member.GetAttributes())
                {
                    var attributeType = attr.AttributeClass;
                    if (attributeType == null)
                        continue;

                    // Dynamically retrieve supported types from Validate method overloads
                    var supportedTypes = attributeType
                        .GetMembers()
                        .OfType<IMethodSymbol>()
                        .Where(m => m.Name == "Validate" && m.Parameters.Length > 1)
                        .Select(m => m.Parameters[1].Type as ITypeSymbol) // Explicitly cast to ITypeSymbol
                        .Where(t => t != null) // Filter out null values
                        .Distinct(SymbolEqualityComparer.Default) // Use SymbolEqualityComparer.Default for distinct comparison
                        .ToList();

                    if (!supportedTypes.Any())
                    {
                        // If no Validate method overloads are found, allow the attribute to apply to all types
                        continue;
                    }

                    // Check if the attribute has a generic Validate<T> method
                    bool hasGenericValidate = attributeType
                        .GetMembers()
                        .OfType<IMethodSymbol>()
                        .Any(m => m.Name == "Validate" && m.IsGenericMethod);

                    if (hasGenericValidate)
                    {
                        // Skip validation errors if the attribute has a generic Validate<T> method
                        continue;
                    }

                    bool isCompatible = supportedTypes
                        .OfType<ITypeSymbol>() // Ensure all elements are ITypeSymbol
                        .Any(supportedType =>
                            member.Type.Equals(supportedType, SymbolEqualityComparer.Default) ||
                            IsAssignableTo(member.Type, supportedType));

                    if (!isCompatible)
                    {
                        var diagnostic = Diagnostic.Create(
                            new DiagnosticDescriptor(
                                id: "EV001",
                                title: "Invalid Attribute Usage",
                                messageFormat: $"{attributeType.Name} can only be applied to types : {string.Join(", ", supportedTypes.Select(t => t.Name))}.",
                                category: "Usage",
                                DiagnosticSeverity.Error,
                                isEnabledByDefault: true
                            ),
                            member.Locations.FirstOrDefault()
                        );

                        context.ReportDiagnostic(diagnostic);
                    }
                }
            }
        }

        private static bool IsAssignableTo(ITypeSymbol fromType, ITypeSymbol toType)
        {
            if (fromType.Equals(toType, SymbolEqualityComparer.Default))
                return true;

            if (toType.TypeKind == TypeKind.Interface)
            {
                return fromType.AllInterfaces.Any(i => i.Equals(toType, SymbolEqualityComparer.Default));
            }

            var baseType = fromType.BaseType;
            while (baseType != null)
            {
                if (baseType.Equals(toType, SymbolEqualityComparer.Default))
                    return true;

                baseType = baseType.BaseType;
            }

            return false;
        }

        private static void LogDebug(string message)
        {
#if DEBUG
            Debugger.Log(0, "Debug", $"[DEBUG] {message}\n");
#endif
        }
    }
}