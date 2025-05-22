using Microsoft.CodeAnalysis;
using System.Text;
using System.Collections.Generic;

namespace EasyValidate.Handlers
{
    public class GenerateValidationClassHandler : ValidationHandlerBase
    {


        public override void Handle(INamedTypeSymbol classSymbol, SourceProductionContext context, StringBuilder sb)
        {

            var namespaceName = classSymbol.ContainingNamespace.IsGlobalNamespace
                ? string.Empty
                : classSymbol.ContainingNamespace.ToDisplayString();

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

            base.Handle(classSymbol, context, sb);
        }
        private static void AppendUsings(StringBuilder sb)
        {
            sb.AppendLine("using System;");
            sb.AppendLine("using System.Collections.Generic;");
            sb.AppendLine("using EasyValidate.Abstraction;");
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

                    var constructorArguments = new List<string>();
                    foreach (var arg in attr.ConstructorArguments)
                    {
                        if (arg.Kind == TypedConstantKind.Array)
                        {
                            var arrayValues = new List<string>();
                            foreach (var value in arg.Values)
                            {
                                arrayValues.Add(value.Value?.ToString() ?? "null");
                            }
                            constructorArguments.Add($"new[] {{ {string.Join(", ", arrayValues)} }}");
                        }
                        else
                        {
                            constructorArguments.Add(arg.Kind switch
                            {
                                TypedConstantKind.Primitive => arg.Value?.ToString() ?? "null",
                                TypedConstantKind.Enum => $"({arg.Type?.ToDisplayString()}){arg.Value}",
                                TypedConstantKind.Type => $"typeof({arg.Value})",
                                TypedConstantKind.Error => "null", // Handle error case gracefully
                                _ => "null"
                            });
                        }
                    }

                    var memberName = member.Name;

                    sb.AppendLine($"            result.TryAddError(nameof({memberName}), new {attributeName}({string.Join(", ", constructorArguments)}), (v) => v.Validate(nameof({memberName}), {memberName}));");
                }
            }

            sb.AppendLine("            return result;");
            sb.AppendLine("        }");
        }
    }
}
