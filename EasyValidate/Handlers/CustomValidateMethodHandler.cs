using Microsoft.CodeAnalysis;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System;

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
                    .Any(subMember => subMember.GetAttributes().Any(attr => attr.AttributeClass?.BaseType?.ToDisplayString() == "EasyValidate.Attributes.ValidationAttributeBase"));


                foreach (var attr in member.GetAttributes())
                {
                    var attributeClass = attr.AttributeClass;
                    if (attributeClass?.BaseType?.ToDisplayString() != "EasyValidate.Attributes.ValidationAttributeBase")
                        continue;

                    var constructorArguments = new List<string>();
                    foreach (var arg in attr.ConstructorArguments)
                    {
                        constructorArguments.Add(FormatArgument(arg));
                    }

                    var memberName = member.Name;

                    sb.AppendLine($"            result.TryAddError(nameof({memberName}), new {attributeClass.ToDisplayString()}({string.Join(", ", constructorArguments)}), (v) => v.Validate(nameof({memberName}), {memberName}));");
                }
                if (hasValidationAttributes)
                {
                    sb.AppendLine($"            if ({member.Name} != null) result.Merge(nameof({member.Name}), {member.Name}.Validate(formatter));");
                }
            }

            sb.AppendLine("            return result;");
            sb.AppendLine("        }");

            base.Handle(classSymbol, context, sb);
        }

        private static readonly Dictionary<Type, Func<object, string>> PrimitiveFormatters = new()
        {
            { typeof(bool), value => (bool)value ? "true" : "false" },
            { typeof(string), value => $"\"{value}\"" },
            { typeof(char), value => $"\'{value}\'" },
            { typeof(int), value => value.ToString() },
            { typeof(double), value => value.ToString() },
            { typeof(float), value => value.ToString() },
            { typeof(long), value => value.ToString() },
            { typeof(short), value => value.ToString() },
            { typeof(byte), value => value.ToString() },
            { typeof(decimal), value => value.ToString() },
            { typeof(uint), value => value.ToString() },
            { typeof(ulong), value => value.ToString() },
            { typeof(ushort), value => value.ToString() },
            { typeof(sbyte), value => value.ToString() }
        };

        private string FormatArgument(TypedConstant arg)
        {
            return arg.Kind switch
            {
                TypedConstantKind.Primitive => arg.Value != null && PrimitiveFormatters.TryGetValue(arg.Value.GetType(), out var formatter)
                    ? formatter(arg.Value)
                    : arg.Value?.ToString() ?? "null",
                TypedConstantKind.Enum => $"({arg.Type?.ToDisplayString()}){arg.Value}",
                TypedConstantKind.Type => $"typeof({arg.Value})",
                TypedConstantKind.Array => FormatArrayArgument(arg),
                TypedConstantKind.Error => "null", // Handle error case gracefully
                _ => "null"
            };
        }

        private string FormatArrayArgument(TypedConstant arg)
        {
            var arrayValues = arg.Values.Select(FormatArgument).ToList();
            return $"new[] {{ {string.Join(", ", arrayValues)} }}";
        }
    }
}
