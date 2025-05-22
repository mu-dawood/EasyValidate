using Microsoft.CodeAnalysis;
using System.Text;

namespace EasyValidate.Handlers
{
    public class DefaultValidateMethodHandler : ValidationHandlerBase
    {
        public override void Handle(INamedTypeSymbol classSymbol, SourceProductionContext context, StringBuilder sb)
        {
            sb.AppendLine("        public ValidationResult Validate()");
            sb.AppendLine("        {");
            sb.AppendLine("            return Validate(ValidationResult.GetDefaultFormatter());");
            sb.AppendLine("        }");

            base.Handle(classSymbol, context, sb);
        }
    }
}
