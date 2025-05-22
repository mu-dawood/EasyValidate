using Microsoft.CodeAnalysis;
using System.Text;

namespace EasyValidate.Handlers
{
    public class ClassDeclarationHandler : ValidationHandlerBase
    {
        public override void Handle(INamedTypeSymbol classSymbol, SourceProductionContext context, StringBuilder sb)
        {
            sb.AppendLine($"    public partial class {classSymbol.Name} : EasyValidate.Abstraction.IValidate");
            sb.AppendLine("    {");

            base.Handle(classSymbol, context, sb);

            sb.AppendLine("    }");
        }
    }
}
