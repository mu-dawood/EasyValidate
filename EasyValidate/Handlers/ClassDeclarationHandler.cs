using Microsoft.CodeAnalysis;
using System.Text;

namespace EasyValidate.Handlers
{
    internal class ClassDeclarationHandler : ValidationHandlerBase
    {
        public override void Handle(INamedTypeSymbol classSymbol, SourceProductionContext context, StringBuilder sb)
        {
            sb.AppendLine($"    public partial class {classSymbol.Name} : EasyValidate.Core.Abstraction.IValidate");
            sb.AppendLine("    {");

            base.Handle(classSymbol, context, sb);

            sb.AppendLine("    }");
        }
    }
}
