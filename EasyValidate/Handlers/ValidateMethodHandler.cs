using Microsoft.CodeAnalysis;
using System.Text;
using EasyValidate.Handlers.Validation;

namespace EasyValidate.Handlers
{
    internal class ValidateMethodHandler(Compilation compilation) : ValidationHandlerBase
    {
        private readonly ValidationAttributeProcessorHandler _processor = new ValidationAttributeProcessorHandler(compilation);

        public override void Handle(INamedTypeSymbol classSymbol, SourceProductionContext context, StringBuilder sb)
        {
            sb.AppendLine("        public IValidationResult Validate(IFormatter formatter, IConfigureValidator configureValidator, params string[] parentPath)");
            sb.AppendLine("        {");
            sb.AppendLine("            var result = new ValidationResult(this, parentPath, formatter, configureValidator);");
                        
            // Process properties
            foreach (var member in classSymbol.GetMembers().OfType<IPropertySymbol>())
            {
                if (_processor.ShouldGenerateValidationMethod(member))
                {
                    var methodName = $"Validate{member.Name}";
                    sb.AppendLine($"            {methodName}(result);");
                }
            }

            // Process fields
            foreach (var member in classSymbol.GetMembers().OfType<IFieldSymbol>())
            {
                if (_processor.ShouldGenerateValidationMethod(member))
                {
                    var methodName = $"Validate{member.Name}";
                    sb.AppendLine($"            {methodName}(result);");
                }
            }

            sb.AppendLine("            return result;");
            sb.AppendLine("        }");
            sb.AppendLine();
            base.Handle(classSymbol, context, sb);
        }
    }
}
