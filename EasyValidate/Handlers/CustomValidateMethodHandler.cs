using Microsoft.CodeAnalysis;
using System.Text;
using System.Collections.Generic;

namespace EasyValidate.Handlers
{
    public class CustomValidateMethodHandler : ValidationHandlerBase
    {
        public override void Handle(INamedTypeSymbol classSymbol, SourceProductionContext context, StringBuilder sb)
        {
            sb.AppendLine("        public ValidationResult Validate(IFormatter formatter)");
            sb.AppendLine("        {");
            sb.AppendLine("            var result = new ValidationResult(formatter);");

            foreach (var member in classSymbol.GetMembers().OfType<IPropertySymbol>())
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

            base.Handle(classSymbol, context, sb);
        }
    }
}
