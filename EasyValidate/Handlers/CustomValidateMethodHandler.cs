using Microsoft.CodeAnalysis;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace EasyValidate.Handlers
{
    public class CustomValidateMethodHandler : ValidationHandlerBase
    {
        public override void Handle(INamedTypeSymbol classSymbol, SourceProductionContext context, StringBuilder sb)
        {
            sb.AppendLine("        public ValidationResult Validate(IFormatter formatter)");
            sb.AppendLine("        {");
            sb.AppendLine("            var result = new ValidationResult(formatter);");
            var iValidateSymbol = classSymbol.ContainingAssembly.GetTypeByMetadataName("EasyValidate.Abstraction.IValidate");
            foreach (var member in classSymbol.GetMembers().OfType<IPropertySymbol>())
            {
                DebuggerUtil.Log($"Processing member: {member.Name}");
                DebuggerUtil.Log($"Member type: {member.Type.ToDisplayString()}");

                // Check if the property's type has any members with attributes derived from ValidationAttributeBase
                var hasValidationAttributes = member.Type.GetMembers()
                    .OfType<IPropertySymbol>()
                    .Any(subMember => subMember.GetAttributes().Any(attr => attr.AttributeClass?.BaseType?.ToDisplayString() == "EasyValidate.Abstraction.Attributes.ValidationAttributeBase"));

                if (hasValidationAttributes)
                {
                    sb.AppendLine($"            if ({member.Name} != null) result.Merge(nameof({member.Name}), {member.Name}.Validate(formatter));");
                }
                else
                {
                    foreach (var attr in member.GetAttributes())
                    {
                        var attributeClass = attr.AttributeClass;
                        if (attributeClass?.BaseType?.ToDisplayString() != "EasyValidate.Abstraction.Attributes.ValidationAttributeBase")
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

                        sb.AppendLine($"            result.TryAddError(nameof({memberName}), new {attributeClass.ToDisplayString()}({string.Join(", ", constructorArguments)}), (v) => v.Validate(nameof({memberName}), {memberName}));");
                    }
                }
            }

            sb.AppendLine("            return result;");
            sb.AppendLine("        }");

            base.Handle(classSymbol, context, sb);
        }
    }
}
