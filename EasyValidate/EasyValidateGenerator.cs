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
            var candidates = context.SyntaxProvider
                .CreateSyntaxProvider(
                    predicate: static (node, _) => node is ClassDeclarationSyntax,
                    transform: static (ctx, _) => ctx.SemanticModel.GetDeclaredSymbol((ClassDeclarationSyntax)ctx.Node) as INamedTypeSymbol
                )
                .Where(static symbol => symbol != null);

            context.RegisterSourceOutput(candidates, static (spc, symbol) =>
            {
                if (symbol is not INamedTypeSymbol classSymbol)
                    return;

                var source = GenerateValidationClass(classSymbol);
                spc.AddSource($"{classSymbol.Name}_Validation.g.cs", SourceText.From(source, Encoding.UTF8));
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

                    var memberName = member.Name;

                    sb.AppendLine($"            var validator = new {attributeName}();");
                    sb.AppendLine($"            result.TryAddError(nameof({memberName}), validator, validator.Validate(nameof({memberName}), {memberName}));");
                }
            }

            sb.AppendLine("            return result;");
            sb.AppendLine("        }");
        }
    }
}